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
using Web.Framework.Server;

namespace UCOnline.Models
{
    public class ModelForums : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public string geothemeid { get; set; }

        public ModelForums() : base("forums", "Forums")
        {
            QuerySetting.SortColumn = "id";

            server.PageLimit = 5;

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "name";

            ViewType = Web.Framework.Enums.EnumViewType.CUSTOM;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("name"));
                columns.Add(new PropertyText("description"));

                var combo = new PropertyComboBox("geotheme", "theme");
                combo.Select = new TableColumnQuery("geotheme", "name");
                combo.SortColumn = "name";
                columns.Add(combo);

                combo = new PropertyComboBox("forumcontact", "contact");
                combo.Select = new TableColumnQuery("forumcontact", "name");
                combo.SortColumn = "name";
                columns.Add(combo);

                combo = new PropertyComboBox("thumbnail", "thumbnail");
                combo.Select = new TableColumnQuery("forumcontact", "thumbnail");
                combo.SortColumn = "thumbnail";
                columns.Add(combo);

                properties.Columns = columns;
            }
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);

            if (geothemeid != null)
                server.SelectFilter("geotheme_id", geothemeid);
        }

        public override string ShowCustomView(DataTable data)
        {
            var output = new StringBuilder();

            foreach (DataRow row in data.Rows)
            {
                output.Append("<div class='panel panel - default'>");
                output.Append("<div class='c-shop-product-details-2 c-opt-1'>");
                output.Append("<div class='row'>");

                output.Append("<div class='col-md-3'>");
                output.AppendFormat("<img class='image-circle c-overlay-object img-responsive' src='../Documents/{0}' alt=''>", row["thumbnail"]);
                output.Append("</div>");

                output.Append("<div class='col-md-9'>");
                output.AppendFormat("<h3 class='c-title c-font-bold c-font-22 c-font-dark'>{0}</h3>", row["name"]);
                output.AppendFormat("<p class='c-font-14 c-font-dark'>{0}</p", row["geotheme"]);
                output.AppendFormat("<p class='c-order-date c-font-14 c-font-thin c-theme-font'>{0}</p>", row["description"]);
                output.Append("</div>");

                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");
            }

            output.Append(new HTMLPage(ViewSetting.Count, ViewSetting.Page, "../Forums", ViewSetting.PageLimit, ViewSetting.GUID).GenerateHTML());

            return output.ToString();
        }

        public virtual string ShowMenu()
        {
            var output = new StringBuilder();
            var server = new ServerBase("geotheme");

            DataTable data = server.SelectQuery();

            if (geothemeid == null)
            {
                output.Append("<li class='selected'>");
                output.Append("<a href='../Forums'><i class='fa fa-check-square-o'></i> All</a>");
            }
            else
            {
                output.Append("<li>");
                output.Append("<a href='../Forums'><i class='fa fa-square-o'></i> All</a>");
            }

            output.Append("</li>");

            foreach (DataRow row in data.Rows)
            {
                if (geothemeid == row["id"].ToString())
                {
                    output.Append("<li class='selected'>");
                    output.AppendFormat("<a href='../Forums?gid={0}'><i class='fa fa-check-square-o'></i> {1}</a>", row["id"].ToString(), row["name"].ToString());
                }
                else
                {
                    output.Append("<li>");
                    output.AppendFormat("<a href='../Forums?gid={0}'><i class='fa fa-square-o'></i> {1}</a>", row["id"].ToString(), row["name"].ToString());
                }

                output.AppendFormat("</li>");
            }

            return output.ToString();
        }
    }
}