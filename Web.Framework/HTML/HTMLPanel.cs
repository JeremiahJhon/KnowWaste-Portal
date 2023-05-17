using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.General;

namespace Web.Framework.HTML
{
    public class HTMLPanel : HTMLBase
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }

        private ViewSettings settings { get; set; }
        private List<string> items;

        public HTMLPanel(ViewSettings settings)
        {
            this.settings = settings;
            this.items = new List<string>();

            Class = "panel";
        }

        public void AddButton(string item)
        {
            this.items.Add(item);
        }

        public override string GenerateHTML()
        {
            StringBuilder output = new StringBuilder();

            if (settings.GUID != null)
                output.AppendFormat("<div id='{0}' class='{1}'>", settings.GUID, Class);
            else
                output.AppendFormat("<div class='{0}'>", Class);

            //Header
            if (Title != null)
            {
                output.Append("<div class='panel-header'>");
                output.AppendFormat("<h3>{0}</h3>", Title);

                output.Append("<div class='panel-toolbar'><ul>");

                foreach (string item in items)
                    output.AppendFormat("<li>{0}</li>", item);

                output.Append("</ul></div>");
                output.Append("</div>");
            }

            //Content
            if (Body != null)
            {
                output.Append("<div class='panel-body'>");
                output.Append(Body);
                output.Append("</div>");
            }


            //Footer
            if (Footer != null)
            {
                output.Append("<div class='panel-footer'>");
                output.Append(Footer);
                output.Append("</div>");
            }

            output.Append("</div>");

            return output.ToString();
        }
    }
}