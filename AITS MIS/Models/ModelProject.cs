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
    public class ModelProject : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelProject() : base("project", "Projects")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "name";

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "code";

            ViewSetting.NewTitle = "New Project";
            ViewSetting.EditTitle = "Edit Project";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("code"));
                columns.Add(new PropertyText("name"));
                columns.Add(new PropertyText("description"));

                var combo = new PropertyComboBox("client");
                combo.Select = new TableColumnQuery("client", "name");

                columns.Add(combo);
                    
                properties.Columns = columns;
            }
        }
    }
}