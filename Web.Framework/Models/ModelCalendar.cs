using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Server;

namespace Web.Framework.Models
{
    public class ModelCalendar : ModelBase
    {
        private int start;

        private List<CalendarItem> items;
        private Dictionary<int, CalendarDay> special;

        public string Month { get; set; }
        public string Year { get; set; }
        public string Day { get; set; }
        public bool FullCalendar { get; set; }

        public bool IncludeDefault { get; set; }

        public ModelCalendar(string table, string title) : base(table, title)
        {
            FullCalendar = true;
            ControllerName = "Calendar";

            items = new List<CalendarItem>();
            special = new Dictionary<int, CalendarDay>();
        }

        public void Add(DateTime date, string text, string url, string icon, string color)
        {
            items.Add(new CalendarItem(date, text, url, icon, color));
        }

        public override string Show()
        {
            items = new List<CalendarItem>();

            if (Year == null)
                Year = DateTime.Now.Year.ToString();

            if (Month == null)
                Month = DateTime.Now.Month.ToString();

            if (Day == null)
                Day = DateTime.Now.Day.ToString();

            int year = int.Parse(Year);
            int month = int.Parse(Month);
            int day = int.Parse(Day);

            if (day > DateTime.DaysInMonth(year, month))
                day = DateTime.DaysInMonth(year, month);

            DateTime date = new DateTime(year, month, day);
            int totaldays = DateTime.DaysInMonth(date.Year, date.Month);

            DateTime datestart = new DateTime(year, month, 1);
            DateTime dateend = new DateTime(year, month, totaldays);

            //Add custom calendar
            AddCustomCalendar(datestart, dateend);


            DataTable data = GenerateMonthData(out date);
            data.TableName = date.ToString("MMMM yyyy");

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            string url = GenerateURL();

            StringBuilder header = new StringBuilder();
            StringBuilder body = new StringBuilder();

            string nexturl;
            string previousurl;

            int currentmonth = int.Parse(Month);
            int currentyear = int.Parse(Year);

            if (currentmonth < 12)
                nexturl = string.Format("{3}&m={0}&d={1}&y={2}&guid={4}", (currentmonth + 1), Day, Year, url, ViewSetting.GUID);
            else
                nexturl = string.Format("{3}&m={0}&d={1}&y={2}&guid={4}", 1, Day, (currentyear + 1), url, ViewSetting.GUID);

            if (currentmonth > 1)
                previousurl = string.Format("{3}&m={0}&d={1}&y={2}&guid={4}", (currentmonth - 1), Day, Year, url, ViewSetting.GUID);
            else
                previousurl = string.Format("{3}&m={0}&d={1}&y={2}&guid={4}", 12, Day, (currentyear - 1), url, ViewSetting.GUID);

            body.Append("<table class='calendar-table'>");

            //Header
            body.Append("<tr>");

            foreach (DataColumn column in data.Columns)
            {
                if (!column.ReadOnly)
                    body.AppendFormat("<th>{0}</th>", column.ColumnName);
            }

            body.Append("</tr>");

            int counter = 1;

            foreach (DataRow row in data.Rows)
            {
                body.Append("<tr class='table-row'>");

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    day = counter - start;

                    if (day > 0 && special.ContainsKey(day))
                        body.AppendFormat("<td class='{1}'>{0}<i class='calendar-editor fa fa-plus'></i></td>", row[i], special[day].ToString().ToLower());
                    else
                        body.AppendFormat("<td>{0}<i class='calendar-editor fa fa-plus'></i></td>", row[i]);

                    counter++;
                }

                body.Append("</tr>");

            }

            body.Append("</table>");

            HTMLPanel panel = new HTMLPanel(ViewSetting);
            panel.Class = "panel calendar";

            panel.Title = data.TableName;

            panel.AddButton(string.Format("<a href='{0}' class='calendar-backward calendar-button'><i class='fa fa-backward'></i></a>", previousurl));
            panel.AddButton(string.Format("<a href='{0}' class='calendar-forward calendar-button'><i class='fa fa-forward'></i></a>", nexturl));
            panel.AddButton(string.Format("<a href='{0}' class='panel-fullscreen calendar-button'><i class='fa fa-arrows-alt'></i></a>", ViewSetting.GUID));

            panel.Body = body.ToString();

            return panel.GenerateHTML();
        }

        private DataTable GenerateMonthData(out DateTime date)
        {
            DateTime datestart = new DateTime(int.Parse(Year), int.Parse(Month), 1);
            DateTime dateend = new DateTime(datestart.Year, datestart.Month, DateTime.DaysInMonth(datestart.Year, datestart.Month));

            if (IncludeDefault)
            {
                UnionBase calserver = new UnionBase("calendar");

                string filter = string.Format("(datestart<='{0}' AND dateend>='{0}') OR (datestart>='{0}' AND dateend<='{1}') OR (datestart<='{1}' AND dateend>='{1}')", server.FormatDateTime(datestart), server.FormatDateTime(dateend));
                calserver.SelectFilter(filter);

                DataTable data = calserver.SelectQuery();
                return FormatCalendar(out date, data);
            }
            else
            {
                string filter = string.Format("(datestart<='{0}' AND dateend>='{0}') OR (datestart>='{0}' AND dateend<='{1}') OR (datestart<='{1}' AND dateend>='{1}')", server.FormatDateTime(datestart), server.FormatDateTime(dateend));

                ServerBase calserver = new ServerBase("holiday");
                calserver.SelectFilter(filter);

                DataTable data = calserver.SelectQuery();
                data.Columns.Add("tablename");

                foreach (DataRow row in data.Rows)
                    row["tablename"] = "holiday";

                return FormatCalendar(out date, data);
            }
        }

        private DataTable FormatCalendar(out DateTime date, DataTable data)
        {
            DataTable table = new DataTable();

            if (FullCalendar)
            {
                table.Columns.Add("Sun");
                table.Columns.Add("Mon");
                table.Columns.Add("Tue");
                table.Columns.Add("Wed");
                table.Columns.Add("Thu");
                table.Columns.Add("Fri");
                table.Columns.Add("Sat");
            }
            else
            {
                table.Columns.Add("SU");
                table.Columns.Add("MO");
                table.Columns.Add("TU");
                table.Columns.Add("WE");
                table.Columns.Add("TH");
                table.Columns.Add("FR");
                table.Columns.Add("SA");
            }

            if (Month == null || Year == null)
                date = DateTime.Now;
            else
            {
                int m = int.Parse(Month);
                int y = int.Parse(Year);

                date = new DateTime(y, m, 1);
            }

            int currentday = DateTime.Now.Day;
            int currentmonth = DateTime.Now.Month;
            int currentyear = DateTime.Now.Year;

            //Get the first day of the month
            date = new DateTime(date.Year, date.Month, 1);
            int calmonth = date.Month;
            int calyear = date.Year;

            //Get the total number of days in a month
            int totaldays = DateTime.DaysInMonth(date.Year, date.Month);

            //Get the days of the week
            DayOfWeek day = date.DayOfWeek;
            int counter = (int)day;

            start = counter;

            if (Month == null || Year == null)
                date = DateTime.Now;
            else
            {
                int m = int.Parse(Month);
                int y = int.Parse(Year);
                int d = currentday;

                if (this.Day != null)
                    d = int.Parse(this.Day);

                if (d > DateTime.DaysInMonth(y, m))
                    d = DateTime.DaysInMonth(y, m);

                date = new DateTime(y, m, d);
            }

            DataRow row = table.NewRow();
            DataTable color = new ServerBase("calendar").SelectAll();


            for (int i = 1; i <= totaldays; i++)
            {
                StringBuilder task = new StringBuilder();

                //Add calendar events
                foreach (DataRow rowcal in data.Rows)
                {
                    DateTime caldate1 = (DateTime)rowcal["datestart"];
                    DateTime caldate2 = (DateTime)rowcal["dateend"];

                    if ((caldate1.Day == i) || (caldate2.Month > date.Month) || (caldate1.Day <= i && caldate2.Day >= i))
                        if (FullCalendar)
                        {
                            string tablename = rowcal["tablename"].ToString().ToLower();
                            string colorname = "";
                            string icon = "";

                            foreach (DataRow calendarstyle in color.Rows)
                            {
                                if (calendarstyle["tablename"].ToString().ToLower() == tablename)
                                {
                                    colorname = calendarstyle["color"].ToString().ToLower();
                                    icon = calendarstyle["icon"].ToString();
                                    break;
                                }
                            }

                            if (tablename == "holiday")
                            {
                                task.AppendFormat("<div class='calendar-item calendar-color-{3}' ><a href='../{2}/Detail?id={0}&datestart={5}'><i class='{4}'></i>{1}</a></div>", rowcal["id"].ToString(), rowcal["name"].ToString(), tablename, colorname, icon, server.FormatDateTime(caldate1));
                                if (!special.ContainsKey(i))
                                    special.Add(i, CalendarDay.HOLIDAY);
                            }
                            else
                                task.AppendFormat("<div class='calendar-item calendar-color-{3}' ><a href='../{2}/Detail?id={0}&datestart={5}'><i class='{4}'></i>{1}</a></div>", rowcal["id"].ToString(), rowcal["name"].ToString(), tablename, colorname, icon, server.FormatDateTime(caldate1));
                        }
                        else
                            task.Append(i);
                }

                int itemcount = 0;

                foreach (CalendarItem item in items)
                {
                    if ((item.Date.Day == i) || (item.Date.Month > date.Month) || (item.Date.Day <= i && item.Date.Day >= i))
                    {
                        itemcount++;

                        if (FullCalendar)
                            task.AppendFormat("<a class='calendar-link calendar-item-{4}' href='{1}'>{0}</a>", item.Text, item.URL, item.Icon, item.Color, itemcount);
                        else
                            task.Append(i);
                    }
                }

                //Add day to the calendar
                if (FullCalendar)
                {
                    if (i == currentday && calmonth == currentmonth && calyear == currentyear && !special.ContainsKey(i))
                    {
                        row[counter] = string.Format("<span class='calendar-today'>{0}</span>", i) + task.ToString();
                        special.Add(i, CalendarDay.TODAY);
                    }
                    else
                        row[counter] = string.Format("<span>{0}</span>", i) + task.ToString();
                }
                else
                {
                    if (task.Length != 0)
                        row[counter] = string.Format("<span class='calendar-event'>{0}</span>", i);

                    else if (i == currentday && calmonth == currentmonth && calyear == currentyear && !special.ContainsKey(i))
                    {
                        row[counter] = string.Format("<span class='calendar-today'>{0}</span>", i);
                        special.Add(i, CalendarDay.TODAY);
                    }
                    else
                        row[counter] = i;
                }

                //Add new week
                if (counter == 6 && i != totaldays)
                {
                    table.Rows.Add(row);
                    row = table.NewRow();

                    counter = -1;
                }
                counter++;
            }

            table.Rows.Add(row);
            return table;
        }

        protected virtual void AddCustomCalendar(DateTime startdate, DateTime enddate)
        {
        }
    }
}