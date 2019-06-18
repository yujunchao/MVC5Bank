using MVC5Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Bank.Controllers
{
    public class BackStageController : Controller
    {
        客戶資料Repository repo客戶資料 = RepositoryHelper.Get客戶資料Repository();
        // GET: BackStage
        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                var data = repo客戶資料.Find(Convert.ToInt32(User.Identity.Name));
                var model = new BackStageDataVM();
            
            
            if (User.Identity.Name!="0")
            {
                model.傳真 = data.傳真;
                model.地址 = data.地址;
                model.密碼 = data.密碼;
                model.電話 = data.電話;
                model.Email = data.Email;
                model.密碼 = "";
            }
            
            
            return View(model);
            }
            else
            {
                return Redirect("/Home/Index/");
            }
        }
        [HttpPost]
        public ActionResult Index(BackStageDataVM item)
        {
            if (ModelState.IsValid)
            {
                var data = repo客戶資料.Find(Convert.ToInt32(User.Identity.Name));
                
                if (User.Identity.Name != "0")
                {
                    data.傳真 = item.傳真;
                    data.地址 = item.地址;
                    data.密碼 = item.密碼;
                    data.電話 = item.電話;
                    data.Email = item.Email;
                    if (!String.IsNullOrEmpty(data.密碼))
                    {
                        data.PasswordHash();

                    }
                  
                }
                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}