using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class ForumsDeletedController : BaseController
    {
        public ForumsDeletedController() : base(new ModelForums())
        {
        }

        public override ActionResult Index()
        {
            var geothemeid = Request.QueryString["gid"];
            ((ModelForums)Model).geothemeid = geothemeid;

            ViewBag.Title = "Forums - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Thematicmenu = ((ModelForums)Model).ShowMenu();
            ViewBag.Forums = ((ModelForums)Model).Show();
            return View();
        }
    }
}