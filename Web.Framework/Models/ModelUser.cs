using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.HTML;
using Web.Framework.Property;

namespace Web.Framework.Models
{
    public class ModelUser : ModelBase
    {
        private static PropertyTable properties;
        protected override PropertyTable Properties { get { return properties; } }

        public ModelUser() : base("user", "User Accounts")
        {
            QuerySetting.SortOrder = Web.Framework.Enums.EnumOrder.ASCENDING;
            QuerySetting.SortColumn = "name";

            ViewSetting.SHOWEDITOR = true;
            ViewSetting.LinkColumn = "name";
            ViewSetting.NewTitle = "New User";
            ViewSetting.EditTitle = "Edit User";

            if (properties == null)
            {
                properties = new PropertyTable();

                var columns = new List<PropertyColumn>();

                var combo = new PropertyComboBox("usertype", "account");
                combo.Select = new TableColumnQuery("usertype", "name");
                columns.Add(combo);

                columns.Add(new PropertyText("firstname"));
                columns.Add(new PropertyText("middlename"));
                columns.Add(new PropertyText("lastname"));
                columns.Add(new PropertyText("email"));
                columns.Add(new PropertyText("username"));
                columns.Add(new PropertyPassword("password"));

                properties.Columns = columns;
            }
        }

        public string ShowLogin()
        {
            var properties = new PropertyTable();

            var columns = new List<PropertyColumn>();
            columns.Add(new PropertyText("username"));
            columns.Add(new PropertyPassword("password"));
            columns.Add(new PropertyBoolean("rememberme", "Remember"));

            properties.Columns = columns;

            ViewSetting.URL = "../account/verify";

            HTMLForm form = new HTMLForm(ViewSetting);
            form.Title = "Login";
            form.Bind(properties);

            return form.GenerateHTML();
        }

        public override ServerAction Insert(HttpRequestBase Request)
        {
            PropertyTable properties = Properties;
            Dictionary<string, string> values = new Dictionary<string, string>();
            Encrypt64 encrypt = new Encrypt64();

            bool handle = false;
            string name;

            foreach (PropertyColumn column in properties.Columns)
            {
                if (column.Type != EnumInputType.CHILD)
                {
                    string item = Request.Params[column.Name];
                    name = column.Name;

                    if (column.Name == "password")
                        item = encrypt.Encrypt(item);

                    switch (column.Type)
                    {
                        case EnumInputType.COMBOBOX:
                            name += "_id";
                            break;

                        case EnumInputType.DATE:
                            item = server.FormatDateTime(item);
                            break;
                    }

                    values.Add(name, item);
                    server.InsertValue(name, item);
                    handle = true;
                }
            }

            if (handle)
            {
                StringBuilder fullname = new StringBuilder();

                string item = Request.Params["firstname"];

                if (item != null)
                    fullname.Append(item);

                item = Request.Params["middlename"];

                if (item != null)
                {
                    if (fullname.Length == 0)
                        fullname.Append(item);
                    else
                        fullname.Append(" " + item);
                }

                item = Request.Params["lastname"];

                if (item != null)
                {
                    if (fullname.Length == 0)
                        fullname.Append(item);
                    else
                        fullname.Append(" " + item);
                }

                server.InsertValue("name", fullname.ToString());
                server.InsertValue("secureid", Guid.NewGuid().ToString());

                server.UserID = "0";
                server.Username = "";
                server.InsertQuery();
            }

            return null;
        }
    }
}