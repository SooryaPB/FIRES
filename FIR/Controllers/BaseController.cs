using ENVHR_CSHARP.App_Code;
using System;
using System.Collections.Generic;

using System.Web;
using System.Web.Mvc;

namespace ENVHR_CSHARP.Controllers
{
    public class BaseController : Controller
    {
        public LogWriter aLogWriter = LogWriter.Instance;
        public BaseController()
        {
            aLogWriter.SetLogFile(new Utility().value("LogFile"));
        }
        protected override bool DisableAsyncSupport
        {
            get
            {
                return true;
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["TOKEN"] == null &&  ( !filterContext.ActionDescriptor.ActionName.Equals("Index")))
            { 
                filterContext.Result = new RedirectResult("TimedOut");
            }
            else
                base.OnActionExecuting(filterContext); 
        }
        protected override void ExecuteCore()
        { 

            try
            {  
                base.ExecuteCore(); 
            }
            catch (Exception ex)
            {
                aLogWriter.WriteLog(String.Format("ExecuteCore(){0}",ex.ToString()));
                RedirectToAction("/Home/Index");
            }
            finally
            {


            }


        }

    }
}
