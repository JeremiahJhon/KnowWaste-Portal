using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Models;
using Web.Framework.Property;

namespace Web.Framework.General
{
    public class JsonOutput
    {
        public string ID { get; set; }

        public string Body { get; set; }

        public PropertyTable Model { get; set; }

        public string Message { get; set; }

    }
}