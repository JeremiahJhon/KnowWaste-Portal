using KnowWaste.Models;
using System.Linq;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class NewsController : Controller
    {
        // GET: New/News
        public ActionResult Index()
        {
            News model = new News();
            return View(model);
        }

        // GET: New/News/Details/5
        public ActionResult Details(int id)
        {
            News model = new News(id);
            if (model.NewsList.Count == 1)
            {
                return View(model.NewsList.First());
            }
            else
            {
                if (Request.UrlReferrer != null)
                    return Redirect(Request.UrlReferrer.ToString());
                else
                    return RedirectToAction("Index");
            }
        }

        // GET: New/News/RefreshData/{countryID}
        public ActionResult RefreshData(string searchText, int pageIndex)
        {
            News model = new News(0, searchText, pageIndex);
            return PartialView("_Data", model);
        }
    }
}