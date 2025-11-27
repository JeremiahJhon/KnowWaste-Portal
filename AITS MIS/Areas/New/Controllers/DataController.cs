using KnowWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kNowaste.Helper;

namespace Knowwaste.Areas.New.Controllers
{
    public class DataController : Controller
    {
        // GET: New/Data
        public ActionResult Index()
        {
            int countryID = Utility.GetCookieInt(Request, "CountryID");
            int cityID = Utility.GetCookieInt(Request, "CityID");
            int wasteTypeID = Utility.GetCookieInt(Request, "WasteTypeID");
            int year = Utility.GetCookieInt(Request, "Year");

            Data model = new Data(0,0,0,0);
            return View(model);
        }

    }
}