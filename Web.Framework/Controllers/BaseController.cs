using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.ActionFilters;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Server;

namespace Web.Framework.Controllers
{
    [ActionFilterBase]
    public class BaseController : Controller
    {
        public ModelBase Model { get; set; }

        public virtual string Username
        {
            get
            {
                if (Session["_username_"] != null)
                    return Session["_username_"].ToString();
                else
                    return null;
            }
            set { Session["_username_"] = value; }
        }

        public virtual string Name
        {
            get
            {
                if (Session["_name_"] != null)
                    return Session["_name_"].ToString();
                else
                    return null;
            }
            set { Session["_name_"] = value; }
        }

        public virtual string UserID
        {
            get
            {
                if (Session["_userid_"] != null)
                    return Session["_userid_"].ToString();
                else
                    return null;
            }
            set { Session["_userid_"] = value; }
        }

        public string SecureID  {
            get { return (string)Session["_secureid_"]; }
            set { Session["_secureid_"] = value; }
        }

        public virtual string UserType
        {
            get
            {
                if (Session["_usertype_"] != null)
                    return Session["_usertype_"].ToString();
                else
                    return null;
            }
            set { Session["_usertype_"] = value; }
        }

        public bool IsAuthenticated  { get { if (Username != null) return true; else return false; } }

        public Dictionary<string, string> Apps { get; set; }

        public BaseController(ModelBase model)
        {
            ViewBag.Style = "Default";

            model.Controller = this;
            Model = model;
        }

        public virtual void Initialize()
        {
        }

        public virtual ActionResult Index()
        {
            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.Home();
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Initialize model
                Model.Initialize();

                Model.ShowLeft();

                ViewBag.Body = Model.Home();
                ViewBag.Left = Model.Left.ToString();
                //ViewBag.Right = Model.Right.ToString();

                return View();
            }
        }

        public virtual ActionResult Detail()
        {
            Model.ShowDetail();

            ViewBag.Body = Model.Body.ToString();
            ViewBag.Left = Model.Left.ToString();
            ViewBag.Right = Model.Right.ToString();

            return View();
        }

        [ActionFilterAdmin]
        public virtual ActionResult New()
        {
            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.ShowNew();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Initialize model
                Model.Initialize();
                ViewBag.Body = Model.ShowNew();

                return View();
            }
        }

        [ActionFilterAdmin]
        public virtual ActionResult Edit()
        {
            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.ShowEdit();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Initialize model
                Model.Initialize();
                ViewBag.Body = Model.ShowEdit();

                return View();
            }
        }

        [ActionFilterAdmin]
        public virtual ActionResult Delete()
        {
            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.ShowDelete();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Initialize model
                Model.Initialize();
                ViewBag.Body = Model.ShowEdit();

                return View();
            }
        }

        [ActionFilterAdmin]
        public virtual ActionResult Insert()
        {
            Model.Insert(Request);

            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.Show();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.Content = Model.Show();
                return View();
            }
        }

        [ActionFilterAdmin]
        public virtual ActionResult Update()
        {
            Model.Update(Request);

            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();

                Model.ID = null;
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.Show();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.Content = Model.Show();
                return View();
            }
        }

        [ActionFilterAdmin]
        public virtual ActionResult Hide()
        {
            var value = Request.QueryString["deleted"];

            if (value != null && (value == "0" || value == "1"))
                Model.Hide(value);


            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();

                Model.ID = null;
                output.ID = Model.ViewSetting.GUID;
                output.Body = Model.Show();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.Content = Model.Show();
                return View();
            }
        }

        public virtual ActionResult Data()
        {
            JsonOutput output = new JsonOutput();
            output.ID = Model.ViewSetting.GUID;
            output.Model = Model.QueryData();

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public virtual string LoginURL()
        {
            return "../account/login";
        }

        public virtual ActionResult RedirectToLogin()
        {
            return Redirect(LoginURL());
        }

        public virtual bool UpdateSession(string userid)
        {
            if (UserID == null)
            {
                ServerBase server = new ServerBase("user");
                server.SelectFilter("id", userid);

                DataTable data = server.SelectQuery();

                if (data.Rows.Count != 0)
                {
                    UpdateSession(data);
                    return true;
                }

                return false;
            }

            return true;
        }

        protected void UpdateSession(DataTable data)
        {
            string userid = data.Rows[0]["id"].ToString();
            string username = data.Rows[0]["username"].ToString();
            string secureid = data.Rows[0]["secureid"].ToString();
            string name = data.Rows[0]["name"].ToString();
            string usertype = data.Rows[0]["usertype_id"].ToString();

            Session["_name_"] = name;
            Session["_username_"] = username;
            Session["_userid_"] = userid;
            Session["_secureid_"] = secureid;
            Session["_usertype_"] = usertype;

            UpdateUserAccount(userid);
        }

        protected virtual void UpdateUserAccount(string userid)
        {
        }

        public virtual ActionResult Error(string message)
        {
            if (Model.ViewSetting.GUID != null)
            {
                JsonOutput output = new JsonOutput();
                output.ID = Model.ViewSetting.GUID;

                HTMLForm form = new HTMLForm(Model.ViewSetting);
                form.Title = "Error";
                form.Message(message, EnumMessageIcon.DANGER);
                form.Height = 200;

                output.Body = form.GenerateHTML();

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Initialize model
                Model.Initialize();

                HTMLForm form = new HTMLForm(Model.ViewSetting);
                form.Title = "Error";
                form.Message(message, EnumMessageIcon.DANGER);
                form.Height = 200;

                ViewBag.Body = form.GenerateHTML();
                return View();
            }
        }
    }
}