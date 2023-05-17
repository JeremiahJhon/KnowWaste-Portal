using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.Server;

namespace Web.Framework.Property
{
    public class PropertyComboBox : PropertyColumn
    {
        public string ReferenceID { get; set; }
        public TableColumnQuery Select { get; set; }
        public TableColumnQuery Insert { get; set; }
        public EnumComboInputType InputType { get; set; }

        public string SortColumn { get; set; }

        public string Value { get; set; }

        public PropertyComboBox(string name) : base(name)
        {
            ReferenceID = "id";
            Caption = name;
            Type = EnumInputType.COMBOBOX;
            InputType = EnumComboInputType.COMBOBOX;
        }

        public PropertyComboBox(string name, string caption) : base(name)
        {
            ReferenceID = "id";
            Caption = caption;
            Type = EnumInputType.COMBOBOX;
            InputType = EnumComboInputType.COMBOBOX;
        }

        public override string GenerateInput(string value = "")
        {
            TableColumnQuery query = Insert;

            if (Insert == null)
                query = Select;

            if (query == null)
                return "<p>No insert or select query in the property combobox.</p>";

            ServerBase server = new ServerBase(query.Table);
            server.SelectColumn(ReferenceID);
            server.SelectColumn(query.Column);

            if (SortColumn != null)
                server.SelectOrder(SortColumn, EnumOrder.ASCENDING);

            DataTable optiontable = server.SelectQuery();

            StringBuilder output = new StringBuilder();

            output.AppendFormat("<div class='col-1'>{0}</div>", Caption);

            switch (InputType)
            {
                case EnumComboInputType.COMBOBOX:
                    output.Append("<div class='col-2'>");
                    output.AppendFormat("<select name='{0}' class='form-control'>", Name);

                    if (value == "")
                    {
                        foreach (DataRow row in optiontable.Rows)
                            output.AppendFormat("<option value='{0}'>{1}</option>", row[0], row[1]);
                    }
                    else
                    {
                        foreach (DataRow row in optiontable.Rows)
                        {
                            if (row[0].ToString() == value)
                                output.AppendFormat("<option value='{0}' selected>{1}</option>", row[0], row[1]);
                            else
                                output.AppendFormat("<option value='{0}'>{1}</option>", row[0], row[1]);
                        }
                    }

                    output.Append("</select>");
                    output.Append("</div>");
                    break;

                case EnumComboInputType.CHECKBOX:
                    output.Append("<div class='col-2 combo'>");

                    foreach (DataRow row in optiontable.Rows)
                    {
                        if (row[0].ToString() == value)
                            output.AppendFormat("<div class='button'><label><input type='checkbox' name='{0}' value='{1}' checked>{2}</label></div>", Name, row[0], row[1]);
                        else
                            output.AppendFormat("<div class='button'><label><input type='checkbox' name='{0}' value='{1}'>{2}</label></div>", Name, row[0], row[1]);
                    }

                    output.Append("</div>");
                    break;
            }

            return output.ToString();
        }
    }
}