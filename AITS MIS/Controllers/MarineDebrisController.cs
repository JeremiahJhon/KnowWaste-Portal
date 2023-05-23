using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
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

            ServerBase country = new ServerBase("Country");
            country.SelectFilter("SubRegion_ID = 3");
            DataTable countryData = country.SelectQuery();

            ViewBag.Country = countryData;

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

        public ActionResult Documents(int? id)
        {
            ViewBag.Title = "Documents";
            if(id == null)
            {
                ServerBase country = new ServerBase("Country");
                country.SelectFilter("SubRegion_ID = 3");
                DataTable countryData = country.SelectQuery();

                ViewBag.Country = countryData;

                ServerBase docType = new ServerBase("documentcategory");
                DataTable docTypeData = docType.SelectQuery();

                ViewBag.DocType = docTypeData;

                return View();
            }
            else
            {
                ServerBase country = new ServerBase("Country");
                country.SelectFilter("SubRegion_ID = 3");
                DataTable countryData = country.SelectQuery();

                ViewBag.Country = countryData;

                ServerBase docType = new ServerBase("documentcategory");
                DataTable docTypeData = docType.SelectQuery();

                ViewBag.DocType = docTypeData;

                return View("DocumentItem");
            }
        }
        
        public ActionResult GoodPractices()
        {
            ViewBag.Title = "Good Practices";

            ServerBase blogs = new ServerBase("blogs");
            blogs.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable blogsData = blogs.SelectQuery();

            ViewBag.Blogs = blogsData;

            return View();
        }

        [HttpPost]
        public String getSolidWasteGraphData(FormCollection formData)
        {
            String country_ = formData["country"];
            String type_ = formData["type"];

            if (type_ == null) type_ = "";

            string columns = "";
            if (type_.Contains("Generated"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Totalgenerated as decimal(10,2))) as Generated";
            }
            if (type_.Contains("Collected"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Totalcollected as decimal(10,2))) as Collected";
            }
            if (type_.Contains("Recycled"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Recycled as decimal(10,2))) as Recycled";
            }
            if (type_.Contains("Recovered"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Recovered as decimal(10,2))) as Recovered";
            }
            if (type_.Contains("Disposal"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Disposal as decimal(10,2))) as Disposal";
            }


            DataTable dtResult = new DataTable();

            if(columns.Length == 0) return JsonConvert.SerializeObject(dtResult);

            MSSQLServer db = new MSSQLServer();
            db.Query = "select [Year]," + columns + " from countrywastestreams  where country_id in (select id from country where subregion_id = 3) and [YEAR] < 2023 and [year] > 2015 and Country_ID in (" + country_ + ") and WasteCategory_ID = 1 group by [year]  order by [year]";
            dtResult = db.ExecuteQuery();

            return JsonConvert.SerializeObject(dtResult);
        }

        [HttpPost]
        public String getPlasticWasteGraphData(FormCollection formData)
        {
            String country_ = formData["country"];
            String type_ = formData["type"];

            if (type_ == null) type_ = "";

            string columns = "";
            if (type_.Contains("Generated"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Totalgenerated as decimal(10,2))) as Generated";
            }
            if (type_.Contains("Collected"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Totalcollected as decimal(10,2))) as Collected";
            }
            if (type_.Contains("Recycled"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Recycled as decimal(10,2))) as Recycled";
            }
            if (type_.Contains("Recovered"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Recovered as decimal(10,2))) as Recovered";
            }
            if (type_.Contains("Disposal"))
            {
                if (columns.Length > 0) columns += ",";
                columns += "SUM(cast(Disposal as decimal(10,2))) as Disposal";
            }


            DataTable dtResult = new DataTable();

            if (columns.Length == 0) return JsonConvert.SerializeObject(dtResult);

            MSSQLServer db = new MSSQLServer();
            db.Query = "select [Year]," + columns + " from countrywastestreams  where country_id in (select id from country where subregion_id = 3) and [YEAR] < 2023 and [year] > 2015 and Country_ID in (" + country_ + ") and WasteCategory_ID = 14 group by [year]  order by [year]";
            dtResult = db.ExecuteQuery();

            return JsonConvert.SerializeObject(dtResult);
        }

        [HttpPost]
        public String getDocumentData(FormCollection formData)
        {
            String country = formData["country"];
            String doctype = formData["doctype"];

            ServerBase doctypes = new ServerBase("documentcategory");
            doctypes.SelectFilter("ID in (" + doctype + ")");
            DataTable doctypeData = doctypes.SelectQuery();

            DataSet result = new DataSet();

            foreach(DataRow row in doctypeData.Rows)
            {
                ServerBase document = new ServerBase("documents");
                document.SelectFilter("Country_ID in (" + country + ") and DocumentCategory_ID = " + row["ID"].ToString() + " and Attachment is not null and Attachment <> ''");
                result.Tables.Add(document.SelectQuery());
                result.Tables[result.Tables.Count - 1].TableName = row["Name"].ToString();
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}