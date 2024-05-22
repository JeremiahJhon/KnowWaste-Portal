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
    public class ModelUpcomingEvents : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelUpcomingEvents() : base("UpcomingEvents", "UpcomingEvents")
        {
            QuerySetting.SortColumn = "id";
            ViewSetting.SHOWEDITOR = true;

            server.PageLimit = 5;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("title"));
                columns.Add(new PropertyText("description"));
                columns.Add(new PropertyDate("UpcomingEventsdate"));

                //var combo = new PropertyComboBox("client");
                //combo.Select = new TableColumnQuery("client", "name");

                //columns.Add(combo);
                    
                properties.Columns = columns;
            }
        }

        public string Tweets()
        {
            StringBuilder output = new StringBuilder();

            output.AppendFormat("<a class='twitter-timeline' href='{0}'>Tweets by unep_ietc</a> <script async src='{1}' style='charset:utf-8;'></script>", "https://twitter.com/unep_ietc", "//platform.twitter.com/widgets.js");

            return output.ToString();

        }
    }
}