using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;

namespace UCOnline.Models
{
    public class ModelFaqs : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelFaqs() : base("faqs", "Frequently Asked Questions")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";
            QuerySetting.QueryAll = true;

            ViewSetting.SHOWEDITOR = true;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("sector_id"));
                columns.Add(new PropertyText("answers"));
                columns.Add(new PropertyText("dateentered"));

                var combo = new PropertyComboBox("sector");
                combo.Select = new TableColumnQuery("sector", "name");
                columns.Add(combo);

                //var combo = new PropertyComboBox("client");
                //combo.Select = new TableColumnQuery("client", "name");

                //columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}