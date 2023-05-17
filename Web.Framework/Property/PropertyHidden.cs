using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Enums;
using Web.Framework.Server;

namespace Web.Framework.Property
{
    public class PropertyHidden: PropertyColumn
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public EnumComboDisplayType DisplayType { get; set; }

        public PropertyHidden(string name) : base(name)
        {
            Type = EnumInputType.HIDDEN;
        }

        public PropertyHidden(string name, string table, string column) : base(name)
        {
            Type = EnumInputType.HIDDEN;
            Table = table;
            Column = column;
        }
    }
}