using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.General;
using Web.Framework.Models;
using Web.Framework.Property;

namespace UCOnline.Models
{
    public class ModelReferences : ModelBase
    {
        public string CountyID { get; set; }

        public List<string> RefIDs { get; set; }

        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelReferences() : base("countryreferences", "References")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";
            QuerySetting.QueryAll = true;

            ViewSetting.SHOWTABLEHEADER = false;
            ViewSetting.SHOWEDITOR = false;

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();

                columns.Add(new PropertyText("Referencenumber", "no."));
                columns.Add(new PropertyText("reference"));

                properties.Columns = columns;
            }
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);

            var countryid = CountyID;
            server.SelectFilter("country_id", countryid);

            StringBuilder ids = new StringBuilder();

            for (int i = 0; i < RefIDs.Count; i++)
            {
                if (RefIDs[i] != "")
                {
                    if (i == 0)
                        ids.Append(RefIDs[i]);
                    else
                        ids.Append("," + RefIDs[i]);
                }
            }

            server.SelectFilter(string.Format(" AND Referencenumber IN ({0})", ids.ToString()));
        }
    }
}