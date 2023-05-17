using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Controllers;

namespace Web.Framework.ActionFilters
{
    public class ActionFilterTable : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TableController controller = (TableController)filterContext.Controller;
            string table = controller.Request.QueryString["table"];

            controller.Model.Table = table;
        }
    }
}