using KnowWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class ThematicAreaController : Controller
    {
        // GET: New/ThematicArea
        public ActionResult Index()
        {
            Documents model = new Documents("");
            return View(model);
        }

        // GET: New/ThematicArea/RefreshData
        public ActionResult RefreshData(string area)
        {
            Documents model = new Documents(area);
            return PartialView("_Data", model);
        }
    }
}