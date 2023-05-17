using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.Property;

namespace Web.Framework.HTML
{
    public class HTMLTable : HTMLBase
    {
        private DataTable data;
        private string body;
        private string title;

        public string LinkColumn { get; set; }

        private ViewSettings ViewSetting { get; set; }

        public HTMLTable(DataTable data, ViewSettings settings)
        {
            this.ViewSetting = settings;
            this.data = data;
            title = settings.Title;

            Class = "panel";
            LinkColumn = settings.LinkColumn;
        }

        public override string GenerateHTML()
        {
            StringBuilder output = new StringBuilder();

            GenerateBody();

            if (ViewSetting.GUID != null)
                output.AppendFormat("<div id='{0}' class='{1}'>", ViewSetting.GUID, Class);
            else
                output.AppendFormat("<div class='{0}'>", Class);

            //Header
            if (this.ViewSetting.SHOWTABLEHEADER && title != null)
            {
                output.Append("<div class='panel-header'>");
                output.AppendFormat("<h3>{0}</h3>", title);

                output.Append("<div class='panel-toolbar'>");

                var url = ViewSetting.URL;
                var newurl = string.Format("../{0}/new?guid={1}&{2}", ViewSetting.Controller, ViewSetting.GUID, ViewSetting.QueryString);

                if (this.ViewSetting.SHOWEDITOR)
                {
                    if (ViewSetting.Parent != null)
                        newurl += string.Format("&pn={0}&pid={1}", ViewSetting.Parent.Table, ViewSetting.Parent.ID);

                    output.AppendFormat("<a class='form' href='{0}'><i class='fa fa-plus'></i></a>", newurl);
                }

                if (this.ViewSetting.SHOWSEARCH)
                    output.AppendFormat("<form method='POST' action='{0}' enctype = 'multipart/form-data' runat='server'><input class='form-control' name='search' id='search' type='text' placeholder='Search'/></form>", url);

                output.Append("</div>");
                output.Append("</div>");
            }

            //Content
            if (body != null)
            {
                output.Append("<div class='panel-body'>");
                output.Append(body);
                output.Append("</div>");
            }

            //Footer

            if (ViewSetting.SHOWPAGE)
            {
                output.Append("<div class='panel-footer'>");
                output.Append(ViewSetting.GeneratePager());
                output.Append("</div>");
            }

            output.Append("</div>");

            return output.ToString();
        }

        private void GenerateBody()
        {
            var url = ViewSetting.URL;
            var editurl = string.Format("../{0}/edit?guid={1}", ViewSetting.Controller, ViewSetting.GUID);
            var deleteurl = string.Format("../{0}/delete?guid={1}", ViewSetting.Controller, ViewSetting.GUID);

            if (this.ViewSetting.SHOWEDITOR)
                if (ViewSetting.Parent != null)
                    editurl += string.Format("&pn={0}&pid={1}", ViewSetting.Parent.Table, ViewSetting.Parent.ID);

            StringBuilder output = new StringBuilder();

            output.Append("<table class='table'>");

            //Column Header

            if (this.ViewSetting.SHOWCOLUMNHEADER)
            {
                output.Append("<tr>");

                if (this.ViewSetting.SHOWEDITOR)
                {
                    output.Append("<th class='editor'></th>");
                    output.Append("<th class='editor'></th>");
                }

                if (ViewSetting.Properties != null)
                {
                    var columns = ViewSetting.Properties.Columns;

                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (columns[i].Visible && columns[i].Type != Enums.EnumInputType.PARENT && columns[i].Type != Enums.EnumInputType.HIDDEN)
                            output.AppendFormat("<th>{0}</th>", columns[i].Caption);
                    }
                }
                else
                {
                    foreach (DataColumn column in data.Columns)
                        output.AppendFormat("<th>{0}</th>", column.Caption);
                }

                output.Append("</tr>");
            }

            output.Append("<tr>");

            int counter = 0;
            string value;
            bool HasID = false;

            if (data.Columns.Contains("id"))
                HasID = true;

            foreach (DataRow row in data.Rows)
            {
                var id = "0";

                if (HasID)
                    id = row["id"].ToString();

                if (counter % 2 == 0)
                    output.Append("<tr class='row-odd'>");
                else
                    output.Append("<tr class='row-even'>");

                if (this.ViewSetting.SHOWEDITOR)
                {
                    output.AppendFormat("<td class='editor'><a class='form' href='{1}&id={0}'><i class='fa fa-pencil'></i></a></td>", id, editurl);
                    output.AppendFormat("<td class='editor'><a class='form' href='{1}&id={0}'><i class='fa fa-trash'></i></a></td>", id, deleteurl);
                }

                if (ViewSetting.Properties != null)
                {
                    var columns = ViewSetting.Properties.Columns;

                    for (int i = 0; i < columns.Count; i++)
                    {
                        PropertyColumn column = columns[i];

                        if (columns[i].Visible && column.Type != Enums.EnumInputType.PARENT && columns[i].Type != Enums.EnumInputType.HIDDEN)
                        {
                            if (column.Type != EnumInputType.CHILD && column.FormatType == EnumFormatType.CUSTOM)
                                value = column.FormatDisplay("", "", row, column.Name);
                            else {
                                if (row.Table.Columns.Contains(column.Name))
                                    value = row[column.Name].ToString();
                                else
                                    value = "";
                            }

                            if (column.Name == LinkColumn)
                                output.AppendFormat("<td><a href='../{1}/detail?id={2}'>{0}</a></td>", value, ViewSetting.Controller, id);
                            else
                                output.AppendFormat("<td>{0}</td>", value);
                        }
                    }
                }
                else
                {
                    foreach (DataColumn column in data.Columns)
                    {
                        value = row[column.ColumnName].ToString();

                        if (column.ColumnName == LinkColumn)
                            output.AppendFormat("<td><a href='../{1}/detail?id={2}'>{0}</a></td>", value, ViewSetting.Controller, id);
                        else
                            output.AppendFormat("<td>{0}</td>", value);
                    }
                }

                output.Append("</tr>");
                counter++;
            }

            output.Append("</table>");

            body = output.ToString();
        }



    }
}