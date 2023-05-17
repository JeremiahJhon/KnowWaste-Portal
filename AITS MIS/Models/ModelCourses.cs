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
    public class ModelCourses : ModelBase
    {
        public string courseID { get; set; }

        public ModelCourses()
            : base("courses", "Courses")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "startdate";

            server.PageLimit = 5;

            ViewSetting.SHOWEDITOR = true;
            courseID = "1";
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);
        }

        public virtual string ShowList()
        {
            StringBuilder output = new StringBuilder();

            QuerySetting.QueryAll = false;

            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();

            DateTime start = DateTime.Now;

            //Use Table as default viewer
            if (Title != null)
                ViewSetting.Title = Title;
            else
                ViewSetting.Title = data.TableName;

            ViewSetting.URL = url;

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            ViewSetting.Count = count;
            ViewSetting.PageLimit = server.PageLimit;
            ViewSetting.Properties = Properties;

            output.Append("<div class='c-content-box c-size-md c-bg-white'>");
            output.Append("<div class='lcontainer'>");
            output.Append("<div class='row'>");

            output.AppendFormat("<div class='col-md-4'>");
            output.Append("</div>");

            output.AppendFormat("<div class='col-md-8'>");
            output.AppendFormat("<div class='c-name c-font-uppercase otherdocs'>E-Learning</div>");

            foreach (DataRow row in data.Rows)
            {
                if (row["startdate"].ToString() != "")
                {
                    start = DateTime.Parse(row["startdate"].ToString());
                }
                else
                {
                    start = DateTime.Parse(row["dateentered"].ToString());
                }


                if (row["Organizer"].ToString() != "RRCAP")
                {
                    output.Append("<div class='c-body margin-bottom-20'>");

                    var desc = row["description"].ToString();
                    if (desc.Length > 256)
                        desc = desc.Substring(0, 256) + "...";

                    output.AppendFormat("<h3 class='c-font-uppercase c-font-sbold'><a href='../Courses/Detail?id={0}'><i class='fa fa-university inline-block'></i> {1}</a></h3>", row["id"].ToString(), row["title"].ToString());
                    output.AppendFormat("<span class='c-font-lowercase c-font-blue-1'>{0}</span>", start.ToString("dd MMM yyyy"));
                    output.AppendFormat("<div class='c-desc'>{0}</div>", desc);
                    output.Append("</div>");
                }
            }

            output.Append("</div>");

            output.Append("</div>");
            output.Append("</div>");
            output.Append("</div>");

            return output.ToString();
        }

        public virtual string ShowCurriculum()
        {
            StringBuilder output = new StringBuilder();

            QuerySetting.QueryAll = false;

            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();

            DateTime start = DateTime.Now;

            //Use Table as default viewer
            if (Title != null)
                ViewSetting.Title = Title;
            else
                ViewSetting.Title = data.TableName;

            ViewSetting.URL = url;

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            ViewSetting.Count = count;
            ViewSetting.PageLimit = server.PageLimit;
            ViewSetting.Properties = Properties;

            output.Append("<div class='c-content-box c-size-md c-bg-white'>");
            output.Append("<div class='lcontainer'>");
            output.Append("<div class='row'>");

            foreach (DataRow row in data.Rows)
            {
                if (row["startdate"].ToString() != "")
                {
                    start = DateTime.Parse(row["startdate"].ToString());
                }
                else
                {
                    start = DateTime.Parse(row["dateentered"].ToString());
                }


                if (row["organizer"].ToString() == "RRCAP")
                {
                    output.AppendFormat("<div class='col-md-4'>");
                    if (row["Thumbnail"] != DBNull.Value)
                    {
                        output.AppendFormat("<img class='c-overlay-object img-responsive' src='../Documents/{0}'></img>", row["Thumbnail"].ToString());
                    }
                    else
                    {
                        output.AppendFormat("<img class='c-overlay-object img-responsive' src='../Documents/{0}'></img>", "elearning.jpg");
                    }
                    output.AppendFormat("</div>");

                    output.AppendFormat("<div class='col-md-8'>");
                    output.AppendFormat("<h3 class='c-font-uppercase c-font-sbold'><a href='http://www.rrcap.ait.asia/Pages/hwm.aspx' target='_blank'>{1}</a></h3>", row["id"].ToString(), row["title"].ToString());
                    output.Append("<div class='c-desc'>");
                    output.AppendFormat("{0}", row["description"].ToString());
                    output.Append("</div>");
                    output.AppendFormat("</div>");
                }
            }

            output.Append("</div>");
            output.Append("</div>");
            output.Append("</div>");
            return output.ToString();
        }


        public virtual string ShowDetail(string id)
        {
            courseID = id;

            StringBuilder output = new StringBuilder();

            server.SelectFilter("id", courseID);
            DataTable data = server.SelectQuery();

            foreach (DataRow row in data.Rows)
            {
                output.Append("<div class='c-content-blog-post-1'>");
                output.Append("<div class='c-media'>");
                output.Append("<div class='c-content-media-2-slider'>");
                output.Append("<div class='item'>");

                if (row["Thumbnail"].ToString() != "")
                {
                    output.AppendFormat("<div class='c-content-media-2' style='background-image: url(../Documents/{0})'>", row["Thumbnail"].ToString());
                }
                else
                {
                    output.AppendFormat("<div class='c-content-media-2' style='background-image: url(../Documents/{0})'>", "elearning.jpg");
                }
                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");

                output.Append("<div class='c-margin-t-20'></div>");

                output.Append("<div class='row'>");

                //Detail Information
                output.Append("<div class='col-md-4'>");
                output.Append("<div class='panel panel-default'>");
                output.Append("<div class='panel-heading c-title c-font-uppercase c-font-bold'>Detail Information</div>");

                output.Append("<div class='panel-body'>");
                output.Append("<div class='c-desc'>");

                if (row["coursenumber"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-info-circle'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Course Number : ", row["coursenumber"].ToString());
                    output.Append("</div>");
                }


                DateTime start = DateTime.Parse(row["startdate"].ToString());

                if (row["startdate"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-calendar'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Classes Start : ", start.ToString("dd MMM yyyy"));
                    output.Append("</div>");
                }

                if (row["duration"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-calendar'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Duration : ", row["duration"].ToString());
                    output.Append("</div>");
                }

                if (row["price"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-tag'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Price : ", row["price"].ToString());
                    output.Append("</div>");
                }

                if (row["level"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-certificate'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Level : ", row["level"].ToString());
                    output.Append("</div>");
                }

                if (row["languages"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-language'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Languages : ", row["languages"].ToString());
                    output.Append("</div>");
                }

                if (row["organizer"].ToString() != "")
                {
                    output.Append("<div class='c-author'>");
                    output.AppendFormat("<i class='fa fa-user'></i> <span class='c-font-uppercase c-font-bold'>{0}</span><span class='c-font-uppercase'>{1}</span>", "Provided By : ", row["organizer"].ToString());
                    output.Append("</div>");
                }

                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");

                output.Append("</div>");

                //Description
                output.Append("<div class='col-md-8'>");

                output.Append("<h3 class='c-header'>");
                output.AppendFormat("{0}", row["title"].ToString());
                output.Append("</h3>");

                output.Append("<div class='c-desc'>");
                output.AppendFormat("{0}", row["description"].ToString());
                output.Append("</div>");

                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");
            }

            return output.ToString();
        }
    }
}