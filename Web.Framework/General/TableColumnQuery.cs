using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Framework.General
{
    public class TableColumnQuery
    {
        public string Table { get; set; }
        public string Column { get; set; }

        public TableColumnQuery()
        {
        }

        public TableColumnQuery(string table, string column)
        {
            Table = table;
            Column = column;
        }
    }
}