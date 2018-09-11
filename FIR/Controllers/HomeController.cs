using Microsoft.Ajax.Utilities;
using ENVHR_CSHARP.App_Code;
using ENVHR_CSHARP.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace ENVHR_CSHARP.Controllers
{
    public class HomeController : BaseController
    {
        public LogWriter aLogWriter = LogWriter.Instance;
       
        public ContentResult TimedOut()
        {
            try {
                var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/TimedOut.txt"));
                return Content(fileContents);
            }catch(Exception ex)
            {
                aLogWriter.WriteLog(String.Format("TimedOut(){0}", ex.ToString()));
            }
            return Content("<H1>Timed Out ! Refresh your page !!");
        }
      
        [HttpPost]
        public ActionResult Impersonate()
        {
            return View();
        }
        [HttpPost] 
        public ActionResult UserImpersonate(string JSONString)
        {
            aLogWriter.WriteLog(String.Format("UserImpersonate(){0}", JSONString));
            Newtonsoft.Json.Linq.JObject PostData = Newtonsoft.Json.Linq.JObject.Parse(JSONString);
            string SessionId = new Guid().ToString();
            Session["GENERATED_SESSION_ID"] = SessionId;
            UserRepository userRepository = new UserRepository(PostData["UserName"].ToString(), "", SessionId);
            userRepository.Login();
            if (userRepository.aResult.retcode == 0)
            {
                Session["IMP_USER"] = PostData["UserName"].ToString();
                Session["TOKEN"] = userRepository.aUserProfile.Token;
                Session["DIV_CODE"] = userRepository.aUserProfile.DivCode;
                Session["ROLE"] = userRepository.aUserProfile.RoleToken;
                Session["USERID"] = userRepository.aUserProfile.UserName; 
                ViewBag.RoleName = userRepository.aUserProfile.RoleToken;
                aLogWriter.WriteLog(String.Format("Index() Token:{0} DIV_CODE:{1} ROLE:{2} USERID:{3}", Session["TOKEN"],
                                                    Session["DIV_CODE"], Session["ROLE"], Session["USERID"]));
                return View("SuccessView");

            }
            else
            {
                aLogWriter.WriteLog(String.Format("UserImpersonate Error:{0}", userRepository.aResult.result));
                ViewBag.ErrorMessage = userRepository.aResult.result;
                return View("ErrorView");
            }
        }
        public ActionResult DashBoard()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult DashView()
        {
            //partial view for Dashboard, loaded into the #content within DASHBOARD
            return View("DashView");
        } 
        
        [HttpGet]
        public ActionResult Index()
        {
            string UserName = new Utility().value("Impersonate");
            string SessionId = System.Web.HttpContext.Current.Session.SessionID;

            if (UserName == "")
                UserName = System.Web.HttpContext.Current.User.Identity.Name;

            if (Session["IMP_USER"] != null && Session["IMP_USER"].ToString() != "")
            {
                UserName = Session["IMP_USER"].ToString();
                SessionId = Session["GENERATED_SESSION_ID"].ToString();
            }

           
            UserRepository userRepository = new UserRepository(UserName, "", SessionId);
            userRepository.Login();
            if (userRepository.aResult.retcode == 0)
            {
                Session["TOKEN"] = userRepository.aUserProfile.Token;
                Session["DIV_CODE"] = userRepository.aUserProfile.DivCode;
                Session["ROLE"] = userRepository.aUserProfile.RoleToken;
                Session["USERID"] = userRepository.aUserProfile.UserName;
                aLogWriter.WriteLog(String.Format("Index() Token:{0} DIV_CODE:{1} ROLE:{2} USERID:{3}",  Session["TOKEN"],
                                                   Session["DIV_CODE"], Session["ROLE"], Session["USERID"]));
                if (userRepository.aUserProfile.RoleToken == "ADMIN")
                {
                    ViewBag.RoleName = "ADMIN";

                }
                else

                {
                    aLogWriter.WriteLog(String.Format("Index() Error:{0}", userRepository.aResult.result));
                    ViewBag.RoleName = userRepository.aUserProfile.RoleToken;
                }
                return View("DashBoard");

            }
            else
            {
                ViewBag.ErrorMessage = userRepository.aResult.result;
                return View();
            }
        }
        public ActionResult Error()
        {
            return View("Error");
        }

        [HttpPost]
        public ActionResult Datasources()
        {
            // ViewBag.guid = new Guid();

            return View("Datasources");
        }
        public ActionResult RolesManagement()
        {
            return View("RolesManagement");
        }
        public ActionResult ReportCategories()
        {
            return View("ReportCategories");
        }
        public ActionResult ReportAccess()
        {
            return View("ReportAccess");
        }
        public ActionResult DatasourceAccess()
        {
            return View("DatasourceAccess");
        }
        public ActionResult DatabaseSource()
        {
            return View("DatabaseSource");
        }
        public ActionResult HelpHeading()
        {
            return View("HelpHeading");
        }
        
        
        [HttpGet]
        public ActionResult SaveImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveImage(string file)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                    var fileName = Path.GetFileName(filebase.FileName);
                    var path = Path.Combine(Server.MapPath("~/HelpImage/"), fileName);
                    filebase.SaveAs(path);
                    return Json(fileName);
                }
                else { return Json("No File Saved."); }
            }
            catch (Exception)
            {
                return Json("Error While Saving.");
            }
        }  
    }
}




