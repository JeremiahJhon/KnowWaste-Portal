using kNOwaste.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;

namespace UCOnline.Models
{
    public class ModelLawPolicy : ModelDocuments
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelLawPolicy() : base()
        {
            ControllerName = "LawPolicy";
            DocumentCategory = "1";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("title"));                
                columns.Add(new PropertyText("description"));
                columns.Add(new PropertyText("datasource"));
                columns.Add(new PropertyText("thumbnail"));
                columns.Add(new PropertyText("year"));
                columns.Add(new PropertyText("publisher"));
                properties.Columns = columns;
            }
        }
    }
}