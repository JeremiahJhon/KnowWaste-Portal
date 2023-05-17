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
    public class ModelBlogs : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelBlogs() : base("blogs", "Blogs")
        {
            QuerySetting.SortColumn = "id";

            server.PageLimit = 5;

            ViewSetting.SHOWEDITOR = true;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("thumbnail"));
                columns.Add(new PropertyText("title"));
                columns.Add(new PropertyText("description"));
                columns.Add(new PropertyText("author"));
                columns.Add(new PropertyText("blogsdate"));

                properties.Columns = columns;
            }
        }
    }
}