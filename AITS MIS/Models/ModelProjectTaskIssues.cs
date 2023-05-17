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
    public class ModelProjectTaskIssues : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelProjectTaskIssues() : base("projecttaskissues", "Issues")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "name";

            ViewSetting.NewTitle = "New Task Issue";
            ViewSetting.EditTitle = "Edit Task Issue";

            if (properties == null)
            {
                properties = new PropertyTable();
                properties.HasChild = true;

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("projecttask_id"));
                columns.Add(new PropertyText("name"));
                columns.Add(new PropertyText("description"));

                var combo = new PropertyComboBox("projecttaskstatus", "status");
                combo.Select = new TableColumnQuery("projecttaskstatus", "name");
                columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}