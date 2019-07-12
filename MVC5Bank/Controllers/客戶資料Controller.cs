using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BlogSample.Infrastructure.Helpers;
using MVC5Bank.ActionFilters;
using MVC5Bank.Models;
using MVC5Bank.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVC5Bank.Controllers
{
    [ExcuteTime]
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo客戶資料;
        客戶聯絡人Repository repo客戶聯絡人;
        public 客戶資料Controller()
        {
            repo客戶資料 = RepositoryHelper.Get客戶資料Repository();
            repo客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository(repo客戶資料.UnitOfWork);
        }

        public ActionResult Order(string order)
        {
            if (!string.IsNullOrEmpty(order))
            {
                return View("Index", repo客戶資料.Order(order));
            }
            return View("Index", repo客戶資料.All().ToList());


        }
        private string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];

        public ActionResult date()
        {
            int weekNumber = 28;
            DateTime firstDateOfYear = new DateTime(2019, 1, 1);
            DateTime dayInWeek = firstDateOfYear.AddDays((weekNumber - 1) * 7);

            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != DayOfWeek.Sunday)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            DateTime lastModifyDayinWeek = firstDayInWeek.AddDays(5);
            lastModifyDayinWeek.AddHours(17);
            return View();
        }

        #region --Export--
        public ActionResult HasData()
        {
            JObject jo = new JObject();
            bool result = repo客戶資料.All().Count().Equals(0);
            jo.Add("Msg", result.ToString());
            return Content(JsonConvert.SerializeObject(jo), "application/json");

        }
        public ActionResult Export()
        {
            //var exportSpource = this.GetExportData();
            //var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            //var exportFileName = string.Concat(
            //    "客戶資料",
            //    DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    ".xlsx");
            //return new ExportExcelResult
            //{
            //    SheetName = "客戶資料",
            //    FileName = exportFileName,

            //    ExportData = dt
            //};


            DataTable dt = ExcelUtility.ConvertObjectsToDataTable(repo客戶資料.All().
                Select(x => new { x.客戶名稱, x.統一編號, x.電話, x.傳真, x.地址, x.Email, x.客戶分類, x.帳號 }).ToList());
            System.IO.MemoryStream stream = ExcelUtility.ExportExcelStreamFromDataTable(dt);
            FileContentResult fResult = new FileContentResult(stream.ToArray(), "application/x-xlsx");
            fResult.FileDownloadName = "test.xlsx";
            return fResult;
        }

        #endregion
        #region --Upload--
        public ActionResult Upload(HttpPostedFileBase file)
        {
            JObject jo = new JObject();
            string result = string.Empty;

            if (file == null)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳檔案!");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }
            if (file.ContentLength <= 0)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳正確的檔案.");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            string fileExtName = Path.GetExtension(file.FileName).ToLower();

            if (!fileExtName.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                &&
                !fileExtName.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳 .xls 或 .xlsx 格式的檔案");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            try
            {
                var uploadResult = this.FileUploadHandler(file);

                jo.Add("Result", !string.IsNullOrWhiteSpace(uploadResult));
                jo.Add("Msg", !string.IsNullOrWhiteSpace(uploadResult) ? uploadResult : "");

                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                jo.Add("Result", false);
                jo.Add("Msg", ex.Message);
                result = JsonConvert.SerializeObject(jo);
            }
            return Content(result, "application/json");
        }

        /// <summary>
        /// Files the upload handler.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file;上傳失敗：沒有檔案！</exception>
        /// <exception cref="System.InvalidOperationException">上傳失敗：檔案沒有內容！</exception>
        private string FileUploadHandler(HttpPostedFileBase file)
        {
            string result;

            if (file == null)
            {
                throw new ArgumentNullException("file", "上傳失敗：沒有檔案！");
            }
            if (file.ContentLength <= 0)
            {
                throw new InvalidOperationException("上傳失敗：檔案沒有內容！");
            }

            try
            {
                string virtualBaseFilePath = Url.Content(fileSavedPath);
                string filePath = HttpContext.Server.MapPath(virtualBaseFilePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string newFileName = string.Concat(
                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Path.GetExtension(file.FileName).ToLower());

                string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath), newFileName);
                file.SaveAs(fullFilePath);

                result = newFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion
        #region --Import--
        [HttpPost]
        public ActionResult Import(string savedFileName)
        {
            var jo = new JObject();
            string result;

            try
            {
                var fileName = string.Concat(Server.MapPath(fileSavedPath), "/", savedFileName);

                var importZipCodes = new List<客戶資料>();

                var helper = new ImportDataHelper();
                var checkResult = helper.CheckImportData(fileName, importZipCodes);

                jo.Add("Result", checkResult.Success);
                jo.Add("Msg", checkResult.Success ? string.Empty : checkResult.ErrorMessage);

                if (checkResult.Success)
                {
                    //儲存匯入的資料
                    helper.SaveImportData(importZipCodes);
                }
                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Content(result, "application/json");
        }
        #endregion
        [HttpPost]
        public ActionResult GetCustomerName()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var selection = repo客戶資料.All().Select(x => x.客戶名稱);
            foreach (var item in selection)
            {
                items.AddRange(new[] { new SelectListItem() { Text = item, Value = item, Selected = false } });
            }
            return this.Json(items);        


        }

        // GET: 客戶資料
        public ActionResult Index()
        {
            return View(repo客戶資料.All().ToList());
        }

        [HttpPost]
        public ActionResult Index(string keyword,string name)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(keyword+name))
                {
                    System.Diagnostics.Debug.WriteLine(name);
                    return View(repo客戶資料.NamenClassification(keyword, name));
                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(keyword);
                    return View(repo客戶資料.Classification(keyword));
                }
                
            }


            return View(repo客戶資料.All().ToList());


        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,Stat,客戶分類,帳號,密碼")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.PasswordHash();
                repo客戶資料.Add(客戶資料);
                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            客戶資料.密碼 = "";

            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶資料 = repo客戶資料.Find(id);
            var oldPW = 客戶資料.密碼;

            if (TryUpdateModel(客戶資料))
            {
                if (!String.IsNullOrEmpty(客戶資料.密碼))
                {
                    客戶資料.PasswordHash();
                }
                else
                {
                    客戶資料.密碼 = oldPW;
                }

                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo客戶資料.Find(id);
            repo客戶資料.Delete(客戶資料);
            repo客戶資料.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult BatchUpdate(int? id, IList<ContactVM> data)
        {
            if (ModelState.IsValid)
            {


                foreach (var item in data)
                {
                    var result = repo客戶聯絡人.Find(item.Id);
                    if (result != null)
                    {
                        result.職稱 = item.職稱;
                        result.電話 = item.電話;
                        result.手機 = item.手機;
                    }
                }
                repo客戶聯絡人.UnitOfWork.Commit();
                return RedirectToAction("Details", new { id = id });
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View("Details", 客戶資料);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo客戶資料.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
