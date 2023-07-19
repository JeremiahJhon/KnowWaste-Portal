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
using Web.Framework.Server;

namespace UCOnline.Models
{
    public class ModelThematicarea : ModelBase
    {
        public string DocumentCategory { get; set; }

        public string geothemeid { get; set; }

        public ModelThematicarea()
            : base("documents", "Documents")
        {
            server.PageLimit = 10;

            ViewSetting.SHOWEDITOR = true;

            ControllerName = "Thematicarea";
            geothemeid = "1";

        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);
            server.SelectFilter("geotheme_ID", geothemeid);
        }


        public virtual string ShowMenu()
        {
            var output = new StringBuilder();
            var server = new ServerBase("geotheme");

            DataTable data = server.SelectQuery();

            foreach (DataRow row in data.Rows)
            {

                if (geothemeid == row["id"].ToString())
                {
                    output.AppendFormat("<li class='selected'>");
                    output.AppendFormat("<i class='fa fa-check-square-o'></i> <a href='../Thematicarea?gid={0}'>{1}</a>", row["id"].ToString(), row["name"].ToString());
                }
                else
                {
                    output.AppendFormat("<li>");
                    output.AppendFormat("<i class='fa fa-square-o'></i> <a href='../Thematicarea?gid={0}'>{1}</a>", row["id"].ToString(), row["name"].ToString());
                }

                output.AppendFormat("</li>");
            }


            return output.ToString();
        }

        public virtual string ShowHeader()
        {
            var output = new StringBuilder();
            var server = new ServerBase("geotheme");

            DataTable data = server.SelectQuery();

            foreach (DataRow row in data.Rows)
            {
                if (geothemeid == row["id"].ToString())
                {
                    output.AppendFormat("<img src='../Content/Images/Logo/{0}'/> <span class='c-font-uppercase c-font-sbold'>{1}</span>", row["photo"].ToString().Split(';')[0], row["Name"].ToString());
                    //output.AppendFormat("<br/>");
                    //output.AppendFormat("<a class='thematic_link' href='{0}' target='_blank'>Lead Org./Int. - {1}</a>", row["referencelink"].ToString(), row["organization"].ToString());
                }
            }


            return output.ToString();
        }

        public virtual string ShowDescription()
        {
            var output = new StringBuilder();
            var server = new ServerBase("geotheme");

            DataTable data = server.SelectQuery();

            foreach (DataRow row in data.Rows)
            {
                if (geothemeid == row["id"].ToString())
                {
                    output.AppendFormat(row["Description"].ToString());
                    //output.AppendFormat("<br/>");
                    //output.AppendFormat("<a class='thematic_link' href='{0}' target='_blank'>Lead Org./Int. - {1}</a>", row["referencelink"].ToString(), row["organization"].ToString());
                }
            }


            return output.ToString();
        }
        public virtual string ShowDocument(string id)
        {
            StringBuilder output = new StringBuilder();
            DataTable data = server.SelectByID(id);

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

                if (row["datasource"].ToString() != "")
                {
                    var dsource = row["datasource"].ToString();

                    if (dsource.Contains(".htm"))
                        icon = "file-code-o";
                    else if (dsource.Contains(".pdf"))
                        icon = "file-pdf-o";
                    else
                        icon = "file-powerpoint-o";
                }

                if (row["attachment"].ToString() == "" && row["datasource"].ToString() == "")
                {
                    icon = "file-o";
                }

                var source = "";

                if (row["datasource"].ToString() != "")
                    source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                var thumbnail = "";

                if (row["thumbnail"].ToString() != "")
                    thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString().Split(';')[0]);
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

            output.Append("<div class='otherdocs'>Other Documents</div>");

            output.Append(new ModelReports().ShowThumbnail(10));

            return output.ToString();
        }

        public virtual string ShowListLawPolicy(string id, int limit = 0)
        {
            StringBuilder output = new StringBuilder();

            geothemeid = id;

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
            ViewSetting.PageLimit = 9;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                var attachment = "";
                var icon = "";

                if (row["documentcategory_ID"].ToString() == "1")
                {

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

                    if (row["datasource"].ToString() != "")
                    {
                        var dsource = row["datasource"].ToString();

                        if (dsource.Contains(".htm"))
                            icon = "file-code-o";
                        else if (dsource.Contains(".pdf"))
                            icon = "file-pdf-o";
                        else
                            icon = "file-powerpoint-o";
                    }

                    if (row["attachment"].ToString() == "" && row["datasource"].ToString() == "")
                    {
                        icon = "file-o";
                    }

                    var source = "";

                    if (row["datasource"].ToString() != "")
                        source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                    var thumbnail = "";

                    if (row["thumbnail"].ToString() != "")
                        thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString().Split(';')[0]);
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
            }

            QuerySetting.QueryAll = true;

            if (limit != 0)
                server.PageLimit = pagelimit;

            return output.ToString();
        }

        public virtual string ShowListCaseStudy(string id, int limit = 0)
        {
            StringBuilder output = new StringBuilder();

            geothemeid = id;

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
            ViewSetting.PageLimit = 3;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                var attachment = "";
                var icon = "";

                if (row["documentcategory_ID"].ToString() == "2")
                {

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

                    if (row["datasource"].ToString() != "")
                    {
                        var dsource = row["datasource"].ToString();

                        if (dsource.Contains(".htm"))
                            icon = "file-code-o";
                        else if (dsource.Contains(".pdf"))
                            icon = "file-pdf-o";
                        else
                            icon = "file-powerpoint-o";
                    }

                    if (row["attachment"].ToString() == "" && row["datasource"].ToString() == "")
                    {
                        icon = "file-o";
                    }

                    var source = "";

                    if (row["datasource"].ToString() != "")
                        source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                    var thumbnail = "";

                    if (row["thumbnail"].ToString() != "")
                        thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString().Split(';')[0]);
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
            }

            //output.AppendFormat("<a href='../Thematicarea' class='btn btn-sm c-theme-btn c-btn-square c-btn-bold c-align-right'>More</a>");
            //output.Append(ViewSetting.GeneratePager());

            QuerySetting.QueryAll = true;

            if (limit != 0)
                server.PageLimit = pagelimit;


            return output.ToString();
        }

        public virtual string ShowListReports(string id, int limit = 0)
        {
            StringBuilder output = new StringBuilder();

            geothemeid = id;

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
            ViewSetting.PageLimit = 3;
            ViewSetting.Properties = Properties;

            foreach (DataRow row in data.Rows)
            {
                var attachment = "";
                var icon = "";

                if (row["documentcategory_ID"].ToString() == "3")
                {

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

                    if (row["datasource"].ToString() != "")
                    {
                        var dsource = row["datasource"].ToString();

                        if (dsource.Contains(".htm"))
                            icon = "file-code-o";
                        else if (dsource.Contains(".pdf"))
                            icon = "file-pdf-o";
                        else
                            icon = "file-powerpoint-o";
                    }

                    if (row["attachment"].ToString() == "" && row["datasource"].ToString() == "")
                    {
                        icon = "file-o";
                    }

                    var source = "";

                    if (row["datasource"].ToString() != "")
                        source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                    var thumbnail = "";

                    if (row["thumbnail"].ToString() != "")
                        thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString().Split(';')[0]);
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
            }

            //output.AppendFormat("<a href='../Thematicarea' class='btn btn-sm c-theme-btn c-btn-square c-btn-bold c-align-right'>More</a>");
            //output.Append(ViewSetting.GeneratePager());

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

                var source = "";

                if (row["datasource"].ToString() != "")
                    source = string.Format("<a class='btn btn-special c-btn-uppercase btn-md c-btn-bold c-btn-square c-bg-blue-1 c-theme-btn wow animate fadeIn' style='visibility: visible; animation-name: fadeIn;' href='{0}' target='_blank'><i class='fa fa-external-link'></i> View Source</a>", row["datasource"]);

                var thumbnail = "";

                if (row["thumbnail"].ToString() != "")
                    thumbnail = string.Format("<div class='doc-thumbnail'><img src='../Documents/{0}'/></div>", row["thumbnail"].ToString().Split(';')[0]);
                else
                    thumbnail = string.Format("<div class='doc-thumbnail'><i class='fa fa-{0}'></i></div>", icon);


                output.Append("<div class='col-md-4 doc-container c-margin-b-40'>");
                output.AppendFormat(thumbnail);

                output.Append("<div class='doc-item'>");
                output.AppendFormat("<h3 class='c-title c-fonts-uppercase c-font-bold c-font-22 c-font-dark'>{0}</h3>", row["title"]);
                output.AppendFormat("<p class='doc-info c-btn-uppercase c-font-14 c-font-thin c-theme-font'>{0} | {1}</p>", row["year"], row["publisher"]);
                output.AppendFormat("<div class='doc-desc'>{0}</div>", row["description"]);

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

        public virtual string ShowRelatedEvents(string id, int limit = 0)
        {
            geothemeid = id;

            ServerBase server1 = new ServerBase("events");
            server1.SelectFilter("geotheme_id", geothemeid);
            DataTable dataevents = server1.SelectQuery();

            StringBuilder output = new StringBuilder();

            output.Append("<div class='container'>");

            foreach (DataRow row in dataevents.Rows)
            {
                output.Append("<div class='row c-margin-b-10'>");
                output.Append("<div class='c-content-product-2 c-bg-white'>");
                output.Append("<div class='col-md-4'>");
                output.Append("<div class='c-content-overlay'>");

                if (row["Thumbnail"].ToString() == "")
                {
                    output.AppendFormat("<div class='c-bg-img-center c-overlay-object' data-height='height' style='height: 240px; background-image: url(../Documents/default.png);'>");
                    output.Append("</div>");
                }
                else
                {
                    output.AppendFormat("<div class='c-bg-img-center c-overlay-object' data-height='height' style='height: 240px; background-image: url(../Documents/{0});'>", row["thumbnail"].ToString());
                    output.Append("</div>");
                }

                output.Append("</div>");
                output.Append("</div>");
                output.Append("<div class='col-md-8'>");
                output.Append("<div class='c-info-list'>");
                output.Append("<h3 class='c-title c-font-bold c-font-22 c-font-dark'>");
                output.AppendFormat("<a class='c-theme-link' href='../Events/Detail?id={0}'>{1}</a>", row["id"].ToString(), row["name"].ToString());
                output.Append("</h3>");
                output.AppendFormat("<p class='c-order-date c-font-14 c-font-thin c-theme-font'>{0} to {1}</p>", row["datestart"].ToString(), row["dateend"].ToString());
                output.AppendFormat("<p class='c-order-date c-font-14 c-font-thin c-theme-font'>{0}</p>", row["location"].ToString());
                output.AppendFormat("<p>{0}</p>", row["description"].ToString());
                output.AppendFormat("<a href='../Events/Detail?{0}' class='btn c-btn-uppercase btn-md c-btn-bold c-btn-square c-theme-btn wow animate fadeIn'>Detail</a>", row["id"].ToString());

                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");
                output.Append("</div>");
            }

            output.Append("</div>");

            return output.ToString();
        }

        //public virtual string ShowExperts(string id)
        //{
        //    StringBuilder output = new StringBuilder();

        //    geothemeid = id;

        //    DataTable data = GetData();
        //    int count = GetCount();
        //    string url = GenerateURL();

        //    ViewSetting.Controller = ControllerName;
        //    ViewSetting.URL = url;

        //    if (ViewSetting.GUID == null)
        //        ViewSetting.GUID = Guid.NewGuid().ToString();

        //    ViewSetting.Properties = Properties;



        //    return output.ToString();
        //}
    }
}