using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Framework.HTML
{
    public class HTMLList : HTMLBase
    {
        private List<string> list;

        public string ID { get; set; }

        public HTMLList()
        {
            list = new List<string>();
            Class = "list";
        }

        public void Add(string item)
        {
            list.Add(item);
        }

        public void Add(string icon, string text, string link)
        {
            list.Add(string.Format("<div><a href='{2}'><i class='fa fa-{0}'></i> {1}</a></div>", icon, text, link));
        }

        public void Add(string icon, string text, string link, HTMLList submenu)
        {
            list.Add(string.Format("<div><i class='fa fa-{0}'></i><a href='{2}'>{1}</a>{3}</div>", icon, text, link, submenu.GenerateHTML()));
        }

        public void Add(string icon, string text, string link, string classname)
        {
            list.Add(string.Format("<div><i class='fa fa-{0}'></i><a class='{3}' href='{2}'>{1}</a></div>", icon, text, link, classname));
        }

        public override string GenerateHTML()
        {
            StringBuilder output = new StringBuilder();

            if (ID == null)
                output.AppendFormat("<div class='{0}'>", Class);
            else
                output.AppendFormat("<div id='{0}' class='{1}'>", ID, Class);

            foreach (string item in list)
            {
                output.Append(item);
            }

            output.Append("</div>");

            return output.ToString();
        }
    }
}