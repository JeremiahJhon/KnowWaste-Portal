using KnowWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class DataController : Controller
    {
        // GET: New/Data
        public ActionResult Index()
        {
            int countryID = GetCookieInt(Request, "CountryID");
            int cityID = GetCookieInt(Request, "CityID");
            int wasteTypeID = GetCookieInt(Request, "WasteTypeID");
            int year = GetCookieInt(Request, "Year");

            Data model = new Data(0,0,0,0);
            return View(model);
        }

        private int GetCookieInt(HttpRequestBase request, string cookieName)
        {
            var cookie = request.Cookies[cookieName];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
                return 0;

            int value;
            return int.TryParse(cookie.Value, out value) ? value : 0;
        }
    }
}