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
    public class ModelExperts : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }
        public string geothemeid { get; set; }

        public ModelExperts() : base("expertrosters", "Expert Rosters")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "Lastname";
            QuerySetting.QueryAll = true;

            ViewSetting.SHOWEDITOR = true;

            geothemeid = "1";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("firstname"));
                columns.Add(new PropertyText("lastname"));
                columns.Add(new PropertyText("position"));
                columns.Add(new PropertyText("organization"));
                columns.Add(new PropertyText("expertise"));
                columns.Add(new PropertyText("description"));

                var combo = new PropertyComboBox("country");
                combo.Select = new TableColumnQuery("country", "name");

                columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}