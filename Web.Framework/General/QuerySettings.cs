using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.General
{
    public class QuerySettings
    {
        public bool QueryAll { get; set; }
        public EnumOrder SortOrder { get; set; }
        public string SortColumn { get; set; }

        public QuerySettings()
        {
            QueryAll = false;
            SortOrder = EnumOrder.DESCENDING;
            SortColumn = "id";
        }
    }
}