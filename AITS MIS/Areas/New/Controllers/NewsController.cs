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
            model.GetData(null);
            return View(model);
        }

        // GET: New/News/Details/5
        public ActionResult Details(int id)
        {
            News model = new News();
            model.GetData(id);
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
    }
}