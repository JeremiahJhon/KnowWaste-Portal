using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class SubRegionController : BaseController
    {
        public SubRegionController() : base(new ModelSubRegion())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Sub Region - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.SubRegion = new ModelSubRegion().GetData();
            return View();
        }
    }
}