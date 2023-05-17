using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyFile: PropertyColumn
    {
        public string Value { get; set; }

        public PropertyFile(string name) : base(name)
        {
            Type = EnumInputType.FILE;
            FormatType = EnumFormatType.CUSTOM;
        }

        public PropertyFile(string name, string caption) : base(name, caption)
        {
            Type = EnumInputType.FILE;
            FormatType = EnumFormatType.CUSTOM;
        }

        public override string FormatDisplay(string table, string id, DataRow row, string column)
        {
            var value = row[column].ToString();

            if (value.Trim() != "")
                return string.Format("<div class='text-hover'><i class='fa fa-file-o'></i><span class='hover'>{0}</span></div>", value);
            else
                return "";
        }

        public override string GenerateInput(string value = "")
        {
            return string.Format("<div class='col-1'>{1}</div><div class='col-2'><input class='form-control' type='file' id='{0}' name='{0}' value='{2}' placeholder='Select file'/></div>", Name, Caption, value);
        }
    }
}