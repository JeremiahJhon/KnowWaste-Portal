using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Framework.Server
{
    public class UnionBase : ServerBase
    {
        private string filter;
        private string limit;

        public UnionBase(string table) : base(table)
        {
        }

        public override DataTable SelectQuery()
        {
            ServerBase columnserver = new ServerBase("_union");
            columnserver.SelectColumn("columns");
            columnserver.SelectFilter("tablename", Table);
            columnserver.SelectLimit(1);

            DataTable columns = columnserver.SelectQuery();

            if (columns.Rows.Count != 0)
            {
                string column = columns.Rows[0][0].ToString();

                ServerBase server = new ServerBase(Table);
                server.SelectFilter("deleted", "0");

                DataTable data = server.SelectQuery();

                StringBuilder query = new StringBuilder();
                query.Append("SELECT * FROM (");

                int i = 0;

                foreach (DataRow row in data.Rows)
                {
                    if (i != 0)
                    {
                        query.AppendLine();
                        query.AppendLine("UNION ALL");
                    }

                    query.AppendFormat("SELECT {0},'{1}' AS tablename FROM {1} WHERE deleted = '0'", column, row["tablename"].ToString());
                    i++;
                }

                query.Append(") AS TBL ");

                server.Query = query.ToString();

                if (filter != null)
                    server.Query = string.Format("{0} WHERE ({1})", query.ToString(), filter);
                else
                    server.Query = query.ToString();

                if (limit != null)
                    server.Query += limit;

                return server.ExecuteQuery();
            }

            return null;
        }

        public override void SelectFilter(string filter)
        {
            this.filter = filter;
        }

    }
}