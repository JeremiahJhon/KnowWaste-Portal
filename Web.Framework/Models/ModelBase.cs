using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Controllers;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Property;
using Web.Framework.Server;

namespace Web.Framework.Models
{
    public class ModelBase
    {
        private static List<ModelBase> models = new List<ModelBase>();
        protected virtual PropertyTable Properties { get; }
        public BaseController Controller { get; set; }

        protected ServerBase server { get; set; }
        public string ControllerName { get; set; }

        public string Title { get; set; }
        public string ID { get; set; }
        public string Table { get; set; }

        public EnumViewType ViewType { get; set; }
        public QuerySettings QuerySetting { set; get; }
        public ViewSettings ViewSetting { get; set; }

        public StringBuilder Left { get; set; }
        public StringBuilder Right { get; set; }
        public StringBuilder Body { get; set; }

        public static byte[] EncryptionKey;
        public static byte[] EncryptionVector;

        public List<SearchFilter> Filters { get; set; }

        public ModelBase(string table, string title)
        {
            QuerySetting = new QuerySettings();
            ViewSetting = new ViewSettings();
            ViewType = EnumViewType.DEFAULT;

            ControllerName = table;
            server = new ServerBase(table);
            Filters = new List<SearchFilter>();

            Table = table;
            Title = title;

            Initialize();
        }

        public virtual void Initialize()
        {
            Left = new StringBuilder();
            Right = new StringBuilder();
            Body = new StringBuilder();
        }

        public virtual string Home()
        {
            return Show();
        }

        public virtual string Show()
        {
            DataTable data = GetData();
            int count = GetCount();
            string url = GenerateURL();

            UpdateBeforeShow(data);

            if (QuerySetting.QueryAll)
                count = data.Rows.Count;

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

            if (Controller != null && Controller.Username != null)
                ViewSetting.SHOWEDITOR = true;
            else
                ViewSetting.SHOWEDITOR = false;

            return ShowView(data);
        }

        public virtual string ShowRelation()
        {
            return "";
        }

        public virtual void ShowDetail()
        {
            if (ID != null)
            {
                ShowLeft();

                ServerBase server = new ServerBase("_relation");
                server.SelectColumn("tablerel");
                server.SelectColumn("name");
                server.SelectFilter("tablename", Table);
                server.SelectColumn("location");
                server.SelectOrder("sort", EnumOrder.ASCENDING);

                DataTable relation = server.SelectQuery();

                if (relation.Rows.Count != 0)
                {
                    foreach (DataRow row in relation.Rows)
                    {
                        string table = row[0].ToString();
                        string name = row[1].ToString();
                        int location = int.Parse(row[2].ToString());

                        ModelBase model = GenerateModel(table);

                        var parent = new ModelBase("", "");
                        parent.Table = Table;
                        parent.ID = ID;

                        model.ViewSetting.Parent = parent;
                        model.Controller = Controller;
                        model.Title = name;

                        switch (location)
                        {
                            case 1:
                                Left.Append(model.Show());
                                break;

                            case 2:
                                Body.Append(model.Show());
                                break;

                            case 3:
                                Right.Append(model.Show());
                                break;
                        }
                    }
                }
            }
        }

        public virtual string ShowNew()
        {
            ViewSetting.URL = GenerateInsertURL();

            HTMLForm form = new HTMLForm(ViewSetting);
            form.Title = ViewSetting.NewTitle;
            form.Bind(Properties);

            SetFormSize(form);

            return form.GenerateHTML();
        }

        public virtual void SetFormSize(HTMLForm form)
        {
        }

        public virtual string ShowEdit()
        {
            DataTable data = GetData();
            ViewSetting.URL = GenerateEditURL();

            HTMLForm form = new HTMLForm(ViewSetting);
            form.Title = ViewSetting.EditTitle;
            form.Bind(Properties, data);

            SetFormSize(form);

            return form.GenerateHTML();
        }

        public virtual string ShowDelete()
        {
            ViewSetting.URL = GenerateDeleteURL();

            HTMLForm form = new HTMLForm(ViewSetting);
            form.Title = ViewSetting.EditTitle;
            form.Message("Are you sure you want to delete the selected item?", EnumMessageIcon.WARNING);
            form.Height = 200;

            return form.GenerateHTML();
        }


        public virtual void ShowLeft()
        {
            ViewSetting.GUID = Guid.NewGuid().ToString();

            HTMLPanel panel = new HTMLPanel(ViewSetting);

            if (ID != null)
            {
                DataTable data = server.SelectByID(ID);
                data.Columns["id"].ExtendedProperties.Add("hide", true);
                data.Columns["deleted"].ExtendedProperties.Add("hide", true);

                foreach (DataColumn column in data.Columns)
                {
                    if (column.ColumnName.Contains("_id"))
                        column.ExtendedProperties.Add("hide", true);
                }

                panel.Title = "Details";

                StringBuilder output = new StringBuilder();

                foreach (DataRow row in data.Rows)
                {
                    if (ViewSetting.Title != null)
                        output.AppendFormat("<div class='detail-category'>{0}</div>", ViewSetting.Title);

                    output.Append("<div class='detail'>");

                    foreach (DataColumn column in data.Columns)
                    {
                        if (!column.ExtendedProperties.ContainsKey("hide"))
                        {
                            if (column.DataType == typeof(double))
                                output.AppendFormat("<div class='detail-item'><div class='label'>{0}:</div><div class='value'>{1}</div></div>", column.ColumnName, double.Parse(row[column.ColumnName].ToString()).ToString("N"));
                            else if (column.DataType == typeof(DateTime))
                                output.AppendFormat("<div class='detail-item'><div class='label'>{0}:</div><div class='value'>{1}</div></div>", column.ColumnName, DateTime.Parse(row[column.ColumnName].ToString()).ToString("MMMM dd, yyyy"));
                            else
                                output.AppendFormat("<div class='detail-item'><div class='label'>{0}:</div><div class='value'>{1}</div></div>", column.ColumnName, row[column.ColumnName].ToString());
                        }
                    }

                    output.Append("</div>");
                }

                panel.Body = output.ToString();
                Left.Append(panel.GenerateHTML());
            }
        }

        public virtual string ShowRight()
        {
            return "";
        }

        public virtual ServerAction Insert(HttpRequestBase Request)
        {
            if (Controller.UserID == null)
                return Redirect();

            PropertyTable properties = Properties;
            Dictionary<string, string> values = new Dictionary<string, string>();

            bool handle = false;
            string name;

            //Insert files

            var i = 0;

            foreach (string key in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[i];

                if (file.ContentLength > 0)
                {
                    foreach (PropertyColumn column in properties.Columns)
                    {
                        if (column.Name == key.ToLower())
                        {
                            name = Path.GetFileNameWithoutExtension(file.FileName);
                            var extension = Path.GetExtension(file.FileName);
                            var date = DateTime.Now.ToFileTime().ToString();

                            string uploadpath = ModelCommon.DocumentPath;

                            string fileName = name + "_" + date + extension;
                            var path = Path.Combine(uploadpath, fileName);
                            file.SaveAs(path);

                            //UploadImage(column.Name, fileName, path);

                            server.InsertValue(key, fileName);
                            handle = true;
                            break;
                        }
                    }
                }

                i++;
            }

            //Insert other fields other than files.

            foreach (PropertyColumn column in properties.Columns)
            {
                if (column.Type != EnumInputType.CHILD && column.Type != EnumInputType.IMAGE && column.Type != EnumInputType.FILE)
                {
                    string item = Request.Params[column.Name];
                    name = column.Name;

                    if (ValidateInsert(name, ref item))
                    {
                        handle = true;

                        switch (column.Type)
                        {
                            case EnumInputType.COMBOBOX:
                                name += "_id";
                                break;

                            case EnumInputType.DATE:
                                item = server.FormatDateTime(item);
                                break;

                            case EnumInputType.BOOLEAN:
                                if (item == null)
                                    item = "0";
                                else
                                    item = "1";

                                break;
                        }

                        values.Add(name, item);
                        server.InsertValue(name, item);
                    }
                }
            }

            if (handle)
            {
                UpdateBeforeInsert(values);

                server.UserID = Controller.UserID;
                server.Username = Controller.Username;
                server.InsertQuery();
            }

            return null;
        }

        public virtual ServerAction Update(HttpRequestBase Request)
        {
            if (Controller.UserID == null)
                return Redirect();

            PropertyTable properties = Properties;
            Dictionary<string, string> values = new Dictionary<string, string>();

            bool handle = false;
            string name;

            var i = 0;

            foreach (string key in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[i];

                if (file.ContentLength > 0)
                {
                    foreach (PropertyColumn column in properties.Columns)
                    {
                        if (column.Name == key.ToLower())
                        {
                            name = Path.GetFileNameWithoutExtension(file.FileName);
                            var extension = Path.GetExtension(file.FileName);
                            var date = DateTime.Now.ToFileTime().ToString();

                            string uploadpath = ModelCommon.DocumentPath;

                            string fileName = name + "_" + date + extension;
                            var path = Path.Combine(uploadpath, fileName);
                            file.SaveAs(path);

                            server.UpdateValue(key, fileName);
                            handle = true;
                            break;
                        }
                    }
                }

                i++;
            }

            foreach (PropertyColumn column in properties.Columns)
            {
                if (column.Type != EnumInputType.CHILD)
                {
                    string item = Request.Params[column.Name];
                    name = column.Name;

                    if (ValidateInsert(name, ref item))
                    {
                        handle = true;

                        switch (column.Type)
                        {
                            case EnumInputType.COMBOBOX:
                                name += "_id";
                                break;

                            case EnumInputType.DATE:
                                item = server.FormatDateTime(item);
                                break;

                            case EnumInputType.BOOLEAN:
                                if (item == null)
                                    item = "0";
                                else
                                    item = "1";

                                break;

                            case EnumInputType.FILE:
                                continue;

                        }

                        values.Add(name, item);
                        server.UpdateValue(name, item);
                    }
                }
            }

            if (handle)
            {
                server.UserID = Controller.UserID;
                server.Username = Controller.Username;

                server.UpdateQuery(ID);
            }

            return null;
        }

        protected virtual void UpdateBeforeInsert(Dictionary<string, string> values)
        {
        }

        public virtual ServerAction Hide(string value)
        {
            if (Controller.UserID == null)
                return Redirect();

            server.UserID = Controller.UserID;
            server.Username = Controller.Username;

            server.DeleteQuery(ID, value);

            return null;
        }

        public ServerAction Redirect()
        {
            var action = new ServerAction();
            action.Message = Controller.LoginURL();
            action.Action = EnumServerAction.REDIRECT;

            return action;
        }

        protected virtual bool ValidateInsert(string column, ref string value)
        {
            return true;
        }

        public PropertyTable QueryData()
        {
            DataTable data = GetData();
            return QueryModelData(data);
        }

        private PropertyTable QueryModelData(DataTable data)
        {
            PropertyTable properties = Properties;
            properties.Rows.Clear();

            foreach (DataRow row in data.Rows)
            {
                PropertyRow proprow = new PropertyRow();

                foreach (PropertyColumn column in properties.Columns)
                    proprow.Add(row[column.Name].ToString());

                properties.Rows.Add(proprow);
            }

            return properties;
        }


        protected virtual void GenerateQuery(QuerySettings settings)
        {
            //Filter by ID
            if (ID != null && ID != "")
            {
                server.SelectFilter("id", ID);
                server.SelectLimit(1);
            }

            //Filter by Parent Table
            else if (ViewSetting.Parent != null)
                server.SelectFilter(string.Format("{0}.{1}_id='{2}'", Table, ViewSetting.Parent.Table, ViewSetting.Parent.ID));

            //Include search queries
            if (ViewSetting.Search != null)
                server.SelectFilterRaw(ViewSetting.Search);

            foreach (SearchFilter filter in Filters)
                server.SelectFilter(filter.Column, filter.Value);

            if (!settings.QueryAll && (ID == null || ID == ""))
            {
                //Select Page
                server.SelectPage(ViewSetting.Page);
            }

            //Select Order
            server.SelectOrder(settings.SortColumn, settings.SortOrder);

            //Get model properties
            if (Properties != null)
                server.SelectColumn(Properties.Columns);
        }

        protected virtual void GenerateRelationQuery(QuerySettings settings)
        {
        }

        protected virtual string GenerateURL()
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("../{0}?", ControllerName);

            if (ViewSetting.Search != null)
                output.AppendFormat("&search={0}", ViewSetting.Search);

            if (ViewSetting.Parent != null)
                output.AppendFormat("&pn={0}&pid={1}", ViewSetting.Parent.Table, ViewSetting.Parent.ID);

            if (ViewSetting.Page != 1)
                output.AppendFormat("&p={0}", ViewSetting.Page);

            if (ViewSetting.QueryString != null && ViewSetting.QueryString != "")
                output.AppendFormat("&{0}", ViewSetting.QueryString);

            return output.ToString();
        }

        public virtual DataTable GetData(int limit = 0)
        {
            var pagelimit = server.PageLimit;

            if (limit != 0)
                server.PageLimit = limit;

            GenerateQuery(QuerySetting);

            if (limit != 0)
                server.PageLimit = pagelimit;

            return server.SelectQuery();
        }

        public virtual DataTable GetPage()
        {
            DataTable data = new DataTable();
            data.Columns.Add("Backward");
            data.Columns.Add("Start");
            data.Columns.Add("End");
            data.Columns.Add("Forward");
            data.Columns.Add("Page");
            data.Columns.Add("Total");

            int count = GetCount();

            HTMLPage page = new HTMLPage(count, ViewSetting.Page, "", server.PageLimit, "");

            int totalpage = page.GetCount();
            int start = page.GetStartPage();
            int end = page.GetEndPage();

            DataRow row = data.NewRow();

            if (ViewSetting.Page > 1)
                row[0] = ViewSetting.Page - 1;
            else
                row[0] = 1;

            row[1] = (start);
            row[2] = (end);

            if (ViewSetting.Page < totalpage)
                row[3] = (ViewSetting.Page + 1);
            else
                row[3] = (totalpage);

            row[4] = (ViewSetting.Page);
            row[5] = (totalpage);

            data.Rows.Add(row);
            return data;
        }

        protected virtual int GetCount()
        {
            var query = QuerySetting.QueryAll;

            QuerySetting.QueryAll = true;
            GenerateQuery(QuerySetting);

            QuerySetting.QueryAll = query;

            return server.SelectCountQuery();
        }

        protected virtual string ShowView(DataTable data)
        {
            switch (ViewType)
            {
                case EnumViewType.GRID:
                    return ShowGridView(data);

                case EnumViewType.BLOG:
                    return ShowBlogView(data);

                case EnumViewType.CUSTOM:
                    return ShowCustomView(data);
            }

            return ShowTableView(data);
        }

        public virtual string ShowTableView(DataTable data)
        {
            HTMLBase viewer = GetViewer(data);
            return viewer.GenerateHTML();
        }

        public virtual string ShowBlogView(DataTable data)
        {
            HTMLBase viewer = GetViewer(data);
            return viewer.GenerateHTML();
        }

        public virtual string ShowGridView(DataTable data)
        {
            HTMLBase viewer = GetViewer(data);
            return viewer.GenerateHTML();
        }

        public virtual string ShowCustomView(DataTable data)
        {
            HTMLBase viewer = GetViewer(data);
            return viewer.GenerateHTML();
        }

        protected virtual HTMLBase GetViewer(DataTable data)
        {
            HTMLTable table = new HTMLTable(data, ViewSetting);
            return table;
        }

        public void SelectFilter(string column, string value)
        {
            Filters.Add(new SearchFilter(){ Column = column, Value = value });
        }

        protected virtual void UpdateBeforeShow(DataTable data)
        {
            if (data.Columns.Contains("deleted"))
                data.Columns["deleted"].ExtendedProperties.Add("hide", true);

            var properties = Properties;

            if (properties != null && properties.HasChild)
            {
                foreach (PropertyColumn column in properties.Columns)
                {
                    if (column.Type == EnumInputType.CHILD)
                    {
                        data.Columns.Add(column.Name);

                        foreach (DataRow row in data.Rows)
                            row[column.Name] = column.FormatDisplay(Table, row["id"].ToString(), row, column.Name);
                    }
                }
            }
        }

        public static void Register(ModelBase model)
        {
            models.Add(model);
        }

        public static void UpdateModelView(BaseController controller)
        {
            foreach (ModelBase model in models)
            {
                model.Controller = controller;
                model.UpdateView();
            }
        }
    
        protected ModelBase GenerateModel(string table)
        {
            table = table.ToLower();

            foreach (ModelBase model in models)
            {
                if (model.Table == table)
                    return model;
            }

            ModelBase modelbase = new ModelBase(table, table);
            return modelbase;
        }

        protected string GenerateInsertURL()
        {
            StringBuilder url = new StringBuilder();
            url.AppendFormat("../{0}/insert?guid={1}&{2}", ControllerName, ViewSetting.GUID, ViewSetting.QueryString);

            if (ViewSetting.Parent != null)
                url.AppendFormat("&pn={0}&pid={1}", ViewSetting.Parent.Table, ViewSetting.Parent.ID);

            return url.ToString();
        }

        protected string GenerateEditURL()
        {
            StringBuilder url = new StringBuilder();
            url.AppendFormat("../{0}/update?guid={1}&id={2}", ControllerName, ViewSetting.GUID, ID);

            if (ViewSetting.Parent != null)
                url.AppendFormat("&pn={0}&pid={1}", ViewSetting.Parent.Table, ViewSetting.Parent.ID);

            if (ViewSetting.Page != 1)
                url.AppendFormat("&p={0}", ViewSetting.Page);

            return url.ToString();
        }

        protected string GenerateDeleteURL()
        {
            StringBuilder url = new StringBuilder();
            url.AppendFormat("../{0}/hide?guid={1}&id={2}&deleted=1", ControllerName, ViewSetting.GUID, ID);

            if (ViewSetting.Parent != null)
                url.AppendFormat("&pn={0}&pid={1}", ViewSetting.Parent.Table, ViewSetting.Parent.ID);

            if (ViewSetting.Page != 1)
                url.AppendFormat("&p={0}", ViewSetting.Page);

            return url.ToString();
        }

        public virtual void UpdateView()
        {
        }
    }
}