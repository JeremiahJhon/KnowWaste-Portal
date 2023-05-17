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

namespace kNOwaste.Models
{
    public class ModelDocuments : ModelBase
    {
        public string DocumentCategory { get; set; }

        public ModelDocuments() : base("documents", "Documents")
        {
            QuerySetting.SortColumn = "id";

            server.PageLimit = 6;

            ViewSetting.SHOWEDITOR = true;

            ControllerName = "LawPolicy";
            DocumentCategory = "1";
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);
            server.SelectFilter("documentcategory_id", DocumentCategory);
        }

        public virtual string ShowList(int limit = 0)
        {
            StringBuilder output = new StringBuilder();

            QuerySetting.QueryAll = false;
            var pagelimit = server.PageLimit;

            if (limit != 0)
                server.PageLimit = limit;

            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();


            //Use Table as default viewer
            if (Title != null)
                ViewSetting.Title = Title;
            else
                ViewSetting.Title = data.TableName;

            ViewSetting.Controller = ControllerName;
            ViewSetting.URL = url;

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            ViewSetting.Count = count;
            ViewSetting.PageLimit = server.PageLimit;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                var attachment = "";
                var icon = "";

                if (row["attachment"].ToString() != "")
                {
                    var document = row["attachment"].ToString();

                    if (document.Contains(".pdf"))
                        icon = "file-pdf-o";
                    else if (document.Contains(".doc"))
                        icon = "file-word-o";
                    else
                        icon = "file-o";

                    attachment = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='../Documents/{0}' target='_blank'><i class='fa fa-{1}'></i> View Document</a>", row["attachment"], icon);
                }

                if(row["datasource"].ToString() != "")
                {
                    var dsource = row["datasource"].ToString();

                    if (dsource.Contains(".htm"))
                        icon = "file-code-o";
                    else if (dsource.Contains(".pdf"))
                        icon = "file-pdf-o";
                    else
                        icon = "file-powerpoint-o";
                }

                if(row["attachment"].ToString() == "" && row["datasource"].ToString() == "")
                {
                    icon = "file-o";
                }

                var source = "";

                if (row["datasource"].ToString() != "")
                    source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                var thumbnail = "";

                if (row["thumbnail"].ToString() != "")
                    thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString());
                else
                    thumbnail = string.Format("<div class='doc-thumbnail'><i class='fa fa-{0}'></i></div>", icon);


                output.Append("<div class='doc-container c-margin-b-40'>");
                output.AppendFormat(thumbnail);

                output.Append("<div class='doc-item'>");
                output.AppendFormat("<h3 class='c-title c-fonts-uppercase c-font-bold c-font-22 c-font-dark'>{0}</h3>", row["title"]);
                output.AppendFormat("<p class='c-btn-uppercase c-font-14 c-font-thin c-theme-font'>{0} | {1}</p>", row["year"], row["publisher"]);
                output.AppendFormat("<p>{0}</p>", row["description"]);

                output.Append("<div class='doc-links'>");
                output.Append(attachment);
                output.Append(source);
                output.Append("</div>");

                output.Append("</div>");

                output.Append("</div>");
            }

            output.Append(ViewSetting.GeneratePager());

            QuerySetting.QueryAll = true;

            if (limit != 0)
                server.PageLimit = pagelimit;

            return output.ToString();
        }

        public virtual string ShowThumbnail(int limit = 0)
        {
            StringBuilder output = new StringBuilder();

            QuerySetting.QueryAll = false;
            var pagelimit = server.PageLimit;

            if (limit != 0)
                server.PageLimit = limit;

            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();


            //Use Table as default viewer
            if (Title != null)
                ViewSetting.Title = Title;
            else
                ViewSetting.Title = data.TableName;

            ViewSetting.Controller = ControllerName;
            ViewSetting.URL = url;

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            ViewSetting.Count = data.Rows.Count;
            ViewSetting.PageLimit = server.PageLimit;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                //var attachment = "";
                //var icon = "file-o";

                //if (row["attachment"].ToString() != "")
                //{
                //    var document = row["attachment"].ToString();

                //    if (document.Contains(".pdf"))
                //        icon = "file-pdf-o";
                //    else if (document.Contains(".doc"))
                //        icon = "file-word-o";
                //    else
                //        icon = "file-o";

                //    attachment = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='../Documents/{0}' target='_blank'><i class='fa fa-{1}'></i> View Document</a>", row["attachment"], icon);
                //}

                //var source = "";

                //if (row["datasource"].ToString() != "")
                //    source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                //var thumbnail = "";

                //if (row["thumbnail"].ToString() != "")
                //    thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString());
                //else
                //    thumbnail = string.Format("<div class='doc-thumbnail'><i class='fa fa-{0}'></i></div>", icon);


                output.Append("<div>");
                //output.AppendFormat(thumbnail);

                output.Append("<div class='doc-item'>");
                output.AppendFormat("<h3 class='c-title c-fonts-uppercase c-font-bold c-font-22 c-font-dark'><a href='../thematicarea?doc={1}'>{0}</a></h3>", row["title"], row["id"]);
                output.AppendFormat("<p class='doc-info c-btn-uppercase c-font-14 c-font-thin c-theme-font'>{0} | {1}</p>", row["year"], row["publisher"]);
                output.AppendFormat("<div class='doc-desc'>{0}</div>", row["description"]);

                output.Append("<div class='doc-links'>");
                //output.Append(attachment);
                //output.Append(source);
                output.Append("</div>");

                output.Append("</div>");
                output.Append("</div>");
            }

            output.Append(ViewSetting.GeneratePager());

            QuerySetting.QueryAll = true;

            if (limit != 0)
                server.PageLimit = pagelimit;

            return output.ToString();
        }

        public virtual string ShowDocument(int limit = 0)
        {
            StringBuilder output = new StringBuilder();

            QuerySetting.QueryAll = false;
            var pagelimit = server.PageLimit;

            if (limit != 0)
                server.PageLimit = limit;

            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();


            //Use Table as default viewer
            if (Title != null)
                ViewSetting.Title = Title;
            else
                ViewSetting.Title = data.TableName;

            ViewSetting.Controller = ControllerName;
            ViewSetting.URL = url;

            if (ViewSetting.GUID == null)
                ViewSetting.GUID = Guid.NewGuid().ToString();

            ViewSetting.Count = count;
            ViewSetting.PageLimit = server.PageLimit;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                var attachment = "";
                var icon = "file-o";

                if (row["attachment"].ToString() != "")
                {
                    var document = row["attachment"].ToString();

                    if (document.Contains(".pdf"))
                        icon = "file-pdf-o";
                    else if (document.Contains(".doc"))
                        icon = "file-word-o";
                    else
                        icon = "file-o";

                    attachment = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='../Documents/{0}' target='_blank'><i class='fa fa-{1}'></i> View Document</a>", row["attachment"], icon);
                }

                var source = "";

                if (row["datasource"].ToString() != "")
                    source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                var thumbnail = "";

                if (row["thumbnail"].ToString() != "")
                    thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString());
                else
                    thumbnail = string.Format("<div class='doc-thumbnail'><i class='fa fa-{0}'></i></div>", icon);


                output.Append("<div>");
                output.AppendFormat(thumbnail);

                output.Append("<div class='doc-item'>");
                output.AppendFormat("<h3 class='c-title c-fonts-uppercase c-font-bold c-font-22 c-font-dark'><a href='../thematicarea?doc={1}'>{0}</a></h3>", row["title"], row["id"]);
                output.AppendFormat("<p class='doc-info c-btn-uppercase c-font-14 c-font-thin c-theme-font'>{0} | {1}</p>", row["year"], row["publisher"]);
                output.AppendFormat("<div class='doc-desc'>{0}</div>", row["description"]);

                output.Append("<div class='doc-links'>");
                //output.Append(attachment);
                //output.Append(source);
                output.Append("</div>");

                output.Append("</div>");
                output.Append("</div>");
            }

            output.Append(ViewSetting.GeneratePager());

            QuerySetting.QueryAll = true;

            if (limit != 0)
                server.PageLimit = pagelimit;

            return output.ToString();
        }
    }
}