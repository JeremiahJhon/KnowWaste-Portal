using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Framework.HTML
{
    public class HTMLPage : HTMLBase
    {
        private int count;
        private int page;
        private string url;
        private string guid;

        public int Count { get { return count; } }

        public HTMLPage(int count, int page, string url, int size, string guid)
        {
            this.count = (int)Math.Ceiling(count / (float)size);
            this.page = page;
            this.url = url;
            this.guid = guid;
        }

        public int GetCount()
        {
            return count;
        }

        public int GetStartPage()
        {
            if (page > 3)
            {
                if (page + 3 < count)
                    return page - 2;
                else
                    return page - 4;
            }
            else
                return 1;
        }

        public int GetEndPage()
        {
            if (page + 2 < count)
            {
                if (count < 6)
                    return count;
                else
                {
                    if (page < 3)
                        return 5;
                    else
                        return page + 2;
                }
            }
            else
                return count;
        }

        public bool Visible
        {
            get
            {
                if (count > 1)
                    return true;
                else
                    return false;
            }
        }

        public override string GenerateHTML()
        {
            StringBuilder output = new StringBuilder();

            if (count > 1)
            {
                int start = GetStartPage();
                int end = GetEndPage();

                string pageurl = string.Format("{0}&guid={1}&partial=1&p=", url, guid);

                output.Append("<div class='page'><ul class='collapse inline'>");

                //First
                output.Append("<li>");

                if (page != 1)
                    output.AppendFormat("<li><a class='json' href='{0}1'><i class='fa fa-step-backward'></i></a></li>", pageurl);
                else
                    output.Append("<li><i class='fa fa-step-backward'></i></li>");

                //Previous
                if (page != 1)
                    output.AppendFormat("<li><a class='json' href='{0}{1}'><i class='fa fa-backward'></i></a></li>", pageurl, page - 1);
                else
                    output.Append("<li><i class='fa fa-backward'></i></li>");

                //Numbers
                for (int number = start; number <= end; number++)
                {
                    if (page == number)
                        output.AppendFormat("<li><span>{0}</span></li>", number.ToString());
                    else
                        output.AppendFormat("<li><a class='json' href='{0}{1}'>{1}</a></li>", pageurl, number);
                }

                //Next
                if (page != count)
                    output.AppendFormat("<li><a class='json' href='{0}{1}'><i class='fa fa-forward'></i></a></li>", pageurl, page + 1);
                else
                    output.Append("<li><i class='fa fa-forward'></i></li>");

                //Last
                if (page != count)
                    output.AppendFormat("<li><a class='json' href='{0}{1}'><i class='fa fa-step-forward'></i></a></li>", pageurl, count);
                else
                    output.Append("<li><i class='fa fa-step-forward'></i></li>");

                output.Append("</ul></div>");

            }

            return output.ToString();
        }
    }
}