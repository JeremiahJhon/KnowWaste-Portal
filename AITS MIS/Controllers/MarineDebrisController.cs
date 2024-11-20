using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using UCOnline.Models;
using Web.Framework.Controllers;
using Web.Framework.Server;
using UCOnline.Data;
using kNowaste.Helper;

namespace UCOnline.Controllers
{
    public class MarineDebrisController : BaseController
    {
        KnowWasteEntities context = new KnowWasteEntities();
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

            ServerBase blogs = new ServerBase("blogs");
            blogs.SelectLimit(10);
            blogs.SelectFilter("Blogscategory_ID = 1");
            blogs.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable blogsData = blogs.SelectQuery();

            ServerBase country_ = new ServerBase("Country");
            country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
            DataTable countryData = country_.SelectQuery();

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("ID", typeof(int));
            dtResult.Columns.Add("Title", typeof(string));
            dtResult.Columns.Add("Description", typeof(string));
            dtResult.Columns.Add("Detail", typeof(string));
            dtResult.Columns.Add("Author", typeof(string));
            dtResult.Columns.Add("Thumbnail", typeof(string));
            dtResult.Columns.Add("Country", typeof(string));
            dtResult.Columns.Add("ResultsAchieved", typeof(string));
            dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
            dtResult.Columns.Add("Replicability", typeof(string));
            dtResult.Columns.Add("Sources", typeof(string));
            dtResult.Columns.Add("Company", typeof(string));
            dtResult.Columns.Add("Email", typeof(string));
            dtResult.Columns.Add("Phone", typeof(string));
            dtResult.Columns.Add("Initiative", typeof(string));
            dtResult.Columns.Add("Photo", typeof(string));

            var JoinResult = from a in blogsData.AsEnumerable()
                             join b in countryData.AsEnumerable()
                             on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                             select dtResult.LoadDataRow(new object[]
                             {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                             }, false);
            JoinResult.CopyToDataTable();

            ViewBag.Blogs = dtResult;

            ServerBase documents = new ServerBase("documents");
            documents.SelectFilter("Documentcategory_ID = 4");
            documents.SelectLimit(10);
            documents.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable documentsData = documents.SelectQuery();

            dtResult = new DataTable();
            dtResult.Columns.Add("ID", typeof(int));
            dtResult.Columns.Add("Title", typeof(string));
            dtResult.Columns.Add("Country", typeof(string));
            dtResult.Columns.Add("Attachment", typeof(string));
            dtResult.Columns.Add("Description", typeof(string));
            dtResult.Columns.Add("Datasource", typeof(string));
            dtResult.Columns.Add("Thumbnail", typeof(string));
            dtResult.Columns.Add("Keyword", typeof(string));
            dtResult.Columns.Add("Year", typeof(string));
            dtResult.Columns.Add("Publisher", typeof(string));
            dtResult.Columns.Add("Subtitle", typeof(string));

            var JoinResult2 = from a in documentsData.AsEnumerable()
                             join b in countryData.AsEnumerable()
                             on a.Field<String>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                             select dtResult.LoadDataRow(new object[]
                                {
                                        a.Field<int>("ID"),
                                        a.Field<string>("Title"),
                                        b.Field<string>("Name"),
                                        a.Field<string>("Attachment"),
                                        a.Field<string>("Description"),
                                        a.Field<string>("Datasource"),
                                        a.Field<string>("Thumbnail"),
                                        a.Field<string>("Keyword"),
                                        a.Field<string>("Year"),
                                        a.Field<string>("Publisher"),
                                        a.Field<string>("Subtitle"),
                                }, false);
            JoinResult2.CopyToDataTable();

            ViewBag.Documents = dtResult;


            ServerBase country = new ServerBase("Country");
            country.SelectFilter("SubRegion_ID = 3");
            countryData = country.SelectQuery();

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
            template.Rows.Add(new String[] { "Country", "Country" });
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

            var microlearning = context.microlearnings.Where(x => x.Deleted == 0).ToList();

            ViewBag.MicroLearning = microlearning;

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

                if (docData.Rows.Count == 1)
                {
                    ServerBase country = new ServerBase("Country");
                    country.SelectFilter("ID in (" + docData.Rows[0]["Country_ID"].ToString() + ")");
                    DataTable countryData = country.SelectQuery();
                    ServerBase category = new ServerBase("DocumentCategory");
                    category.SelectFilter("ID = " + docData.Rows[0]["DocumentCategory_ID"].ToString());
                    DataTable categoryData = category.SelectQuery();
                    ServerBase geotheme = new ServerBase("Geotheme");
                    geotheme.SelectFilter("ID = " + docData.Rows[0]["Geotheme_ID"].ToString());
                    DataTable geothemeData = geotheme.SelectQuery();

                    ViewBag.Document = docData.Rows[0];

                    ViewData["Country"] = CountryNames(docData.Rows[0]["Country_ID"].ToString());
                    ViewData["Category"] = categoryData.Rows[0]["Name"].ToString();
                    ViewData["Geotheme"] = geothemeData.Rows[0]["Name"].ToString();

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
            ViewBag.Title = "GOOD PRACTICES";
            ViewBag.MediaPath = "Blogs";

            if (id == null)
            {
                ServerBase blogs = new ServerBase("blogs");
                blogs.SelectFilter("Blogscategory_ID = 1");
                blogs.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable blogsData = blogs.SelectQuery();

                if (blogsData.Rows.Count == 0)
                {
                    return RedirectToAction("Index", "Pages");
                }

                ServerBase country_ = new ServerBase("Country");
                country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                DataTable countryData = country_.SelectQuery();

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("ID", typeof(int));
                dtResult.Columns.Add("Title", typeof(string));
                dtResult.Columns.Add("Description", typeof(string));
                dtResult.Columns.Add("Detail", typeof(string));
                dtResult.Columns.Add("Author", typeof(string));
                dtResult.Columns.Add("Thumbnail", typeof(string));
                dtResult.Columns.Add("Country", typeof(string));
                dtResult.Columns.Add("ResultsAchieved", typeof(string));
                dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                dtResult.Columns.Add("Replicability", typeof(string));
                dtResult.Columns.Add("Sources", typeof(string));
                dtResult.Columns.Add("Company", typeof(string));
                dtResult.Columns.Add("Email", typeof(string));
                dtResult.Columns.Add("Phone", typeof(string));
                dtResult.Columns.Add("Initiative", typeof(string));
                dtResult.Columns.Add("Photo", typeof(string));
                dtResult.Columns.Add("BlogDate", typeof(string));

                var JoinResult = from a in blogsData.AsEnumerable()
                                 join b in countryData.AsEnumerable()
                                 on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                 select dtResult.LoadDataRow(new object[]
                                 {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<DateTime>("Blogsdate").ToString(),
                                 }, false);
                JoinResult.CopyToDataTable();

                ViewBag.Data = dtResult;

                return View();
            }
            else
            {
                ServerBase blogs = new ServerBase("blogs");
                blogs.SelectFilter("ID = " + id.ToString());
                DataTable blogsData = blogs.SelectQuery();

                if (blogsData.Rows.Count == 1)
                {
                    ServerBase country_ = new ServerBase("Country");
                    country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                    DataTable countryData = country_.SelectQuery();

                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("ID", typeof(int));
                    dtResult.Columns.Add("Title", typeof(string));
                    dtResult.Columns.Add("Description", typeof(string));
                    dtResult.Columns.Add("Detail", typeof(string));
                    dtResult.Columns.Add("Author", typeof(string));
                    dtResult.Columns.Add("Thumbnail", typeof(string));
                    dtResult.Columns.Add("Country", typeof(string));
                    dtResult.Columns.Add("ResultsAchieved", typeof(string));
                    dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                    dtResult.Columns.Add("Replicability", typeof(string));
                    dtResult.Columns.Add("Sources", typeof(string));
                    dtResult.Columns.Add("Company", typeof(string));
                    dtResult.Columns.Add("Email", typeof(string));
                    dtResult.Columns.Add("Phone", typeof(string));
                    dtResult.Columns.Add("Initiative", typeof(string));
                    dtResult.Columns.Add("Photo", typeof(string));
                    dtResult.Columns.Add("BlogDate", typeof(string));

                    var JoinResult = from a in blogsData.AsEnumerable()
                                     join b in countryData.AsEnumerable()
                                     on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                     select dtResult.LoadDataRow(new object[]
                                     {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<DateTime>("Blogsdate").ToString(),
                                     }, false);
                    JoinResult.CopyToDataTable();

                    ViewBag.Data = dtResult.Rows[0];

                    return View("BlogItem");
                }
                else
                {
                    return View("Empty", "Pages");
                }
            }
        }

        public ActionResult MicroLearning(int id)
        {
            ViewBag.Title = "MICRO LEARNING";
            ViewBag.MediaPath = "MicroLearning";

            var microlearning = context.microlearnings.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.Data = microlearning;

            return View("MicroLearning");
        }

        public ActionResult News(int? id)
        {
            ViewBag.Title = "NEWS";
            ViewBag.MediaPath = "News";

            if (id == null)
            {
                ServerBase news = new ServerBase("news");
                news.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable newsData = news.SelectQuery();

                if (newsData.Rows.Count == 0)
                {
                    return RedirectToAction("Index", "Pages");
                }

                ServerBase country_ = new ServerBase("Country");
                country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                DataTable countryData = country_.SelectQuery();

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("ID", typeof(int));
                dtResult.Columns.Add("Title", typeof(string));
                dtResult.Columns.Add("Description", typeof(string));
                dtResult.Columns.Add("Detail", typeof(string));
                dtResult.Columns.Add("Author", typeof(string));
                dtResult.Columns.Add("Thumbnail", typeof(string));
                dtResult.Columns.Add("Country", typeof(string));
                dtResult.Columns.Add("ResultsAchieved", typeof(string));
                dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                dtResult.Columns.Add("Replicability", typeof(string));
                dtResult.Columns.Add("Sources", typeof(string));
                dtResult.Columns.Add("Company", typeof(string));
                dtResult.Columns.Add("Email", typeof(string));
                dtResult.Columns.Add("Phone", typeof(string));
                dtResult.Columns.Add("Initiative", typeof(string));
                dtResult.Columns.Add("Photo", typeof(string));
                dtResult.Columns.Add("BlogDate", typeof(string));
                dtResult.Columns.Add("Location", typeof(string));

                var JoinResult = from a in newsData.AsEnumerable()
                                 join b in countryData.AsEnumerable()
                                 on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                 select dtResult.LoadDataRow(new object[]
                                 {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<string>("StartDate"),
                                    a.Field<string>("Location"),
                                 }, false);
                JoinResult.CopyToDataTable();

                ViewBag.Data = dtResult;

                return View();
            }
            else
            {
                ServerBase news = new ServerBase("news");
                news.SelectFilter("ID = " + id.ToString());
                DataTable newsData = news.SelectQuery();

                if (newsData.Rows.Count == 1)
                {
                    ServerBase country_ = new ServerBase("Country");
                    country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                    DataTable countryData = country_.SelectQuery();

                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("ID", typeof(int));
                    dtResult.Columns.Add("Title", typeof(string));
                    dtResult.Columns.Add("Description", typeof(string));
                    dtResult.Columns.Add("Detail", typeof(string));
                    dtResult.Columns.Add("Author", typeof(string));
                    dtResult.Columns.Add("Thumbnail", typeof(string));
                    dtResult.Columns.Add("Country", typeof(string));
                    dtResult.Columns.Add("ResultsAchieved", typeof(string));
                    dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                    dtResult.Columns.Add("Replicability", typeof(string));
                    dtResult.Columns.Add("Sources", typeof(string));
                    dtResult.Columns.Add("Company", typeof(string));
                    dtResult.Columns.Add("Email", typeof(string));
                    dtResult.Columns.Add("Phone", typeof(string));
                    dtResult.Columns.Add("Initiative", typeof(string));
                    dtResult.Columns.Add("Photo", typeof(string));
                    dtResult.Columns.Add("BlogDate", typeof(string));
                    dtResult.Columns.Add("Location", typeof(string));

                    var JoinResult = from a in newsData.AsEnumerable()
                                     join b in countryData.AsEnumerable()
                                     on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                     select dtResult.LoadDataRow(new object[]
                                     {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<string>("StartDate"),
                                    a.Field<string>("Location"),
                                     }, false);
                    JoinResult.CopyToDataTable();

                    ViewBag.Data = dtResult.Rows[0];

                    return View("NewsItem");
                }
                else
                {
                    return View("Empty", "Pages");
                }
            }
        }

        public ActionResult Technology(int? id)
        {
            ViewBag.Title = "TECHNOLOGY";
            ViewBag.MediaPath = "Technology";

            if (id == null)
            {
                ServerBase blogs = new ServerBase("blogs");
                blogs.SelectFilter("Blogscategory_ID = 3");
                blogs.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable blogsData = blogs.SelectQuery();

                if (blogsData.Rows.Count == 0)
                {
                    return RedirectToAction("Index", "Pages");
                }

                ServerBase country_ = new ServerBase("Country");
                country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                DataTable countryData = country_.SelectQuery();

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("ID", typeof(int));
                dtResult.Columns.Add("Title", typeof(string));
                dtResult.Columns.Add("Description", typeof(string));
                dtResult.Columns.Add("Detail", typeof(string));
                dtResult.Columns.Add("Author", typeof(string));
                dtResult.Columns.Add("Thumbnail", typeof(string));
                dtResult.Columns.Add("Country", typeof(string));
                dtResult.Columns.Add("ResultsAchieved", typeof(string));
                dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                dtResult.Columns.Add("Replicability", typeof(string));
                dtResult.Columns.Add("Sources", typeof(string));
                dtResult.Columns.Add("Company", typeof(string));
                dtResult.Columns.Add("Email", typeof(string));
                dtResult.Columns.Add("Phone", typeof(string));
                dtResult.Columns.Add("Initiative", typeof(string));
                dtResult.Columns.Add("Photo", typeof(string));
                dtResult.Columns.Add("BlogDate", typeof(string));

                var JoinResult = from a in blogsData.AsEnumerable()
                                 join b in countryData.AsEnumerable()
                                 on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                 select dtResult.LoadDataRow(new object[]
                                 {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<DateTime>("Blogsdate").ToString(),
                                 }, false);
                JoinResult.CopyToDataTable();

                ViewBag.Data = dtResult;

                return View();
            }
            else
            {
                ServerBase blogs = new ServerBase("blogs");
                blogs.SelectFilter("ID = " + id.ToString());
                DataTable blogsData = blogs.SelectQuery();

                if (blogsData.Rows.Count == 1)
                {
                    ServerBase country_ = new ServerBase("Country");
                    country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                    DataTable countryData = country_.SelectQuery();

                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("ID", typeof(int));
                    dtResult.Columns.Add("Title", typeof(string));
                    dtResult.Columns.Add("Description", typeof(string));
                    dtResult.Columns.Add("Detail", typeof(string));
                    dtResult.Columns.Add("Author", typeof(string));
                    dtResult.Columns.Add("Thumbnail", typeof(string));
                    dtResult.Columns.Add("Country", typeof(string));
                    dtResult.Columns.Add("ResultsAchieved", typeof(string));
                    dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                    dtResult.Columns.Add("Replicability", typeof(string));
                    dtResult.Columns.Add("Sources", typeof(string));
                    dtResult.Columns.Add("Company", typeof(string));
                    dtResult.Columns.Add("Email", typeof(string));
                    dtResult.Columns.Add("Phone", typeof(string));
                    dtResult.Columns.Add("Initiative", typeof(string));
                    dtResult.Columns.Add("Photo", typeof(string));
                    dtResult.Columns.Add("BlogDate", typeof(string));

                    var JoinResult = from a in blogsData.AsEnumerable()
                                     join b in countryData.AsEnumerable()
                                     on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                     select dtResult.LoadDataRow(new object[]
                                     {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<DateTime>("Blogsdate").ToString(),
                                     }, false);
                    JoinResult.CopyToDataTable();

                    ViewBag.Data = dtResult.Rows[0];

                    return View("TechItem");
                }
                else
                {
                    return View("Empty", "Pages");
                }
            }
        }

        public ActionResult Publications(int? id)
        {
            ViewBag.Title = "Publications";

            //if (id == null)
            //{
            ServerBase _3RproMar = new ServerBase("documents");
            _3RproMar.SelectFilter("(Publisher like '%RRC.AP%' or Publisher like '%ERIA%' or Publisher like '%NIVA%' or Publisher like '%GIZ%' or Documentcategory_ID = 4) and CAST(Year as int) >= 2010");
            _3RproMar.SelectOrder("Year", Web.Framework.Enums.EnumOrder.DESCENDING);
            _3RproMar.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable _3RproMarData = _3RproMar.SelectQuery();

            ServerBase country_ = new ServerBase("Country");
            country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
            DataTable countryData = country_.SelectQuery();

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("ID", typeof(int));
            dtResult.Columns.Add("Title", typeof(string));
            dtResult.Columns.Add("Location", typeof(string));
            dtResult.Columns.Add("Country", typeof(string));
            dtResult.Columns.Add("Countries", typeof(string));
            dtResult.Columns.Add("Attachment", typeof(string));
            dtResult.Columns.Add("Description", typeof(string));
            dtResult.Columns.Add("Datasource", typeof(string));
            dtResult.Columns.Add("Thumbnail", typeof(string));
            dtResult.Columns.Add("Keyword", typeof(string));
            dtResult.Columns.Add("Year", typeof(string));
            dtResult.Columns.Add("Publisher", typeof(string));
            dtResult.Columns.Add("Subtitle", typeof(string));

            string Countries;
            var JoinResult = from a in _3RproMarData.AsEnumerable()
                                join b in countryData.AsEnumerable()
                                on a.Field<String>("Country_ID").ToString().Split(',')[0] equals b.Field<int>("ID").ToString()
                                select dtResult.LoadDataRow(new object[]
                                {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Location"),
                                    b.Field<string>("Name"),
                                    Countries = CountryNames(a.Field<String>("Country_ID")),
                                    a.Field<string>("Attachment"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Datasource"),
                                    a.Field<string>("Thumbnail"),
                                    a.Field<string>("Keyword"),
                                    a.Field<string>("Year"),
                                    a.Field<string>("Publisher"),
                                    a.Field<string>("Subtitle"),
                                }, false);
            JoinResult.CopyToDataTable();
            ViewBag.Data = dtResult;

            var year = (from r in _3RproMarData.AsEnumerable() select r.Field<string>("Year")).Distinct().ToList();

            ViewBag.Year = year;

            return View();
            //}
            //else
            //{
            //    ServerBase blogs = new ServerBase("blogs");
            //    blogs.SelectFilter("ID = " + id.ToString());
            //    DataTable blogsData = blogs.SelectQuery();

            //    if (blogsData.Rows.Count == 1)
            //    {

            //        ViewBag.Blog = blogsData.Rows[0];

            //        return View("BlogItem");
            //    }
            //    else
            //    {
            //        return View("Empty", "Pages");
            //    }
            //}
        }

        private string CountryNames(string countryIDs)
        {
            string result = string.Empty;

            int[] countryArray = countryIDs.Split(',').Select(int.Parse).ToArray();

            result = string.Join(", ", context.countries.Where(x => countryArray.Contains(x.ID)).Select(x => x.Name));

            return result;
        }

        public ActionResult _3RproMar(int? id)
        {
            ViewBag.Title = "3RproMar";

            if (id == null)
            {
                /*ServerBase _3RproMar = new ServerBase("documents");
                _3RproMar.SelectFilter("documentcategory_id = 4"); //3RproMar
                _3RproMar.SelectOrder("Year", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable _3RproMarData = _3RproMar.SelectQuery();*/

                var _3RproMarData = context.documents.Where(x => x.Documentcategory_ID == 4 && x.Deleted == 0).OrderByDescending(x => x.Year);

                /*ServerBase country_ = new ServerBase("Country");
                country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                DataTable countryData = country_.SelectQuery();*/

                var countryData = context.countries.Where(x => x.SubRegion_ID != null && x.SubRegion_ID != "");

                var JoinResult = from a in _3RproMarData.AsEnumerable()
                                 join b in countryData.AsEnumerable()
                                 on a.Country_ID equals b.ID.ToString()
                                 select new
                                 {
                                    a.ID,
                                    a.Title,
                                    a.Location,
                                    Country = b.Name,
                                    a.Attachment,
                                    a.Description,
                                    a.Datasource,
                                    a.Thumbnail,
                                    a.Keyword,
                                    a.Year,
                                    a.Publisher,
                                    a.Subtitle,
                                };

                ViewBag._3RproMar = Utility.LinqToDataTable(JoinResult); ;

                var year = _3RproMarData.Select(x => x.Year).Distinct().OrderByDescending(y => y).ToList();

                ViewBag.Year = year;

                return View();
            }
            else
            {
                //ServerBase blogs = new ServerBase("documents");
                //blogs.SelectFilter("ID = " + id.ToString());
                //DataTable blogsData = blogs.SelectQuery();

                //if (blogsData.Rows.Count == 1)
                //{

                //    ViewBag.Blog = blogsData.Rows[0];

                //    return View("BlogItem");
                //}
                //else
                //{
                //    return View("Empty", "Pages");
                //}
                ServerBase _3RproMar = new ServerBase("documents");
                _3RproMar.SelectFilter("documentcategory_id = 4"); //3RproMar
                _3RproMar.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable _3RproMarData = _3RproMar.SelectQuery();

                ViewBag._3RproMar = _3RproMarData;

                return View();
            }
        }

        public ActionResult UpcomingEvents(int? id)
        {
            ViewBag.Title = "UPCOMING EVENTS";
            ViewBag.MediaPath = "UpcomingEvents";

            if (id == null)
            {
                ServerBase upcomingevents = new ServerBase("upcomingevents");
                upcomingevents.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
                DataTable upcomingeventsData = upcomingevents.SelectQuery();

                if (upcomingeventsData.Rows.Count == 0)
                {
                    return RedirectToAction("Index", "Pages");
                }

                ServerBase country_ = new ServerBase("Country");
                country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                DataTable countryData = country_.SelectQuery();

                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("ID", typeof(int));
                dtResult.Columns.Add("Title", typeof(string));
                dtResult.Columns.Add("Description", typeof(string));
                dtResult.Columns.Add("Detail", typeof(string));
                dtResult.Columns.Add("Author", typeof(string));
                dtResult.Columns.Add("Thumbnail", typeof(string));
                dtResult.Columns.Add("Country", typeof(string));
                dtResult.Columns.Add("ResultsAchieved", typeof(string));
                dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                dtResult.Columns.Add("Replicability", typeof(string));
                dtResult.Columns.Add("Sources", typeof(string));
                dtResult.Columns.Add("Company", typeof(string));
                dtResult.Columns.Add("Email", typeof(string));
                dtResult.Columns.Add("Phone", typeof(string));
                dtResult.Columns.Add("Initiative", typeof(string));
                dtResult.Columns.Add("Photo", typeof(string));
                dtResult.Columns.Add("BlogDate", typeof(string));
                dtResult.Columns.Add("Location", typeof(string));

                var JoinResult = from a in upcomingeventsData.AsEnumerable()
                                 join b in countryData.AsEnumerable()
                                 on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                 select dtResult.LoadDataRow(new object[]
                                 {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<string>("StartDate"),
                                    a.Field<string>("Location"),
                                 }, false);
                JoinResult.CopyToDataTable();

                ViewBag.Data = dtResult;

                return View();
            }
            else
            {
                ServerBase upcomingevents = new ServerBase("upcomingevents");
                upcomingevents.SelectFilter("ID = " + id.ToString());
                DataTable upcomingeventsData = upcomingevents.SelectQuery();

                if (upcomingeventsData.Rows.Count == 1)
                {
                    ServerBase country_ = new ServerBase("Country");
                    country_.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> ''"); // 3 = Asia
                    DataTable countryData = country_.SelectQuery();

                    DataTable dtResult = new DataTable();
                    dtResult.Columns.Add("ID", typeof(int));
                    dtResult.Columns.Add("Title", typeof(string));
                    dtResult.Columns.Add("Description", typeof(string));
                    dtResult.Columns.Add("Detail", typeof(string));
                    dtResult.Columns.Add("Author", typeof(string));
                    dtResult.Columns.Add("Thumbnail", typeof(string));
                    dtResult.Columns.Add("Country", typeof(string));
                    dtResult.Columns.Add("ResultsAchieved", typeof(string));
                    dtResult.Columns.Add("ChallengesLessonLearned", typeof(string));
                    dtResult.Columns.Add("Replicability", typeof(string));
                    dtResult.Columns.Add("Sources", typeof(string));
                    dtResult.Columns.Add("Company", typeof(string));
                    dtResult.Columns.Add("Email", typeof(string));
                    dtResult.Columns.Add("Phone", typeof(string));
                    dtResult.Columns.Add("Initiative", typeof(string));
                    dtResult.Columns.Add("Photo", typeof(string));
                    dtResult.Columns.Add("BlogDate", typeof(string));
                    dtResult.Columns.Add("Location", typeof(string));

                    var JoinResult = from a in upcomingeventsData.AsEnumerable()
                                     join b in countryData.AsEnumerable()
                                     on a.Field<int>("Country_ID").ToString() equals b.Field<int>("ID").ToString()
                                     select dtResult.LoadDataRow(new object[]
                                     {
                                    a.Field<int>("ID"),
                                    a.Field<string>("Title"),
                                    a.Field<string>("Description"),
                                    a.Field<string>("Detail"),
                                    a.Field<string>("Author"),
                                    a.Field<string>("Thumbnail"),
                                    b.Field<string>("Name"),
                                    a.Field<string>("ResultsArchieved"),
                                    a.Field<string>("ChallengesLessonLearned"),
                                    a.Field<string>("Replicability"),
                                    a.Field<string>("Sources"),
                                    a.Field<string>("Company"),
                                    a.Field<string>("Email"),
                                    a.Field<string>("Phone"),
                                    a.Field<string>("Initiative"),
                                    a.Field<string>("Photo"),
                                    a.Field<string>("StartDate"),
                                    a.Field<string>("Location"),
                                     }, false);
                    JoinResult.CopyToDataTable();

                    ViewBag.Data = dtResult.Rows[0];

                    return View("UpcomingEventsItem");
                }
                else
                {
                    return View("Empty", "Pages");
                }
            }
        }

        [HttpGet]
        public String getSolidWasteGraphData(int country,int city,int year)
        {
            DataTable dtResult = new DataTable();
            MSSQLServer db = new MSSQLServer();
            string yearFilter = "";
            if (year > 0) yearFilter = " and [Year] = " + year.ToString();
            string countryFilter = "";
            if (country > 0) countryFilter = "select ID from city where [Country_ID] = " + country.ToString();

            string limit = "top 100";
            if (IsMobileDevice(Request))
            {
                limit = "top 5";
            }

            if (city > 0)
            {
                db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from citywastestreams where city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and City_ID in (" + city + ") " + yearFilter + " and WasteCategory_ID = 1 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                dtResult = db.ExecuteQuery();
            }
            else if (city == 0)
            {
                if (country == 0)
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from citywastestreams where city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and City_ID in (" + city + ") " + yearFilter + " and WasteCategory_ID = 1 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from citywastestreams where city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and City_ID in (" + countryFilter + ") " + yearFilter + " and WasteCategory_ID = 1 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
            }
            else
            {
                if (country == 0)
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from countrywastestreams where country_id in (select id from country where subregion_id = 3) " + yearFilter + " and WasteCategory_ID = 1 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from countrywastestreams where country_id in (select id from country where subregion_id = 3) " + yearFilter + " and Country_ID in (" + country + ") and WasteCategory_ID = 1 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
            }

            foreach (DataColumn col in dtResult.Copy().Columns)
            {
                double total = 0;
                foreach(DataRow row in dtResult.Rows)
                {
                    try
                    {
                        total += Convert.ToDouble(row[col.ToString()].ToString());
                    }
                    catch
                    {
                        total += 0;
                    }
                }
                if(total == 0)
                {
                    dtResult.Columns.Remove(col.ToString());
                }
            }

            return JsonConvert.SerializeObject(dtResult);
        }

        [HttpGet]
        public String getPlasticWasteGraphData(int country, int city, int year)
        {
            DataTable dtResult = new DataTable();
            MSSQLServer db = new MSSQLServer();
            string yearFilter = "";
            if (year > 0) yearFilter = " and [Year] = " + year.ToString();
            string countryFilter = "";
            if (country > 0) countryFilter = "select ID from city where [Country_ID] = " + country.ToString();

            string limit = "top 100";
            if (IsMobileDevice(Request))
            {
                limit = "top 5";
            }

            if (city > 0)
            {
                db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from citywastestreams where city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and City_ID in (" + city + ") " + yearFilter + " and WasteCategory_ID = 14 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                dtResult = db.ExecuteQuery();
            }else if (city == 0)
            {
                if (country == 0)
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from citywastestreams where city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and City_ID in (" + city + ") " + yearFilter + " and WasteCategory_ID = 14 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from citywastestreams where city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and City_ID in (" + countryFilter + ") " + yearFilter + " and WasteCategory_ID = 14 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
            }
            else
            {
                if (country == 0)
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from countrywastestreams where country_id in (select id from country where subregion_id = 3) " + yearFilter + " and WasteCategory_ID = 14 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select * from (select {limit} [Year],SUM(cast(Totalgenerated as decimal(10,2))) as Generated,SUM(cast(Totalcollected as decimal(10,2))) as Collected,SUM(cast(Recycled as decimal(10,2))) as Recycled,SUM(cast(Recovered as decimal(10,2))) as Recovered,SUM(cast(Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference from countrywastestreams where country_id in (select id from country where subregion_id = 3) " + yearFilter + " and Country_ID in (" + country + ") and WasteCategory_ID = 14 and Deleted = 0 and ([year] is not null and [year] <> '') group by [year] order by [year] desc) as lastfive order by Year asc";
                    dtResult = db.ExecuteQuery();
                }
            }

            foreach (DataColumn col in dtResult.Copy().Columns)
            {
                double total = 0;
                foreach (DataRow row in dtResult.Rows)
                {
                    try
                    {
                        total += Convert.ToDouble(row[col.ToString()].ToString());
                    }
                    catch
                    {
                        total += 0;
                    }
                }
                if (total == 0)
                {
                    dtResult.Columns.Remove(col.ToString());
                }
            }

            return JsonConvert.SerializeObject(dtResult);
        }

        [HttpGet]
        public String getSolidWasteTableData(int country, int city, int year)
        {
            MSSQLServer db = new MSSQLServer();
            DataTable dtResult = new DataTable();
            string yearFilter = "";
            if (year > 0) yearFilter = " and [Year] = " + year.ToString();
            string countryFilter = "";
            if (country > 0) countryFilter = "select ID from city where [Country_ID] = " + country.ToString();

            if (city > 0)
            {
                db.Query = $"select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                dtResult = db.ExecuteQuery();
            }
            else if (city == 0)
            {
                if (country == 0)
                {
                    db.Query = $"select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + countryFilter + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }
            else
            {
                if (country == 0)
                {
                    db.Query = $"select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.Country_ID in (select id from country where subregion_id = 3) and a.WasteCategory_ID = 1 " + yearFilter + " and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.Country_ID in (select id from country where subregion_id = 3) and a.Country_ID in (" + country + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }

            dtResult.TableName = "data";

            return JsonConvert.SerializeObject(dtResult);
        }

        [HttpGet]
        public String getPlasticWasteTableData(int country, int city, int year)
        {
            MSSQLServer db = new MSSQLServer();
            DataTable dtResult = new DataTable();
            string yearFilter = "";
            if (year > 0) yearFilter = " and [Year] = " + year.ToString();
            string countryFilter = "";
            if (country > 0) countryFilter = "select ID from city where [Country_ID] = " + country.ToString();

            if (city > 0)
            {
                db.Query = $"select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                dtResult = db.ExecuteQuery();
            }else if (city == 0)
            {
                if (country == 0)
                {
                    db.Query = $"select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else {
                    db.Query = $"select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + countryFilter + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }
            else
            {
                if (country == 0)
                {
                    db.Query = $"select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.country_id in (select id from country where subregion_id = 3) and a.WasteCategory_ID = 14 " + yearFilter + " and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = $"select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.country_id in (select id from country where subregion_id = 3) and a.Country_ID in (" + country + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year] desc";
                    dtResult = db.ExecuteQuery();
                }
            }

            dtResult.TableName = "data";

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

        [HttpGet]
        public String getCountry(int subregion)
        {
            var countryData = context.countries.Where(x => x.SubRegion_ID == subregion.ToString() && x.Deleted == (byte)0);

            return JsonConvert.SerializeObject(countryData);
        }

        [HttpGet]
        public String getCity(int country)
        {
            ServerBase country_ = new ServerBase("City");
            country_.SelectFilter("Country_ID in (" + country.ToString() + ")"); // 3 = Asia
            DataTable countryData = country_.SelectQuery();

            return JsonConvert.SerializeObject(countryData);
        }

        [HttpGet]
        public String getYears(int country)
        {
            string[] yearData;
            if(country == 0)
            {
                yearData = context.countrywastestreams.Where(x => x.Deleted == (byte)0).Select(x => x.Year).Distinct().OrderByDescending(year => year).ToArray();
            }
            else
            {
                yearData = context.countrywastestreams.Where(x => x.Country_ID == country.ToString() && x.Deleted == (byte)0).Select(x => x.Year).Distinct().OrderByDescending(year => year).ToArray();
            }

            return JsonConvert.SerializeObject(yearData);
        }

        [HttpGet]
        public String getDataYears(int country, int city)
        {
            if(city > 0)
            {
                ServerBase cw = new ServerBase("CityWastestreams");
                cw.SelectFilter("City_ID in (" + city.ToString() + ")");
                cw.SelectOrder("Year", Web.Framework.Enums.EnumOrder.ASCENDING);
                DataTable cwData = cw.SelectDistinct("Year");

                return JsonConvert.SerializeObject(cwData);
            }else if (city == 0)
            {
                ServerBase cw = new ServerBase("CityWastestreams");
                cw.SelectFilter("City_ID in (" + city.ToString() + ")");
                cw.SelectOrder("Year", Web.Framework.Enums.EnumOrder.ASCENDING);
                DataTable cwData = cw.SelectDistinct("Year");

                return JsonConvert.SerializeObject(cwData);
            }
            else
            {
                if(country == 0)
                {
                    ServerBase cw = new ServerBase("CountryWastestreams");
                    cw.SelectOrder("Year", Web.Framework.Enums.EnumOrder.ASCENDING);
                    DataTable cwData = cw.SelectDistinct("Year");

                    return JsonConvert.SerializeObject(cwData);
                }
                else
                {
                    ServerBase cw = new ServerBase("CountryWastestreams");
                    cw.SelectFilter("Country_ID in (" + country + ")");
                    cw.SelectOrder("Year", Web.Framework.Enums.EnumOrder.ASCENDING);
                    DataTable cwData = cw.SelectDistinct("Year");

                    return JsonConvert.SerializeObject(cwData);
                }
            }
        }
        private bool IsMobileDevice(HttpRequestBase request)
        {
            string userAgent = request.UserAgent?.ToLower() ?? string.Empty;

            // List of mobile devices or OS indicators
            string[] mobileDevices = new[] {
                "iphone", "ipod", "ipad", "android", "windows phone", "blackberry", "opera mini", "mobile"
            };

            return mobileDevices.Any(device => userAgent.Contains(device));
        }
    }
}