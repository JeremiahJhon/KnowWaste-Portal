using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;
using Web.Framework.Server;

namespace UCOnline.Controllers
{
    public class MarineDebrisController : BaseController
    {
        public MarineDebrisController() : base(new ModelHome())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Marine Debris";

            ServerBase videoposts = new ServerBase("videoposts");
            videoposts.SelectLimit(5);
            videoposts.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable videopostsData = videoposts.SelectQuery();

            ViewBag.ELearning = videopostsData;

            ServerBase documents = new ServerBase("documents");
            documents.SelectLimit(5);
            documents.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable documentsData = documents.SelectQuery();

            ViewBag.Documents = documentsData;

            ServerBase blogs = new ServerBase("blogs");
            blogs.SelectLimit(10);
            blogs.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable blogsData = blogs.SelectQuery();

            ViewBag.Blogs = blogsData;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.About = "";

            return View();
        }
        public ActionResult Template()
        {
            ViewBag.Title = "Template";

            return View();
        }
        public ActionResult Policies()
        {
            ViewBag.Title = "Policy and Regulations";

            ServerBase country = new ServerBase("Country");
            country.SelectFilter("SubRegion_ID = 3");
            DataTable countryData = country.SelectQuery();

            ViewBag.Country = countryData;

            ServerBase area = new ServerBase("countrypolicy_area");
            area.SelectFilter("Type_ID = 1");
            DataTable areaData = area.SelectQuery();

            ViewBag.Area = areaData;

            ViewBag.CountryPolicy = new ModelCountryPolicy().GetData();

            DataTable template = new DataTable();
            template.Columns.Add("FieldName", typeof(string));
            template.Columns.Add("FieldTitle", typeof(string));

            template.Rows.Add(new String[] { "code_number", "Code/Number" });
            template.Rows.Add(new String[] { "Title", "Title" });
            template.Rows.Add(new String[] { "Year", "Year" });
            template.Rows.Add(new String[] { "Description", "Description" });

            ViewBag.Template = template;

            return View();
        }

        public ActionResult ELearning()
        {
            ViewBag.Title = "E-Learning Courses";

            ServerBase videos = new ServerBase("videoposts");
            //videos.SelectFilter("SubRegion_ID = 3");
            DataTable videoData = videos.SelectQuery();

            ViewBag.Videos = videoData;

            return View();
        }

        public ActionResult Documents()
        {
            ViewBag.Title = "Documents";

            return View();
        }

        public ActionResult GoodPractices()
        {
            ViewBag.Title = "Good Practices";

            return View();
        }

        [HttpGet]
        public String getGraphData(int category,int filter)
        {
            ServerBase countryWS = new ServerBase("CountryWastestreams");
            countryWS.SelectOrder("Year", Web.Framework.Enums.EnumOrder.ASCENDING);
            countryWS.SelectFilter("WasteCategory_ID = " + category.ToString()); //1 = Solid Waste, 15 = Plastic Waste
            DataTable countryWSData = countryWS.SelectQuery();

            MSSQLServer db = new MSSQLServer();
            db.Query = "select [Year], Country_ID, TotalGenerated, TotalCollected, Recycled, Recovered, Disposal\r\nfrom countrywastestreams \r\nwhere country_id in (select id from country where subregion_id = 3) \r\nand [YEAR] < 2023 \r\nand [year] > 1\r\nand wastecategory_id = 1\r\norder by [year]";
            db.ExecuteQuery();

            return "";
        }
    }
}