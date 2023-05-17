using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyDateTime: PropertyColumn
    {
        public DateTime Value { get; set; }

        public PropertyDateTime(string name) : base(name)
        {
            Type = EnumInputType.DATE;
            FormatType = EnumFormatType.CUSTOM;
        }

        public PropertyDateTime(string name, string caption) : base(name, caption)
        {
            Type = EnumInputType.DATE;
            FormatType = EnumFormatType.CUSTOM;
        }

        public override string GenerateInput(string value = "")
        {
            return string.Format("<div class='col-1'>{1}</div><div class='col-2'><input class='form-control' type='date' id='{0}' name='{0}' value='{2}'/></div>", Name, Caption, value);
        }

        public override string FormatDisplay(string table, string id, DataRow row, string column)
        {
            var value = row[column].ToString();
            return string.Format("<div class='property-time'>{0}</div><div class='property-date'>{1}</div>", DateTime.Parse(value).ToString("H:mm"),  DateTime.Parse(value).ToString("dd-MMM-yy"));
        }
    }
}