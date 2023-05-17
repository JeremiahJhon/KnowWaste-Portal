using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyTextArea: PropertyColumn
    {
        public string Value { get; set; }

        public PropertyTextArea(string name) : base(name)
        {
            Type = EnumInputType.TEXT;
        }

        public PropertyTextArea(string name, string caption) : base(name, caption)
        {
            Type = EnumInputType.TEXT;
        }

        public override string GenerateInput(string value = "")
        {
            value = value.Replace("</", "[/");
            value = value.Replace("<", "[");
            value = value.Replace(">", "]");

            if (ReadOnly)
                return string.Format("<input class='form-control textarea' type='hidden' name='{0}' value='{1}'/>", Name, Value);
            else
                return string.Format("<div class='col-1'>{1}</div><div class='col-2 col-large'><textarea class='form-control textarea' id='{0}' name='{0}'>{2}</textarea></div>", Name, Caption, value);
        }
    }
}