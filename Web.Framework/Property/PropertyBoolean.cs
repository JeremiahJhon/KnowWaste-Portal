using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyBoolean: PropertyColumn
    {
        public string Value { get; set; }

        public PropertyBoolean(string name) : base(name)
        {
            Type = EnumInputType.BOOLEAN;
            FormatType = EnumFormatType.CUSTOM;
        }

        public PropertyBoolean(string name, string caption) : base(name, caption)
        {
            Type = EnumInputType.BOOLEAN;
            FormatType = EnumFormatType.CUSTOM;
        }

        public override string FormatDisplay(string table, string id, DataRow row, string column)
        {
            if (row["estimated"].ToString() == "0")
                return "NO";

            else if (row["estimated"].ToString() == "1")
                return "YES";

            return "NO";
        }

        public override string GenerateInput(string value = "")
        {
            if (ReadOnly)
                return string.Format("<input class='form-control' type='hidden' name='{0}' value='{1}'/>", Name, Value);
            else
            {
                if (value == "0")
                    return string.Format("<div class='col-1 col-checkbox'><input class='form-control' type='checkbox' id='{0}' name='{0}' value='{2}'/> {1}</div><div class='col-2 col-checkbox'></div>", Name, Caption, value);
                else
                    return string.Format("<div class='col-1 col-checkbox'><input class='form-control' type='checkbox' id='{0}' name='{0}' value='{2}' checked/> {1}</div><div class='col-2 col-checkbox'></div>", Name, Caption, value);
            }
        }
       
    }
}