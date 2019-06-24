using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Bank.ActionFilters
{
    public class ExcuteTimeAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string Controller = filterContext.RouteData.Values["Controller"].ToString() + " ";
            string Action = filterContext.RouteData.Values["Action"].ToString()+" ";
            System.Diagnostics.Debug.WriteLine(Controller + Action + "Action時間:" +DateTime.Now.ToString("mm:ss.ffff"));
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            string Controller = filterContext.RouteData.Values["Controller"].ToString() + " ";
            string Action = filterContext.RouteData.Values["Action"].ToString() + " ";
            System.Diagnostics.Debug.WriteLine(Controller + Action + "Result時間:" + DateTime.Now.ToString("mm:ss.ffff"));
            base.OnResultExecuted(filterContext);
        }
    }
}