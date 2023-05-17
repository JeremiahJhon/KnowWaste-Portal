using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.General
{
    public class ServerAction
    {
        public string Message { get; set; }
        public EnumServerAction Action { get; set; }
    }
}