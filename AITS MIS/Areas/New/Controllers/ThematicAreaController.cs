using kNowaste.Helper;
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
            string searchText = Utility.GetCookieString(Request, "SearchText");
            Documents model = new Documents("", searchText, 0);
            return View(model);
        }

        // GET: New/ThematicArea/RefreshData
        public ActionResult RefreshData(string area, string searchText, int pageIndex)
        {
            Documents model = new Documents(area, searchText, pageIndex);
            return PartialView("_Data", model);
        }
    }
}