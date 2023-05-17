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
    public class PropertyChild: PropertyColumn
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public EnumComboDisplayType DisplayType { get; set; }

        public PropertyChild(string name, string table, string column) : base(name)
        {
            Type = EnumInputType.CHILD;
            Table = table;
            Column = column;
            FormatType = EnumFormatType.CUSTOM;
        }

        public override string FormatDisplay(string table, string id, DataRow row, string column)
        {
            var childserver = new ServerBase(Table);
            childserver.SelectColumn(Column);
            childserver.SelectFilter(table + "_id", id);

            return FormatChildDisplay(childserver.SelectQuery());
       }

        protected virtual string FormatChildDisplay(DataTable data)
        {
            StringBuilder output = new StringBuilder();

            foreach (DataRow childrow in data.Rows)
                output.AppendFormat("<div class='button'><a href='../{0}/detail?id={2}'>{3}</a></div>", Table, Name, childrow[0].ToString(), childrow[1].ToString());

            return output.ToString();
        }
    }
}