using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Framework.Controllers;

namespace Web.Framework.ActionFilters
{
    public class ActionFilterAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseController controller = (BaseController)filterContext.Controller;
            int auth = Authenticate(controller, filterContext);

            string action = controller.RouteData.Values["action"].ToString();
            string controllername = controller.RouteData.Values["controller"].ToString();

            if (controllername.ToLower() == "account" && action.ToLower() == "insert")
                return;

            if (auth == 1)
            {

            }
            else
                RedirectToError(filterContext);
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

        /// <summary>
        /// Extract query strings besides the default query strings
        /// </summary>
        /// <param name="controller"></param>
        protected virtual void OnActionExecuting(BaseController controller)
        {
            //For inheriting action filters
        }

        /// <summary>
        /// Redirect to this route if user is not authenticated
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void RedirectToError(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "Login",
                controller = "Account",
                area = ""
            }));
        }

        /// <summary>
        /// Redirect to this route if user is not authenticated
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void RedirectToNoPermission(ActionExecutingContext filterContext, string control, string guid)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = string.Format("Error?guid={0}", guid),
                controller = control,
                area = ""
            }));
        }
    }
}