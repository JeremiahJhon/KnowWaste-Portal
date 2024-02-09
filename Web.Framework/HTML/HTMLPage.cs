using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

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

                output.Append("<ul class=\"pagination\">");

                //Previous
                output.AppendFormat("<li class=\"page-item previous disabled\"><a href=\"#\" class=\"page-link\"><i class=\"previous\"></i></a></li>");


                //Numbers
                for (int number = start; number <= end; number++)
                {
                    if (page == number)
                        output.AppendFormat("<li class=\"page-item active\"><a href=\"#\" class=\"page-link\">{0}</a></li>", number.ToString());
                    else
                        output.AppendFormat("<li class=\"page-item \"><a href=\"{0}{1}\" class=\"page-link\">{1}</a></li>", pageurl, number);
                }

                //Next
                if (page != count)
                    output.AppendFormat("<li class=\"page - item next\"><a href=\"#\"  class=\"page-link\"><i class=\"next\"></i></a></li>");

            }

            return output.ToString();
        }
    }
}