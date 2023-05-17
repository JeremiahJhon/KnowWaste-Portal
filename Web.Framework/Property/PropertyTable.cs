using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Framework.Property
{
    public class PropertyTable
    {
        public List<PropertyColumn> Columns { get; set; }
        public List<PropertyRow> Rows { get; set; }

        public bool HasChild { get; set; }

        public PropertyTable()
        {
            Columns = new List<PropertyColumn>();
            Rows = new List<PropertyRow>();
        }
    }
}