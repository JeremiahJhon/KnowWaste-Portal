using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Framework.HTML
{
    public abstract class HTMLBase
    {
        public string Class;
        public abstract string GenerateHTML();
    }
}