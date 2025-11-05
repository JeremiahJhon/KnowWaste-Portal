using KnowWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class PublicationsController : Controller
    {
        // GET: New/Publications
        public ActionResult Index()
        {
            Documents model = new Documents("Publications", 0);
            return View(model);
        }

        // GET: New/Publications/Details/5
        public ActionResult Details(int id)
        {
            Documents model = new Documents("Publications", id);
            if (model.DocumentList.Count == 1)
            {
                return View(model.DocumentList.First());
            }
            else
            {
                if (Request.UrlReferrer != null)
                    return Redirect(Request.UrlReferrer.ToString());
                else
                    return RedirectToAction("Index");
            }
        }

        // GET: New/Publications/RefreshData
        public ActionResult RefreshData(int countryID, int year, string publisher)
        {
            Documents model = new Documents("Publications", countryID, year, publisher);
            return PartialView("_Data", model);
        }
    }
}