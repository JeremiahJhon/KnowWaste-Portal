using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UCOnline.Data;
using UCOnline.Entity;
using UCOnline.Models;
using Web.Framework.Controllers;
using Web.Framework.Server;

namespace UCOnline.Controllers
{
    public class DataController :  BaseController
    {
        public DataController() : base(new ModelData())
        {
            ViewBag.Title = "Data and Trends - kNOw.Waste Management Platform";            
        }

        public override ActionResult Index()
        {
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            var regionid = Request.QueryString["regionid"];
            var subregionid = Request.QueryString["subregionid"];
            var countryid = Request.QueryString["countryid"];

            if (regionid == null)
                regionid = "3";

            var year = Request.QueryString["year"];

            return refresh(regionid, subregionid, countryid, year);
        }

        [HttpGet]
        public string Body(string regionid, string subregionid, string countryid, string year)
        {
            refresh(regionid, subregionid, countryid, year);

            String body = ViewBag.Body;

            return body;
        }

        [HttpGet]
        public override ActionResult Data()
        {
            var regionid = Request.QueryString["regionid"];
            var subregionid = Request.QueryString["subregionid"];
            var countryid = Request.QueryString["countryid"];

            if (regionid == null)
                regionid = "3";

            ServerBase server = new ServerBase("subregion");
            server.SelectFilter("region_id", regionid);

            DataTable data = server.SelectQuery();

            var handle = true;

            foreach (DataRow row in data.Rows)
                if (row["id"].ToString() == subregionid)
                {
                    handle = false;
                    break;
                }

            if (handle && data.Rows.Count != 0)
                subregionid = data.Rows[0]["id"].ToString();

            if (countryid == null && subregionid != null)
                countryid = "2";

            server = new ServerBase("country");
            server.SelectFilter("subregion_id", subregionid);

            data = server.SelectQuery();

            handle = true;

            if (countryid != "0")
            {
                foreach (DataRow row in data.Rows)
                    if (row["id"].ToString() == countryid)
                    {
                        handle = false;
                        break;
                    }

                if (handle && data.Rows.Count != 0 && subregionid != null)
                    countryid = data.Rows[0]["id"].ToString();
            }

            var year = Request.QueryString["year"];

            var wstream = new ModelWasteStream();

            if (year == null)
                wstream.GetYear(countryid, ref year);

            var stream = new ModelWasteStream();
            stream.CountryID = countryid;
            stream.SubRegionID = subregionid;
            stream.Year = year;

            var waste = new List<Waste>();
            waste = stream.GetObject();

            return Json(waste, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportToCsv(int country, int city, int year)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(getSolidWasteTableData(country, city, year));
            ds.Tables.Add(getPlasticWasteTableData(country, city, year));

            string csvContent = DataSetToCsv(ds);

            // Return the CSV file as a FileResult for download
            byte[] byteArray = Encoding.UTF8.GetBytes(csvContent);
            MemoryStream csvStream = new MemoryStream(byteArray);

            return File(csvStream, "text/csv", "wastedata.csv");
        }

        private string DataTableToCsv(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            // Write column headers
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append('"' + column.ColumnName.Replace("\"", "\"\"") + '"' + ",");
            }
            sb.AppendLine();

            // Write rows
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append('"' + row[column].ToString().Replace("\"", "\"\"") + '"' + ",");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        static string DataSetToCsv(DataSet dataSet)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataTable dataTable in dataSet.Tables)
            {
                // Write table name as a CSV section header
                sb.AppendLine(dataTable.TableName);

                // Write column headers
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append('"' + column.ColumnName.Replace("\"", "\"\"") + '"' + ",");
                }
                sb.AppendLine();

                // Write rows
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        sb.Append('"' + row[column].ToString().Replace("\"", "\"\"") + '"' + ",");
                    }
                    sb.AppendLine();
                }

                // Add an empty line between tables
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private DataTable getSolidWasteTableData(int country, int city, int year)
        {
            MSSQLServer db = new MSSQLServer();
            DataTable dtResult = new DataTable();
            string yearFilter = "";
            if (year > 0) yearFilter = " and [Year] = " + year.ToString();
            string countryFilter = "";
            if (country > 0) countryFilter = "select ID from city where [Country_ID] = " + country.ToString();

            if (city > 0)
            {
                db.Query = "select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                dtResult = db.ExecuteQuery();
            }
            else if (city == 0)
            {
                if (country == 0)
                {
                    db.Query = "select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = "select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + countryFilter + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }
            else
            {
                if (country == 0)
                {
                    db.Query = "select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.Country_ID in (select id from country where subregion_id = 3) and a.WasteCategory_ID = 1 " + yearFilter + " and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = "select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.Country_ID in (select id from country where subregion_id = 3) and a.Country_ID in (" + country + ") " + yearFilter + " and a.WasteCategory_ID = 1 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }

            dtResult.TableName = "Solid Waste Data";

            return dtResult;
        }

        private DataTable getPlasticWasteTableData(int country, int city, int year)
        {
            MSSQLServer db = new MSSQLServer();
            DataTable dtResult = new DataTable();
            string yearFilter = "";
            if (year > 0) yearFilter = " and [Year] = " + year.ToString();
            string countryFilter = "";
            if (country > 0) countryFilter = "select ID from city where [Country_ID] = " + country.ToString();

            if (city > 0)
            {
                db.Query = "select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                dtResult = db.ExecuteQuery();
            }
            else if (city == 0)
            {
                if (country == 0)
                {
                    db.Query = "select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + city + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = "select b.Name as City,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from citywastestreams a inner join city b on a.city_id = b.id where a.city_id in (SELECT id from city where Country_ID in (select id from country where subregion_id = 3)) and a.City_ID in (" + countryFilter + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }
            else
            {
                if (country == 0)
                {
                    db.Query = "select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.country_id in (select id from country where subregion_id = 3) and a.WasteCategory_ID = 14 " + yearFilter + " and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
                else
                {
                    db.Query = "select b.Name as Country,a.[Year],SUM(cast(a.Totalgenerated as decimal(10,2))) as Generated,SUM(cast(a.Totalcollected as decimal(10,2))) as Collected,SUM(cast(a.Recycled as decimal(10,2))) as Recycled,SUM(cast(a.Recovered as decimal(10,2))) as Recovered,SUM(cast(a.Disposal as decimal(10,2))) as Disposal,STRING_AGG(reference,'') as Reference,STRING_AGG(Definitions,'') as Definitions from countrywastestreams a inner join country b on a.country_id = b.id where a.country_id in (select id from country where subregion_id = 3) and a.Country_ID in (" + country + ") " + yearFilter + " and a.WasteCategory_ID = 14 and a.Deleted = 0 and ([year] is not null and [year] <> '') group by b.Name,a.[Year] order by b.Name,a.[Year]";
                    dtResult = db.ExecuteQuery();
                }
            }

            dtResult.TableName = "Plastic Waste Data";

            return dtResult;
        }

        private ActionResult refresh(string regionid, string subregionid, string countryid, string year)
        {
            ServerBase server = new ServerBase("subregion");
            server.SelectFilter("region_id", regionid);

            DataTable data = server.SelectQuery();

            var handle = true;

            foreach (DataRow row in data.Rows)
                if (row["id"].ToString() == subregionid)
                {
                    handle = false;
                    break;
                }

            if (handle && data.Rows.Count != 0)
                subregionid = data.Rows[0]["id"].ToString();
            if (countryid == null && subregionid != null)
                countryid = "2";

            server = new ServerBase("country");
            server.SelectFilter("subregion_id", subregionid);

            data = server.SelectQuery();
            handle = true;

            if (countryid != "0")
            {
                foreach (DataRow row in data.Rows)
                    if (row["id"].ToString() == countryid)
                    {
                        handle = false;
                        break;
                    }

                if (handle && data.Rows.Count != 0 && subregionid != null)
                    countryid = data.Rows[0]["id"].ToString();
            }

            var wstream = new ModelWasteStream();

            if (year == null)
                wstream.GetYear(countryid, ref year);

            if (subregionid != null)
            {
                if (countryid != null)
                {
                    ViewBag.Year = wstream.GetYear(countryid, ref year);

                    ((ModelData)Model).Year = year;
                    ((ModelData)Model).CountryID = countryid;

                    //List of Countries
                    var country = new ModelCountry();
                    country.SubRegionID = subregionid;
                    country.CountyID = countryid;
                    ViewBag.Country = country.GetCombobox();
                }

                //List Subregions
                var subregion = new ModelSubRegion();
                subregion.RegionID = regionid;
                subregion.SubRegionID = subregionid;
                ViewBag.SubRegion = subregion.GetCombobox();
            }

            //List of Regions
            var region = new ModelRegion();
            region.RegionID = regionid;
            ViewBag.Region = region.GetCombobox();

            return base.Index();
        }
    }
}