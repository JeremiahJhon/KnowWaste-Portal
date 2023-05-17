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
    public class ModelBlogsComment : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelBlogsComment() : base("blogscomment", "Blogs Comment")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";

            server.PageLimit = 5;

            ViewSetting.SHOWEDITOR = true;
            //ViewSetting.LinkColumn = "code";

            //ViewSetting.NewTitle = "New Project";
            //ViewSetting.EditTitle = "Edit Project";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("comment"));
                columns.Add(new PropertyText("Dateentered"));

                var combo = new PropertyComboBox("blogs");
                combo.Select = new TableColumnQuery("blogs", "title");

                var combocontact = new PropertyComboBox("blogscontact");
                combocontact.Select = new TableColumnQuery("blogscontact", "name");

                var combophoto = new PropertyComboBox("contactphoto");
                combophoto.Select = new TableColumnQuery("blogscontact", "thumbnail");

                columns.Add(combo);
                columns.Add(combocontact);
                columns.Add(combophoto);

                properties.Columns = columns;
            }
        }
    }
}