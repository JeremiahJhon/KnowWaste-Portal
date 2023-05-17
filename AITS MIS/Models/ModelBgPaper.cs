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
    public class ModelBgPaper : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelBgPaper() : base("backgroundpaper", "Background Papers")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "title";

            ViewSetting.SHOWEDITOR = true;
            //ViewSetting.LinkColumn = "code";

            //ViewSetting.NewTitle = "New Project";
            //ViewSetting.EditTitle = "Edit Project";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("title"));
                columns.Add(new PropertyText("abstract"));
                columns.Add(new PropertyText("author"));
                columns.Add(new PropertyText("posteddate"));

                //var combo = new PropertyComboBox("client");
                //combo.Select = new TableColumnQuery("client", "name");

                //columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}