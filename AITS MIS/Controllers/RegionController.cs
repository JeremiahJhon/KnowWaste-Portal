using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class RegionController : BaseController
    {
        public RegionController() : base(new ModelRegion())
        {
        }

        public override ActionResult Index()
        {            
            ViewBag.Title = "Region - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Region = new ModelRegion().GetData();
            
            return View();
        }
    }
}