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
    public class ModelSubRegion : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public string RegionID { get; set; }

        public string SubRegionID { get; set; }

        public ModelSubRegion() : base("subregion", "Sub Region")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "name";

            ViewSetting.SHOWEDITOR = true;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("name"));                
                
                properties.Columns = columns;
            }
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);
            server.SelectFilter("region_id", RegionID);
        }

        public string GetCombobox()
        {
            StringBuilder output = new StringBuilder();

            DataTable data = GetData();

            output.Append("<select class='subregion'>");

            foreach (DataRow row in data.Rows)
            {
                var id = row["id"].ToString();

                if (SubRegionID == id)
                    output.AppendFormat("<option value='{0}' selected>{1}</option>", id, row["name"]);
                else
                    output.AppendFormat("<option value='{0}'>{1}</option>", id, row["name"]);
            }

            output.Append("</select>");

            return output.ToString();
        }
    }
}