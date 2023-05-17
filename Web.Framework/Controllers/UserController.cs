using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.ActionFilters;
using Web.Framework.General;
using Web.Framework.Models;
using Web.Framework.Server;

namespace Web.Framework.Controllers
{
    public class UserController : BaseController
    {
        protected ModelUser model { get; set; }

        public UserController() : base(new ModelUser())
        {
            ViewBag.Style = "Default";
            model = (ModelUser) Model;
        }

        [ActionFilterAdmin]
        public override ActionResult Index()
        {
            return base.Index();
        }

        public virtual ActionResult Login(string error)
        {
            ViewBag.Body = model.ShowLogin();
            return View();
        }

        [HttpPost]
        public virtual ActionResult Verify(String username, string password, string rememberme)
        {
            ServerBase server = new ServerBase("user");
            server.SelectFilter("username", username);

            Encrypt64 encrypt = new Encrypt64();
            server.SelectFilter("password", encrypt.Encrypt(password));
            server.SelectLimit(1);

            DataTable data = server.SelectQuery();

            if (data.Rows.Count != 0)
            {
                string userid = data.Rows[0]["id"].ToString();

                UpdateSession(data);

                HttpCookie cookieid = new HttpCookie("userid");
                cookieid.Value = userid;

                if (rememberme != null)
                    cookieid.Expires = DateTime.Now.AddDays(30);

                System.Web.HttpContext.Current.Response.Cookies.Add(cookieid);

                return Redirect("../");
            }

            string message = "";
            return Redirect("../account/login?error=" + message);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            ViewBag.Body = model.ShowNew();
            return View();
        }

        [HttpPost]
        public override ActionResult Insert()
        {
            model.Insert(Request);
            return Redirect("../");
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Logout()
        {
            ClearSession();
            return RedirectToAction("../");
        }

        private void ClearSession()
        {
            Session.Clear();

            HttpCookie cookieguid = new HttpCookie("guid");
            cookieguid.Expires = DateTime.Now.AddDays(-1);

            HttpCookie cookieid = new HttpCookie("userid");
            cookieid.Expires = DateTime.Now.AddDays(-1);

            System.Web.HttpContext.Current.Response.Cookies.Add(cookieguid);
            System.Web.HttpContext.Current.Response.Cookies.Add(cookieid);
        }
    }
}