using KnowWaste.Models;
using System.Linq;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class GoodPracticesController : Controller
    {
        // GET: New/GoodPractices
        public ActionResult Index()
        {
            GoodPractices model = new GoodPractices();
            return View(model);
        }

        // GET: New/GoodPractices/Details/5
        public ActionResult Details(int id)
        {
            GoodPractices model = new GoodPractices(id);
            if (model.BlogList.Count == 1)
            {
                return View(model.BlogList.First());
            }
            else
            {
                if (Request.UrlReferrer != null)
                    return Redirect(Request.UrlReferrer.ToString());
                else
                    return RedirectToAction("Index");
            }
        }

        // GET: New/GoodPractices/RefreshData/{countryID}
        public ActionResult RefreshData(int countryID)
        {
            GoodPractices model = new GoodPractices(0, countryID);
            return PartialView("_Data", model);
        }
    }
}
