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
    public class ModelProjectEmployee : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelProjectEmployee() : base("projectemployee", "Employees")
        {
            QuerySetting.QueryAll = true;
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "employee";

            ViewSetting.NewTitle = "New Project Employee";
            ViewSetting.EditTitle = "Edit Project Employee";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyParent("project_id"));

                var combo = new PropertyComboBox("employee", "employee");
                combo.Select = new TableColumnQuery("employee", "(SELECT CONCAT_WS(' ', firstname, lastname) FROM contact WHERE contact.id=employee.contact_id) AS employee");
                combo.SortColumn = "employee";

                columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}