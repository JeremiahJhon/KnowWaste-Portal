using KnowWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class TechnologyController : Controller
    {
        // GET: New/Technology
        public ActionResult Index()
        {
            Technology model = new Technology(0);
            return View(model);
        }

        // GET: New/Technology/Details/5
        public ActionResult Details(int id)
        {
            Technology model = new Technology(id);
            if (model.TechnologyList.Count == 1)
            {
                return View(model.TechnologyList.First());
            }
            else
            {
                if (Request.UrlReferrer != null)
                    return Redirect(Request.UrlReferrer.ToString());
                else
                    return RedirectToAction("Index");
            }
        }

        // GET: New/Technology/RefreshData/{countryID}
        public ActionResult RefreshData(int countryID)
        {
            Technology model = new Technology(0, countryID);
            return PartialView("_Data", model);
        }
    }
}