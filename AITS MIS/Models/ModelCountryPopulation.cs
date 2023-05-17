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
    public class ModelCountryPopulation : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelCountryPopulation() : base("countrypopulation", "Country Population")
        {
            QuerySetting.QueryAll = true;
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "id";

            //ViewSetting.NewTitle = "New Task";
            //ViewSetting.EditTitle = "Edit Task";

            if (properties == null)
            {
                properties = new PropertyTable();
                properties.HasChild = true;

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("population"));
                columns.Add(new PropertyText("urbanpopulation"));
                columns.Add(new PropertyText("area"));
                columns.Add(new PropertyText("incomelevel"));

                properties.Columns = columns;
            }
        }
    }
}