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
    public class ModelSeminars : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelSeminars() : base("seminars", "Seminars")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";

            server.PageLimit = 5;
            ViewSetting.SHOWEDITOR = true;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();                
                columns.Add(new PropertyText("title"));
                columns.Add(new PropertyText("description"));
                columns.Add(new PropertyText("location"));
                columns.Add(new PropertyText("datestart"));
                columns.Add(new PropertyText("dateend"));

                //var combo = new PropertyComboBox("client");
                //combo.Select = new TableColumnQuery("client", "name");

                //columns.Add(combo);

                properties.Columns = columns;
            }
        }
    }
}