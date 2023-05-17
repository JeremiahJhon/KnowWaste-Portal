using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class SectorController : BaseController
    {
        public SectorController() : base(new ModelSector())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Sectors - kNOw.Waste Management Platform";

            ViewBag.Sector = new ModelSector().GetData();            
            return View();
        }
    }
}