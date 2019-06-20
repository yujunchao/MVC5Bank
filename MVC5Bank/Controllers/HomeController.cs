using MVC5Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Bank.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM form)
        {
            if (ModelState.IsValid)
            {
                int id = -1;
                if (form.Username == "admin")
                {
                    if (form.Pssword == "123")
                    {
                        id = 0;
                        
                    }
                }
                else
                {
                    var repo客戶資料 = RepositoryHelper.Get客戶資料Repository();
                    var hashPW = FormsAuthentication.HashPasswordForStoringInConfigFile(form.Pssword, "SHA1");

                    var item = repo客戶資料.All().SingleOrDefault(p => p.帳號 == form.Username && p.密碼 == hashPW);

                    if (item != null)
                    {
                        id = item.Id;
                    }

                }
                if (id >= 0)
                {
                    FormsAuthentication.RedirectFromLoginPage(id.ToString(), false);
                    return RedirectToAction("Index");

                }
            }
            

            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}