using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using LinqToExcel;
using MVC5Bank.Models;

namespace BlogSample.Infrastructure.Helpers
{
    public class ImportDataHelper
    {
        /// <summary>
        /// 檢查匯入的 Excel 資料.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="importZipCodes">The import zip codes.</param>
        /// <returns></returns>
        public CheckResult CheckImportData(
            string fileName,
            List<客戶資料> importZipCodes)
        {
            var result = new CheckResult();

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {
                result.ID = Guid.NewGuid();
                result.Success = false;
                result.ErrorCount = 0;
                result.ErrorMessage = "匯入的資料檔案不存在";
                return result;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //欄位對映
            excelFile.AddMapping<客戶資料>(x => x.Id, "Id");
            excelFile.AddMapping<客戶資料>(x => x.客戶名稱, "客戶名稱");
            excelFile.AddMapping<客戶資料>(x => x.統一編號, "統一編號");
            excelFile.AddMapping<客戶資料>(x => x.電話, "電話");
            excelFile.AddMapping<客戶資料>(x => x.傳真, "傳真");
            excelFile.AddMapping<客戶資料>(x => x.地址, "地址");
            excelFile.AddMapping<客戶資料>(x => x.Email, "Email");
            excelFile.AddMapping<客戶資料>(x => x.Stat, "Stat");
            excelFile.AddMapping<客戶資料>(x => x.客戶分類, "客戶分類");
            excelFile.AddMapping<客戶資料>(x => x.帳號, "帳號");
            excelFile.AddMapping<客戶資料>(x => x.密碼, "密碼");


            //SheetName
            var excelContent = excelFile.Worksheet<客戶資料>("客戶資料");

            int errorCount = 0;
            int rowIndex = 1;
            var importErrorMessages = new List<string>();

            //檢查資料
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var 客戶資料 = new 客戶資料();

                //客戶資料.Id = row.Id;
                客戶資料.客戶名稱 = row.客戶名稱;
                客戶資料.統一編號 = row.統一編號;
                客戶資料.電話 = row.電話;
                客戶資料.傳真 = row.傳真;
                客戶資料.地址 = row.地址;
                客戶資料.Email = row.Email;
                客戶資料.Stat = true;
                客戶資料.客戶分類 = row.客戶分類;
                客戶資料.帳號 = row.帳號;
                客戶資料.密碼 = row.密碼;



                //CityName
                if (string.IsNullOrWhiteSpace(row.客戶名稱))
                {
                    errorMessage.Append("客戶名稱 - 不可空白. ");
                }
                客戶資料.客戶名稱 = row.客戶名稱;

                //Town
                if (string.IsNullOrWhiteSpace(row.統一編號))
                {
                    errorMessage.Append("統一編號 - 不可空白. ");
                }
                客戶資料.統一編號 = row.統一編號;

                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errorCount += 1;
                    importErrorMessages.Add(string.Format(
                        "第 {0} 列資料發現錯誤：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                importZipCodes.Add(客戶資料);
                rowIndex += 1;
            }

            try
            {
                result.ID = Guid.NewGuid();
                result.Success = errorCount.Equals(0);
                result.RowCount = importZipCodes.Count;
                result.ErrorCount = errorCount;

                string allErrorMessage = string.Empty;

                foreach (var message in importErrorMessages)
                {
                    allErrorMessage += message;
                }

                result.ErrorMessage = allErrorMessage;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Saves the import data.
        /// </summary>
        /// <param name="importZipCodes">The import zip codes.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveImportData(IEnumerable<客戶資料> 客戶資料)
        {
            客戶資料Repository repo客戶資料;
            客戶聯絡人Repository repo客戶聯絡人;

            repo客戶資料 = RepositoryHelper.Get客戶資料Repository();
            repo客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository(repo客戶資料.UnitOfWork);


            try
            {

                //再把匯入的資料給存到資料庫

                foreach (var item in 客戶資料)
                {
                    repo客戶資料.Add(item);
                }
                repo客戶資料.UnitOfWork.Commit();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}