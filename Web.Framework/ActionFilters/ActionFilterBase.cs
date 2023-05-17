using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Models;

namespace Web.Framework.ActionFilters
{
    public class ActionFilterBase : ActionFilterAttribute
    {
        private string search { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseController controller = (BaseController)filterContext.Controller;

            Authenticate(controller, filterContext);
            controller.Initialize();

            if (controller.Model.ControllerName != "" && controller.Apps != null && !controller.Apps.ContainsKey(controller.Model.ControllerName.ToLower()))
                filterContext.Result = controller.Error("Access denied.");

            string id = controller.Request.QueryString["id"];
            string parenttable = controller.Request.QueryString["pn"];
            string parentid = controller.Request.QueryString["pid"];
            string page = controller.Request.QueryString["p"];
            string guid = controller.Request.QueryString["guid"];
            string search = controller.Request.Params["search"];

            if (page == null || page == "")
                page = "1";

            if (page.Contains(","))
                page = page.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1];

            ModelBase model = controller.Model;
            model.ID = id;

            model.ViewSetting.Page = int.Parse(page);
            model.ViewSetting.GUID = guid;

            if (controller.Model.ControllerName != "" && controller.Apps != null && !controller.Apps.ContainsKey(controller.Model.ControllerName.ToLower()))
                filterContext.Result = controller.Error("Access denied.");

            //Reset page if search for the first time
            if (search != null && model.ViewSetting.Search == null)
                model.ViewSetting.Page = 1;

            model.ViewSetting.Search = search;

            if (parenttable != null)
            {
                var parent = new ModelBase("", "");
                parent.Table = parenttable;
                parent.ID = parentid;
                model.ViewSetting.Parent = parent;
            }

            //For calendar
            if (controller.Model is ModelCalendar)
            {
                ModelCalendar calendar = controller.Model as ModelCalendar;

                string month = controller.Request.QueryString["m"];
                string day = controller.Request.QueryString["d"];
                string year = controller.Request.QueryString["y"];

                calendar.Month = month;
                calendar.Day = day;
                calendar.Year = year;
            }
        }
        protected virtual int Authenticate(BaseController controller, ActionExecutingContext filterContext)
        {
            //If user is not authenticated. Session-based.
            if (!controller.IsAuthenticated)
            {
                //Check cookies if exists.
                if (controller.Request.Cookies.Count != 0 && controller.Request.Cookies["userid"] != null)
                {
                    string userid = controller.Request.Cookies["userid"].Value;

                    //Update sessions
                    if (controller.UpdateSession(userid))
                        return 1;
                }
            }
            else
            {
                //User is already authenticated
                return 1;
            }


            return 0;
        }
    }
}