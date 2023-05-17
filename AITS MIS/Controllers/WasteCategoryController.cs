using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class WasteCategoryController : BaseController
    {
        public WasteCategoryController() : base(new ModelWasteCategory())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Waste Categories - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.WasteCategory = new ModelWasteCategory().GetData();            
            return View();
        }
       
    }
}