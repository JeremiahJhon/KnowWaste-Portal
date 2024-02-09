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

            int page = 1;
            try { page = Convert.ToInt32(HttpUtility.ParseQueryString(url).Get("p")); } catch (Exception ex) { }

            if (page == 0) page = 1;
            
            //foreach (DataRow row in data.Rows)
            for (int ctr = pagelimit * (page - 1); ctr < (pagelimit * page) && ctr < count; ctr++)
            {
                DataRow row = data.Rows[ctr];
                var attachment = "";
                var icon = "";

                if (row["attachment"].ToString() != "")
                {
                    var document = row["attachment"].ToString();

                    if (document.Contains(".pdf"))
                        icon = "bi bi-file-earmark-pdf";
                    else if (document.Contains(".doc"))
                        icon = "bi bi-file-earmark-word";
                    else
                        icon = "bi bi-file-earmark";

                    attachment = string.Format("<a class='btn btn-light-success w-auto px-5' href='../Documents/{0}' target='_blank'><i class='pe-2 fs-2 {1}'></i> View Document</a>", row["attachment"], icon);
                }

                if(row["datasource"].ToString() != "")
                {
                    var dsource = row["datasource"].ToString();

                    if (dsource.Contains(".htm"))
                        icon = "bi bi-file-earmark-code";
                    else if (dsource.Contains(".pdf"))
                        icon = "bi bi-file-earmark-pdf";
                    else
                        icon = "bi bi-file-earmark-ppt";
                }

                if(row["attachment"].ToString() == "" && row["datasource"].ToString() == "")
                {
                    icon = "bi bi-file-earmark";
                }

                var source = "";

                if (row["datasource"].ToString() != "")
                    source = string.Format("<a class='btn btn-light-primary w-auto px-5'href='{0}' target='_blank'><i class='pe-2 fs-2 {1}'></i> View Source</a>", row["datasource"], icon);

                var thumbnail = "";

                if (row["thumbnail"].ToString() != "")
                    thumbnail = string.Format("<div class='col-md-2 mb-10'><img class='w-100 pe-5' src='../Documents/{0}'/></div>", row["thumbnail"].ToString());
                else
                    thumbnail = string.Format("<div class='col-md-2 mb-10'><i class='d-block text-center {0}' style='font-size: 100px;'></i></div>", icon);


                output.Append("<div class='row mb-10'>");
                output.AppendFormat(thumbnail);

                output.Append("<div class='col-md-10 mb-10'>");
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

            int page = 1;
            try { page = Convert.ToInt32(HttpUtility.ParseQueryString(url).Get("p")); } catch (Exception ex) { }

            if (page == 0) page = 1;

            //foreach (DataRow row in data.Rows)
            for (int ctr = pagelimit * (page - 1); ctr < (pagelimit * page) && ctr < count; ctr++)
            {
                DataRow row = data.Rows[ctr];
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


                output.Append("<div class='m-0'>");
                //output.AppendFormat(thumbnail);

                output.Append("<div class='doc-item'>");
                output.AppendFormat("<h4 class='fs-1 text-gray-800 w-bolder mb-1 pt-5'><a href='../thematicarea?doc={1}'>{0}</a></h4>", row["title"], row["id"]);
                output.AppendFormat("<span class='fw-semibold fs-4 text-gray-600 mb-1'>{0} | {1}</span>", row["year"], row["publisher"]);
                output.AppendFormat("<div class='fw-semibold fs-5 text-black mb-1'>{0}</div>", row["description"]);

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

            int page = 1;
            try { page = Convert.ToInt32(HttpUtility.ParseQueryString(url).Get("p")); } catch (Exception ex) { }

            if (page == 0) page = 1;

            //foreach (DataRow row in data.Rows)
            for (int ctr = pagelimit * (page - 1); ctr < (pagelimit * page) && ctr < count; ctr++)
            {
                DataRow row = data.Rows[ctr];
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