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
    public class ModelProjectTask : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelProjectTask() : base("projecttask", "Project Task")
        {
            QuerySetting.QueryAll = true;
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "name";

            ViewSetting.NewTitle = "New Task";
            ViewSetting.EditTitle = "Edit Task";

            if (properties == null)
            {
                properties = new PropertyTable();
                properties.HasChild = true;

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("project_id"));
                columns.Add(new PropertyText("name"));
                columns.Add(new PropertyText("description"));
                columns.Add(new PropertyChild("employee", "projecttaskemployee", "employee_id, (SELECT CONCAT_WS(' ', contact.firstname, contact.lastname) FROM contact, employee WHERE contact.id=employee.contact_id AND employee.id=projecttaskemployee.employee_id) AS employee"));

                var combo = new PropertyComboBox("projecttaskstatus", "status");
                combo.Select = new TableColumnQuery("projecttaskstatus", "name");
                columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}