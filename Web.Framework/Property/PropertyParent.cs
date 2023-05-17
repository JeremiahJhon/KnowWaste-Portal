using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyParent: PropertyColumn
    {
        public string Value { get; set; }

        public PropertyParent(string name) : base(name)
        {
            Type = EnumInputType.PARENT;
        }

        public override string GenerateInput(string value = "")
        {
            return string.Format("<input class='form-control' type='hidden' name='{0}' value='{1}'/>", Name, Value);
        }
    }
}