using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;

namespace UCOnline.Models
{
    public class ModelCountryReferences : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelCountryReferences() : base("countryreferences", "References")
        {
            QuerySetting.QueryAll = true;
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.DESCENDING;
            QuerySetting.SortColumn = "referencenumber";

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "id";

            //ViewSetting.NewTitle = "New Task";
            //ViewSetting.EditTitle = "Edit Task";

            if (properties == null)
            {
                properties = new PropertyTable();
                properties.HasChild = true;

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("referencenumber"));
                columns.Add(new PropertyText("reference"));
                
                properties.Columns = columns;
            }
        }
    }
}