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
    public class ModelRegion : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }
        
        public string RegionID { get; set; }

        public ModelRegion() : base("region", "Region Wise")
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
                columns.Add(new PropertyText("name"));                                

                properties.Columns = columns;
            }
        }

        public string GetCombobox()
        {
            StringBuilder output = new StringBuilder();

            ServerBase region = new ServerBase("Region");
            region.SelectFilter("Name = 'Asia'");
            DataTable data = region.SelectQuery();

            output.Append("<select class='region'>");

            foreach (DataRow row in data.Rows)
            {
                var id = row["id"].ToString();

                if (RegionID == id)
                    output.AppendFormat("<option value='{0}' selected>{1}</option>", id, row["name"]);
                else
                    output.AppendFormat("<option value='{0}'>{1}</option>", id, row["name"]);
            }

            output.Append("</select>");

            return output.ToString();
        }
    }
}