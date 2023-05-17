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
    public class HTMLForm : HTMLBase
    {
        public string Title { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Body { get; set; }

        private ViewSettings settings { get; set; }
        private StringBuilder bind;

        public HTMLForm(ViewSettings settings)
        {
            this.settings = settings;
            this.bind = new StringBuilder();
        }

        public override string GenerateHTML()
        {
            string size = "";

            if (Width != 0)
                size = "width:" + Width + "px;";

            if (Height != 0)
                size += "height:" + Height + "px;";

            if (size != "")
                size = string.Format(" style='{0}'", size);

            StringBuilder output = new StringBuilder();

            output.Append("<div id='form-background'></div>");
            output.AppendFormat("<div class='form {0}'{1}>", Class, size);

            //Header
            output.Append("<div class='form-header'>");
            output.AppendFormat("<h1>{0}</h1>", Title);
            output.Append("<i class='form-close fa fa-close'></i>");
            output.Append("</div>");

            //Body
            var url = settings.URL;

            output.Append("<div class='form-body'>");
            output.AppendFormat("<form method='POST' action='{0}' enctype='multipart/form-data' runat='server'>", url);
            output.Append("<div class='form-inner'>");
            output.Append(Body);
            output.Append(bind.ToString());
            output.Append("</div>");

            //Footer
            output.Append("<div class='form-footer'>");
            output.AppendFormat("<div class='button form-submit'><input type='submit' value='OK'></div>", url);
            output.Append("<div class='button form-cancel'>Cancel</div>");
            output.Append("</div>");

            output.Append("</form>");
            output.Append("</div>");

            output.Append("</div>");
            return output.ToString();
        }

        public void Bind(PropertyColumn column)
        {
            bind.Append(column.GenerateInput());
        }

        public void Bind(PropertyTable properties)
        {
            foreach (PropertyColumn column in properties.Columns)
            {
                if (column.Type == Enums.EnumInputType.PARENT)
                {
                    if (settings.Parent != null)
                    {
                        var parent = (PropertyParent)column;
                        parent.Value = settings.Parent.ID;
                        bind.Append(column.GenerateInput());
                    }
                }
                else if (column.Type != Enums.EnumInputType.CHILD)
                {
                    if (column.Type == Enums.EnumInputType.COMBOBOX)
                    {
                        var combo = (PropertyComboBox)column;
                        bind.Append(column.GenerateInput(combo.Value));
                    }
                    else
                        bind.Append(column.GenerateInput());
                }
            }
        }

        public void Bind(PropertyTable properties, DataTable data)
        {
            DataRow row = data.Rows[0];

            foreach (PropertyColumn column in properties.Columns)
            {
                if (column.Type == Enums.EnumInputType.PARENT)
                {
                    if (settings.Parent != null)
                    {
                        var parent = (PropertyParent)column;
                        parent.Value = settings.Parent.ID;
                        bind.Append(column.GenerateInput());
                    }
                }
                else if (column.Type == Enums.EnumInputType.COMBOBOX)
                {
                    bind.Append(column.GenerateInput(row[column.Name + "_id"].ToString()));
                }
                else if (column.Type != Enums.EnumInputType.CHILD)
                    bind.Append(column.GenerateInput(row[column.Name].ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="icon">EnumMessageIcon</param>
        public void Message(string message, string icon)
        {
            Body = string.Format("<i class='message-icon fa fa-{1}'></i><div class='message'>{0}</div>", message, icon);
        }
    }
}