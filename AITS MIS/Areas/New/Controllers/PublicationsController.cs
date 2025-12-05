using kNowaste.Helper;
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
            int countryID = Utility.GetCookieInt(Request, "CountryID");
            int year = Utility.GetCookieInt(Request, "Year");
            string publisher = Utility.GetCookieString(Request, "Publisher");

            Documents model = new Documents("Publications", countryID, year, publisher, "", 0);
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
        public ActionResult RefreshData(int countryID, int year, string publisher, string searchText, int pageIndex)
        {
            Documents model = new Documents("Publications", countryID, year, publisher, searchText, pageIndex);
            return PartialView("_Data", model);
        }
    }
}