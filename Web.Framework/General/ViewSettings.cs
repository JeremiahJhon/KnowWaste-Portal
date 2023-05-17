using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;

namespace Web.Framework.General
{
    public class ViewSettings
    {
        public bool SHOWEDITOR { get; set; }
        public bool SHOWTABLEHEADER { get; set; }
        public bool SHOWCOLUMNHEADER { get; set; }
        public bool SHOWPAGE { get; set; }
        public bool SHOWSEARCH { get; set; }

        public ModelBase Parent { get; set; }

        public string Title { get; set; }
        public string Controller { get; set; }

        public string NewTitle { get; set; }
        public string EditTitle { get; set; }

        public string QueryString { get; set; }

        public string URL { get; set; }
        public string GUID { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public int PageLimit { get; set; }

        public string LinkColumn { get; set; }

        public string Search { get; set; }

        public HTMLBase Viewer { get; set; }

        public PropertyTable Properties { get; set; }

        public ViewSettings()
        {
            SHOWTABLEHEADER = true;
            SHOWCOLUMNHEADER = true;
            SHOWPAGE = true;
            SHOWEDITOR = false;
            SHOWSEARCH = true;
            Page = 1;
            LinkColumn = "id";
        }

        public string GeneratePager()
        {
            return new HTMLPage(Count, Page, URL, PageLimit, GUID).GenerateHTML();
        }
    }
}