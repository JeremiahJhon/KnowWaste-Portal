using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.PerformanceData;
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
            documents.SelectFilter("Country_id in (select id from country where SubRegion_ID = 3) and Attachment is not null and Attachment <> '' and Geotheme_ID = 6");
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

        public override ActionResult Data()
        {
            ViewBag.Title = "Data Map";

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

            ViewBag.CountryPolicy = new ModelCountryPolicy().GetDataNew();

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

            ServerBase videoposts = new ServerBase("videoposts");
            videoposts.SelectLimit(5);
            videoposts.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable videopostsData = videoposts.SelectQuery();

            ViewBag.ELearning = videopostsData;

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
                ServerBase doc = new ServerBase("Documents");
                doc.SelectFilter("ID = " + id.ToString());
                DataTable docData = doc.SelectQuery();

                if(docData.Rows.Count == 1)
                {

                    ViewBag.Document = docData.Rows[0];

                    return View("DocumentItem");
                }
                else
                {
                    return View("Empty","Pages");
                }
            }
        }
        
        public ActionResult GoodPractices(int? id)
        {
            ViewBag.Title = "Good Practices";
            
            if (id == null)
            {
                ServerBase blogs = new ServerBase("blogs");
                blogs.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable blogsData = blogs.SelectQuery();

                ViewBag.Blogs = blogsData;

                return View();
            }
            else
            {
                ServerBase blogs = new ServerBase("blogs");
                blogs.SelectFilter("ID = " + id.ToString());
                DataTable blogsData = blogs.SelectQuery();

                if (blogsData.Rows.Count == 1)
                {

                    ViewBag.Blog = blogsData.Rows[0];

                    return View("BlogItem");
                }
                else
                {
                    return View("Empty", "Pages");
                }
            }
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
                document.SelectFilter("Country_ID in (" + country + ") and DocumentCategory_ID = " + row["ID"].ToString() + " and Attachment is not null and Attachment <> '' and Geotheme_ID = 6");
                result.Tables.Add(document.SelectQuery());
                result.Tables[result.Tables.Count - 1].TableName = row["Name"].ToString();
            }

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public String getPolicyData(int country, int area)
        {
            string filter = "";
            if (country > 0)
            {
                filter += " and Country_Id = " + country;
            }
            if (area > 0)
            {
                if (filter != "") filter += " and ";

                filter += "Area_Id = " + area;
            }
            DataSet data = new DataSet();
            ServerBase countryPolicy = new ServerBase("CountryPolicy");
            countryPolicy.SelectFilter("(Area_ID > 0 and Area_ID is not null) and (Country_ID > 0 and Country_ID is not null)" + filter);
            countryPolicy.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable countryPolicyData = countryPolicy.SelectQuery();
            ServerBase country_ = new ServerBase("Country");
            country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> '' and SubRegion_ID in (select ID from SubRegion where Region_id = 3)"); // 3 = Asia
            DataTable countryData = country_.SelectQuery();
            ServerBase countryPolicyType = new ServerBase("CountryPolicy_Type");
            DataTable countryPolicyTypeData = countryPolicyType.SelectQuery();
            ServerBase countryPolicyArea = new ServerBase("CountryPolicy_Area");
            DataTable countryPolicyAreaData = countryPolicyArea.SelectQuery();
            ServerBase wasteCategory = new ServerBase("WasteCategory");
            DataTable wasteCategoryData = wasteCategory.SelectQuery();

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("ID", typeof(int));
            dtResult.Columns.Add("code_number", typeof(string));
            dtResult.Columns.Add("Title", typeof(string));
            dtResult.Columns.Add("Year", typeof(string));
            dtResult.Columns.Add("Description", typeof(string));
            dtResult.Columns.Add("Country_ID", typeof(string));
            dtResult.Columns.Add("Country", typeof(string));
            dtResult.Columns.Add("WasteCategory", typeof(string));
            dtResult.Columns.Add("PolicyType", typeof(string));
            dtResult.Columns.Add("Area_ID", typeof(int));
            dtResult.Columns.Add("Area", typeof(string));
            dtResult.Columns.Add("Link", typeof(string));

            if (countryPolicyData.Rows.Count == 0)
            {
                return JsonConvert.SerializeObject(dtResult);
            }

            var JoinResult = from a in countryPolicyData.AsEnumerable()
                             join b in countryData.AsEnumerable()
                             on a.Field<String>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                             join c in countryPolicyTypeData.AsEnumerable()
                             on a.Field<int?>("Type_ID").ToString() equals c.Field<int?>("ID").ToString() into bc
                             from ac in bc.DefaultIfEmpty()
                             join d in countryPolicyAreaData.AsEnumerable()
                             on a.Field<int?>("Area_ID").ToString() equals d.Field<int?>("ID").ToString() into bd
                             from ad in bd.DefaultIfEmpty()
                             join e in wasteCategoryData.AsEnumerable()
                             on a.Field<int?>("WasteCategory_ID").ToString() equals e.Field<int?>("ID").ToString() into be
                             from ae in be.DefaultIfEmpty()
                             select dtResult.LoadDataRow(new object[]
                             {
                                a.Field<int>("ID"),
                                a.Field<string>("code_number"),
                                a.Field<string>("Legal"),
                                a.Field<string>("Year"),
                                a.Field<string>("Description"),
                                a.Field<string>("Country_ID"),
                                b.Field<string>("Name"),
                                ae == null ? "" : ae.Field<string>("Name"),
                                ac == null ? "" : ac.Field<string>("Name"),
                                a.Field<int>("Area_ID"),
                                ad == null ? "" : ad.Field<string>("Name"),
                                a.Field<string>("Link"),
                             }, false);
            JoinResult.CopyToDataTable();

            return JsonConvert.SerializeObject(dtResult);
        }

    }
}