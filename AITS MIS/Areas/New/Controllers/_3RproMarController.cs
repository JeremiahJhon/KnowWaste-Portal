using KnowWaste.Models;
using System.Linq;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class _3RproMarController : Controller
    {
        // GET: New/_3RproMar
        public ActionResult Index()
        {
            Documents model = new Documents("3Rpromar",0);
            return View(model);
        }

        // GET: New/_3RproMar/Details/5
        public ActionResult Details(int id)
        {
            Documents model = new Documents("3Rpromar", id);
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

        // GET: New/_3RproMar/RefreshData
        public ActionResult RefreshData(int countryID, int year, string publisher)
        {
            Documents model = new Documents("3Rpromar", countryID, year, publisher);
            return PartialView("_Data", model);
        }
    }
}