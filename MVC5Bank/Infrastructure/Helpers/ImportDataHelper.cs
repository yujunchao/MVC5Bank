﻿using System;
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
        ///// <summary>
        ///// 檢查匯入的 Excel 資料.
        ///// </summary>
        ///// <param name="fileName">Name of the file.</param>
        ///// <param name="importZipCodes">The import zip codes.</param>
        ///// <returns></returns>
        //public CheckResult CheckImportData(
        //    string fileName,
        //    List<客戶資料> importZipCodes)
        //{
        //    var result = new CheckResult();

        //    var targetFile = new FileInfo(fileName);

        //    if (!targetFile.Exists)
        //    {
        //        result.ID = Guid.NewGuid();
        //        result.Success = false;
        //        result.ErrorCount = 0;
        //        result.ErrorMessage = "匯入的資料檔案不存在";
        //        return result;
        //    }

        //    var excelFile = new ExcelQueryFactory(fileName);

        //    //欄位對映
        //    excelFile.AddMapping<客戶資料>(x => x.ID, "ID");
        //    excelFile.AddMapping<客戶資料>(x => x.Zip, "Zip");
        //    excelFile.AddMapping<客戶資料>(x => x.CityName, "CityName");
        //    excelFile.AddMapping<客戶資料>(x => x.Town, "Town");
        //    excelFile.AddMapping<客戶資料>(x => x.Sequence, "Sequence");

        //    //SheetName
        //    var excelContent = excelFile.Worksheet<客戶資料>("臺灣郵遞區號");

        //    int errorCount = 0;
        //    int rowIndex = 1;
        //    var importErrorMessages = new List<string>();

        //    //檢查資料
        //    foreach (var row in excelContent)
        //    {
        //        var errorMessage = new StringBuilder();
        //        var 客戶資料 = new 客戶資料();

        //        客戶資料.ID = row.ID;
        //        客戶資料.Sequence = row.Sequence;
        //        客戶資料.Zip = row.Zip;
        //        客戶資料.CreateDate = DateTime.Now;

        //        //CityName
        //        if (string.IsNullOrWhiteSpace(row.CityName))
        //        {
        //            errorMessage.Append("縣市名稱 - 不可空白. ");
        //        }
        //        客戶資料.CityName = row.CityName;

        //        //Town
        //        if (string.IsNullOrWhiteSpace(row.Town))
        //        {
        //            errorMessage.Append("鄉鎮市區名稱 - 不可空白. ");
        //        }
        //        zipCode.Town = row.Town;

        //        //=============================================================================
        //        if (errorMessage.Length > 0)
        //        {
        //            errorCount += 1;
        //            importErrorMessages.Add(string.Format(
        //                "第 {0} 列資料發現錯誤：{1}{2}",
        //                rowIndex,
        //                errorMessage,
        //                "<br/>"));
        //        }
        //        importZipCodes.Add(zipCode);
        //        rowIndex += 1;
        //    }

        //    try
        //    {
        //        result.ID = Guid.NewGuid();
        //        result.Success = errorCount.Equals(0);
        //        result.RowCount = importZipCodes.Count;
        //        result.ErrorCount = errorCount;

        //        string allErrorMessage = string.Empty;

        //        foreach (var message in importErrorMessages)
        //        {
        //            allErrorMessage += message;
        //        }

        //        result.ErrorMessage = allErrorMessage;

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Saves the import data.
        ///// </summary>
        ///// <param name="importZipCodes">The import zip codes.</param>
        ///// <exception cref="System.NotImplementedException"></exception>
        //public void SaveImportData(IEnumerable<客戶資料> importZipCodes)
        //{
        //    try
        //    {
        //        //先砍掉全部資料
        //        using (var db = new SampleEntities())
        //        {
        //            foreach (var item in db.TaiwanZipCodes.OrderBy(x => x.ID))
        //            {
        //                db.TaiwanZipCodes.Remove(item);
        //            }
        //            db.SaveChanges();
        //        }

        //        //再把匯入的資料給存到資料庫
        //        using (var db = new SampleEntities())
        //        {
        //            foreach (var item in importZipCodes)
        //            {
        //                db.TaiwanZipCodes.Add(item);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}