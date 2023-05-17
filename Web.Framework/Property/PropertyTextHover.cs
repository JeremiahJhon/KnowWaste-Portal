using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyTextHover: PropertyColumn
    {
        public string Value { get; set; }

        public PropertyTextHover(string name) : base(name)
        {
            Type = EnumInputType.TEXT;
            FormatType = EnumFormatType.CUSTOM;
        }

        public PropertyTextHover(string name, string caption) : base(name, caption)
        {
            Type = EnumInputType.TEXT;
            FormatType = EnumFormatType.CUSTOM;
        }

        public override string FormatDisplay(string table, string id, DataRow row, string column)
        {
            var value = row[column].ToString();

            if (value.Trim() != "")
                return string.Format("<div class='text-hover'><i class='fa fa-info'></i><span class='hover'>{0}</span></div>", value);
            else
                return "";
        }

        public override string GenerateInput(string value = "")
        {
            if (ReadOnly)
                return string.Format("<input class='form-control' type='hidden' name='{0}' value='{1}'/>", Name, Value);
            else
                return string.Format("<div class='col-1'>{1}</div><div class='col-2'><input class='form-control' type='text' id='{0}' name='{0}' value='{2}'/></div>", Name, Caption, value);
        }
    }
}