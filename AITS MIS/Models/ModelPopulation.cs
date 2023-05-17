using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.General;
using Web.Framework.Models;
using Web.Framework.Property;

namespace UCOnline.Models
{
    public class ModelPopulation : ModelBase
    {
        public string CountyID { get; set; }

        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelPopulation() : base("countrypopulation", "Country Population")
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

                columns.Add(new PropertyText("population"));
                columns.Add(new PropertyText("urbanpopulation", "urban population"));
                columns.Add(new PropertyText("area", "area (km<sup>2</sup>)"));
                columns.Add(new PropertyText("incomelevel", "income level ($)"));

                columns.Add(new PropertyTextHidden("description"));
                columns.Add(new PropertyTextHidden("source"));

                properties.Columns = columns;
            }
        }

        protected override void GenerateQuery(QuerySettings settings)
        {
            base.GenerateQuery(settings);

            var countryid = CountyID;
            server.SelectFilter("country_id", countryid);
        }

        protected override void UpdateBeforeShow(DataTable data)
        {
            base.UpdateBeforeShow(data);

            int i = 1;

            foreach (DataRow row in data.Rows)
            {
                var income = row["incomelevel"].ToString();
                var definition = row["description"].ToString().Trim();
                var defineby = row["source"].ToString().Trim();

                if (definition != "")
                    row["incomelevel"] = string.Format("{0} <div class='table-info'><i class='fa fa-info'></i><div class='table-tooltip table-tooltip-right'>{1} <span class='source'>{3}</span></div></div>", income, definition, i++, defineby);
            }
        }
    }
}