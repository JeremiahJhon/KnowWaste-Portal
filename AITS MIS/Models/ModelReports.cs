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
    public class ModelReports : ModelDocuments
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelReports() : base()
        {
            ControllerName = "Reports";
            DocumentCategory = "3";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("title"));
                columns.Add(new PropertyText("Thumbnail"));
                columns.Add(new PropertyText("year"));
                columns.Add(new PropertyText("publisher"));
                columns.Add(new PropertyText("keyword"));
                columns.Add(new PropertyText("Description"));
                columns.Add(new PropertyText("Datasource"));
                columns.Add(new PropertyText("Attachment"));
               
                properties.Columns = columns;
            }
        }
    }
}