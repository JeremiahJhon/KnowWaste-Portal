using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using UCOnline.Entity;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;
using Web.Framework.Server;

namespace UCOnline.Models
{
    public class ModelWasteStream : ModelBase
    {
        public string CountryID { get; set; }
        public string SubRegionID { get; set; }
        public string Year { get; set; }

        public List<string> RefIDs { get; set; }

        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelWasteStream() : base("countrywastestreams", "Waste Streams")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";
            QuerySetting.QueryAll = true;

            ViewSetting.SHOWTABLEHEADER = false;
            ViewSetting.SHOWEDITOR = false;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();

                var combocat = new PropertyComboBox("wastecategory");
                combocat.Select = new TableColumnQuery("wastecategory", "name");
                columns.Add(combocat);

                var combotype = new PropertyComboBox("wastetype");
                combotype.Visible = false;
                combotype.Select = new TableColumnQuery("wastecategory", "type");
                columns.Add(combotype);

                columns.Add(new PropertyTextHidden("Definitions"));
                columns.Add(new PropertyTextHidden("Defineby"));

                columns.Add(new PropertyText("Totalgenerated", "Generated"));
                columns.Add(new PropertyText("Hazardous"));
                columns.Add(new PropertyText("Totalcollected", "Collected"));
                columns.Add(new PropertyText("Recycled"));
                columns.Add(new PropertyText("Recovered"));
                columns.Add(new PropertyText("Disposal"));
                columns.Add(new PropertyText("Treatment"));
                columns.Add(new PropertyText("Reuse"));
                columns.Add(new PropertyText("Sludge"));
                columns.Add(new PropertyText("reference", "ref"));

                var country = new PropertyComboBox("country");
                country.Visible = false;
                country.Select = new TableColumnQuery("country", "name");
                columns.Add(country);

                var subregion = new PropertyComboBox("subregion");
                subregion.Visible = false;
                subregion.Select = new TableColumnQuery("country", "subregion_id");
                columns.Add(subregion);

                properties.Columns = columns;
            }
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);

            if (CountryID != "0")
            {
                var countryid = CountryID;

                server.SelectFilter("country_id", countryid);

                if (Year != "0")
                    server.SelectFilter("year", Year);              
            }
            
        }

        protected override void UpdateBeforeShow(DataTable data)
        {
            base.UpdateBeforeShow(data);

            int i;

            RefIDs = new List<string>();

            if (Year == "0")
            {
                List<string> categories = new List<string>();

                foreach (DataRow row in data.Rows)
                {
                    var category = row["wastecategory"].ToString();

                    if (!categories.Contains(category))
                        categories.Add(category);
                }

                DataTable summary = data.Clone();
                List<DataRow> Rows = new List<DataRow>();

                String newdata;
                String current;

                string[] columns = new string[] { "totalgenerated", "hazardous", "totalcollected", "recycled", "disposal", "treatment", "reuse", "sludge" };

                for (i = 0; i < categories.Count; i++)
                {
                    DataRow newrow = summary.NewRow();
                    newrow["wastecategory"] = categories[i];

                    foreach (DataRow row in data.Rows)
                    {
                        var category = row["wastecategory"].ToString();
                        if (category == categories[i])
                        {
                            foreach (string column in columns)
                            {
                                newdata = newrow[column].ToString();
                                current = row[column].ToString();

                                if (newdata == "")
                                    newdata = "0";

                                if (current == "")
                                    current = "0";

                                newdata = newdata.Replace(",", "");
                                current = current.Replace(",", "");

                                newrow[column] = double.Parse(newdata, CultureInfo.InvariantCulture) + double.Parse(current, CultureInfo.InvariantCulture);
                            }
                        }
                    }

                    Rows.Add(newrow);
                }

                data.Clear();

                for (i = 0; i < Rows.Count; i++)
                    data.Rows.Add(Rows[i].ItemArray);
            }

            i = 1;

            foreach (DataRow row in data.Rows)
            {
                var category = row["wastecategory"].ToString();
                var definition = row["Definitions"].ToString().Trim();
                var defineby = row["definedby"].ToString().Trim();
                var reference = row["reference"].ToString();

                var refid = row["datasource"].ToString().Trim();
                RefIDs.Add(refid);

                if (definition != "")
                    row["wastecategory"] = string.Format("{0} <div class='table-info'><i class='fa fa-info'></i><div class='table-tooltip'>{1} <span class='source'>{3}</span></div></div>", category, definition, i, defineby);

                if (reference != "")
                    row["reference"] = string.Format("<div class='table-info'><i class='fa fa-info'></i><div class='table-tooltip table-tooltip-right'>{0}</div></div>", reference);

                row["datasource"] = string.Format("<a href='#name{1}'>{0}</a>", row["datasource"], i);
                i++;
            }
        }

        public string GetYear(string cid, ref string selected)
        {
            ServerBase serveryear = new ServerBase("countrywastestreams");

            serveryear.SelectColumn("DISTINCT(year) AS year");
            serveryear.SelectFilter("country_id", cid);
            serveryear.SelectOrder("year", Web.Framework.Enums.EnumOrder.DESCENDING);

            DataTable data = serveryear.SelectQuery();
            string year = null;

            StringBuilder output = new StringBuilder();
            output.Append("<select class='year' id='yearFilter'>");
            output.AppendFormat("<option value='0'>All</option>");

            var i = 0;

            foreach(DataRow row in data.Rows)
            {
                year = row["year"].ToString();

                if (selected != null)
                {
                    if (selected == year)
                        output.AppendFormat("<option value='{0}' selected>{0}</option>", year);
                    else
                        output.AppendFormat("<option value='{0}'>{0}</option>", year);
                }
                else
                {
                    if (i++ == 0)
                    {
                        output.AppendFormat("<option value='{0}' selected>{0}</option>", year);
                        selected = year;
                    }
                    else
                        output.AppendFormat("<option value='{0}'>{0}</option>", year);
                }
            }

            output.Append("</select>");

            return output.ToString();
        }

        protected override HTMLBase GetViewer(DataTable data)
        {
            HTMLTable table = (HTMLTable) base.GetViewer(data);
            table.Class += " waste-table";
            return table;
        }

        public List<Waste> GetObject()
        {
            DataTable data = GetData();
            var wastes = new List<Waste>();

            foreach (DataRow row in data.Rows)
            {
                if (row["subregion"].ToString() == SubRegionID)
                {
                    var waste = new Waste();
                    waste.Country = row["country"].ToString();
                    waste.ID = row["wastecategory_id"].ToString();
                    waste.Category = row["wastecategory"].ToString();
                    waste.Type = GetInt(row["wastetype"]);
                    waste.Generated = GetInt(row["Totalgenerated"]);
                    waste.Hazardous = GetInt(row["Hazardous"]);
                    waste.Collected = GetInt(row["Totalcollected"]);
                    waste.Recycled = GetInt(row["Recycled"]);
                    waste.Recovered = GetInt(row["Recovered"]);
                    waste.Disposal = GetInt(row["Disposal"]);
                    waste.Treatment = GetInt(row["Treatment"]);
                    waste.Reuse = GetInt(row["Reuse"]);
                    waste.Sludge = GetInt(row["Sludge"]);
                    waste.Year = GetInt(row["Year"]);

                    wastes.Add(waste);
                }
            }

            return wastes;
        }

        public int GetInt(object row)
        {
            if (row is DBNull)
                return 0;
            else
            {
                var temp = row.ToString();
                temp = temp.Replace(",", "");

                int value;
                double dValue;
                double.TryParse(temp, out dValue);
                int.TryParse(dValue.ToString(), out value);

                return value;
            }
        }
    }
}