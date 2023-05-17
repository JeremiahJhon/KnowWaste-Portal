using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public class PropertyTextHidden: PropertyText
    {
        public PropertyTextHidden(string name) : base(name)
        {
            Type = EnumInputType.HIDDEN;
        }
    }
}