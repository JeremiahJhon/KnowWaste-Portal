using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Models;
using Web.Framework.Server;

namespace UCOnline.Models
{
    public class ModelHome : ModelBase
    {
        public ModelHome() : base("", "Home")
        {
        }

        public override string Show()
        {
            return new ModelNews().Show();
        }

        public virtual string ShowThematicMenuHome()
        {
            var output = new StringBuilder();
            var server = new ServerBase("geotheme");
            server.SelectOrder("ordersort", Web.Framework.Enums.EnumOrder.ASCENDING);

            DataTable data = server.SelectQuery();

            foreach (DataRow row in data.Rows)
            {
                output.Append("<li class='c-menu-type-classic'>");
                output.AppendFormat("<a href='../Thematicarea?gid={1}' class='c-link dropdown-toggle'><div><img src='../Content/Images/Logo/{0}' /></div><div>{2}</div></a>", row["photo"].ToString(), row["id"].ToString(), row["name"].ToString());
                output.Append("</li>");
            }

            return output.ToString();
        }
    }
}