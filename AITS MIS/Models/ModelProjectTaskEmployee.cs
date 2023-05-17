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
    public class ModelProjectTaskEmployee : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelProjectTaskEmployee() : base("projecttaskemployee", "Assigned Employees")
        {
            QuerySetting.QueryAll = true;
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "employee";

            ViewSetting.NewTitle = "Assign Employee";
            ViewSetting.EditTitle = "Assign Employee";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("projecttask_id"));

                var combo = new PropertyComboBox("employee", "employee");
                combo.Select = new TableColumnQuery("employee", "(SELECT CONCAT_WS(' ', firstname, lastname) FROM contact WHERE contact.id=employee.contact_id) AS employee");
                combo.Insert = new TableColumnQuery("projectemployee", "(SELECT CONCAT_WS(' ', firstname, lastname) FROM contact, employee WHERE contact.id=employee.contact_id AND projectemployee.employee_id=employee.ID) AS employee");
                combo.InputType = Web.Framework.Enums.EnumComboInputType.CHECKBOX;

                columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}