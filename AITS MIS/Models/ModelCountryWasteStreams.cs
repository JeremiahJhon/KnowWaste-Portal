using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Models;
using Web.Framework.Property;

namespace UCOnline.Models
{
    public class ModelCountryWasteStreams : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelCountryWasteStreams() : base("countrywastestreams", "Waste Streams")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "id";

            server.PageLimit = 5;

            ViewSetting.SHOWEDITOR = true;
            //ViewSetting.LinkColumn = "code";

            //ViewSetting.NewTitle = "New Project";
            //ViewSetting.EditTitle = "Edit Project";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();
                columns.Add(new PropertyText("dateentered"));
                columns.Add(new PropertyText("Definitions"));
                columns.Add(new PropertyText("Definedby"));
                columns.Add(new PropertyText("Totalgenerated"));
                columns.Add(new PropertyText("Hazardous"));
                columns.Add(new PropertyText("Totalcollected"));
                columns.Add(new PropertyText("Recycled"));
                columns.Add(new PropertyText("Recovered"));
                columns.Add(new PropertyText("Disposal"));
                columns.Add(new PropertyText("Treatment"));
                columns.Add(new PropertyText("Reuse"));
                columns.Add(new PropertyText("Sludge"));


                var combo = new PropertyComboBox("country");
                combo.Select = new TableColumnQuery("country", "name");

                var wcombo = new PropertyComboBox("wastecat");
                wcombo.Select = new TableColumnQuery("wastecategory", "name");

                columns.Add(combo);
                columns.Add(wcombo);

                properties.Columns = columns;
            }
        }
    }
}