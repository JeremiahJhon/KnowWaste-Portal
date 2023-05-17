using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Framework.General
{
    public class CalendarItem
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }

        public string Color { get; set; }

        public CalendarItem(DateTime date, string text, string url, string icon, string color)
        {
            this.Date = date;
            this.Text = text;
            this.URL = url;
            this.Icon = icon;
            this.Color = color;
        }
    }
}