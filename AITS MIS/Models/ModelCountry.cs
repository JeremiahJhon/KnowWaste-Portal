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

namespace UCOnline.Models
{
    public class ModelCountry : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public string CountyID { get; set; }
        public string SubRegionID { get; set; }

        public ModelCountry() : base("country", "Country")
        {
            QuerySetting.QueryAll = true;
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "name";

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "name";

            if (properties == null)
            {
                properties = new PropertyTable();
                properties.HasChild = true;

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("subregion_id"));
                columns.Add(new PropertyText("name"));                                

                properties.Columns = columns;
            }
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);
            server.SelectFilter("subregion_id", SubRegionID);
        }

        public string GetCombobox()
        {
            StringBuilder output = new StringBuilder();

            DataTable data = GetData();

            output.Append("<select class='country'>");
            output.AppendFormat("<option value='0'>All</option>");

            foreach (DataRow row in data.Rows)
            {
                var id = row["id"].ToString();

                if (CountyID == id)
                    output.AppendFormat("<option value='{0}' selected>{1}</option>", id, row["name"]);
                else
                    output.AppendFormat("<option value='{0}'>{1}</option>", id, row["name"]);
            }

            output.Append("</select>");

            return output.ToString();
        }
    }
}