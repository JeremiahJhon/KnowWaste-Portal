using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;
using Web.Framework.Server;

namespace UCOnline.Models
{
    public class ModelCountryPolicy : ModelBase
    {
        public string CountyID { get; set; }

        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelCountryPolicy() : base("countrypolicy", "Country Policy")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";
            QuerySetting.QueryAll = true;

            ViewSetting.SHOWTABLEHEADER = false;
            ViewSetting.SHOWEDITOR = false;

            CountyID = "1";
            ControllerName = "LawPolicy";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();

                columns.Add(new PropertyText("year"));
                columns.Add(new PropertyText("legal"));
                columns.Add(new PropertyText("description"));

                properties.Columns = columns;
            }
        }

        public virtual string ShowList()
        {
            StringBuilder output = new StringBuilder();

            QuerySetting.QueryAll = false;

            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();


            //Use Table as default viewer
            if (Title != null)
                ViewSetting.Title = Title;
            else
                ViewSetting.Title = data.TableName;

            ViewSetting.Controller = ControllerName;
            ViewSetting.URL = url;

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            ViewSetting.Count = count;
            ViewSetting.PageLimit = server.PageLimit;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                output.Append("<div class='c-margin-b-40'>");
                output.AppendFormat("<h3 class='c-title c-font-bold c-font-22 c-font-dark'>{0}</h3>", row["legal"]);
                output.AppendFormat("<p class='c-font-14 c-font-thin c-theme-font'>{0}</p>", row["year"]);
                output.AppendFormat("<p>{0}</p>", row["description"]);
                output.Append("</div>");
            }

            output.Append(ViewSetting.GeneratePager());

            QuerySetting.QueryAll = true;

            return output.ToString();
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);

            var countryid = CountyID;
            server.SelectFilter("country_id", countryid);
        }

        public override string Show()
        {
            DataTable data = GetData();
            StringBuilder output = new StringBuilder();

            output.Append("<div class='policy'>");

            foreach (DataRow row in data.Rows)
            {
                output.Append("<div class='c-margin-b-40'>");
                output.AppendFormat("<h3 class='c-title c-font-18 c-font-dark'>{0}</h3>", row["legal"]);
                output.AppendFormat("<p class='c-font-14 c-font-thin c-theme-font'>{0}</p>", row["year"]);
                output.AppendFormat("<div class='expand-mobile'>{0} <i class='fa fa-chevron-down'></i></div>", row["description"]);
                output.Append("</div>");
            }

            output.Append("</div>");

            return output.ToString();
        }

        public DataTable GetData()
        {
            DataSet data = new DataSet();
            ServerBase countryPolicy = new ServerBase("CountryPolicy");
            countryPolicy.SelectFilter("(Area_ID > 0 and Area_ID is not null) and (Country_ID > 0 and Country_ID is not null)");
            countryPolicy.SelectOrder("ID", Web.Framework.Enums.EnumOrder.DESCENDING);
            DataTable countryPolicyData = countryPolicy.SelectQuery();
            ServerBase country = new ServerBase("Country");
            country.SelectFilter("SubRegion_ID is not null and SubRegion_ID <> '' and SubRegion_ID in (select ID from SubRegion where Region_id = 3)"); // 3 = Asia
            DataTable countryData = country.SelectQuery();
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
                return dtResult;
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
            return dtResult;
        }
    }
}