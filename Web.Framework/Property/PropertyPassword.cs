using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyPassword: PropertyColumn
    {
        public string Value { get; set; }

        public PropertyPassword(string name) : base(name)
        {
            Type = EnumInputType.TEXT;
        }

        public override string GenerateInput(string value = "")
        {
            if (ReadOnly)
                return string.Format("<input class='form-control' type='hidden' name='{0}' value='{1}'/>", Name, Value);
            else
                return string.Format("<div class='col-1'>{0}</div><div class='col-2'><input class='form-control' type='password' id='{1}' name='{1}' value='{2}'/></div>", Name, Caption, value);
        }
    }
}