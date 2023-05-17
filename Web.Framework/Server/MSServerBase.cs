using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.Property;

namespace Web.Framework.Server
{
    public class ServerBase : MSSQLServer
    {
        #region ----- Declarations -----

        private class ColumnUpdate
        {
            public string column;
            public string value;

            public ColumnUpdate(string column, string value)
            {
                this.column = column;
                this.value = value;
            }
        }

        public string Username { get; set; }

        public string UserID { get; set; }

        public static bool HasReference { get; set; }

        public static bool HasRelation { get; set; }

        public static string UploadDocumentPath { get; set; }

        public static string RelativePath { get; set; }

        public static string UploadPhotoPath { get; set; }

        public static string RelativePhotoPath { get; set; }

        public static Dictionary<string, DataTable> Tables;

        public static bool RequestJson { get; set; }

        public string Table { get; set; }

        public int PageLimit { get; set; }

        private StringBuilder Columns;
        private StringBuilder Where;
        private StringBuilder Order;
        private StringBuilder Limit;
        private StringBuilder Value;

        private List<ColumnUpdate> Updates;

        #endregion

        #region ----- Initializations -----

        public MSServerBase(string table)
        {
            Table = table;
            PageLimit = 10;

            Columns = new StringBuilder();
            Where = new StringBuilder();
            Order = new StringBuilder();
            Limit = new StringBuilder();
            Value = new StringBuilder();

            Updates = new List<ColumnUpdate>();
        }

        public new void Initialize()
        {
            Tables = new Dictionary<string, DataTable>();
            DataTable tables = ShowTables();

            foreach (DataRow row in tables.Rows)
            {
                string table = row[0].ToString().ToLower();
                switch (table)
                {
                    case "_reference":
                        HasReference = true;
                        break;

                    case "_relation":
                        HasRelation = true;
                        break;
                }

                Table = table;
                DataTable columns = ShowColumns();

                Tables.Add(table, columns);
            }

            UploadPhotoPath = "../";
            UploadDocumentPath = "../";
        }

        public string TableCaption(string table)
        {
            if (HasRelation)
            {
                MSServerBase server = new MSServerBase("_relation");
                server.SelectColumn("name");
                server.SelectFilter("tablerel", Table);

                DataTable data = server.SelectQuery();

                if (data.Rows.Count != 0)
                    return data.Rows[0][0].ToString();
                else
                    return table;
            }
            else
                return table;
        }

        #endregion

        #region ----- Select Query -----

        public DataTable ShowTables()
        {
            Query = "SELECT distinct TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS";
            return ExecuteQuery();
        }

        public DataTable ShowColumns()
        {
            Query = string.Format("SELECT distinct COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", Table);
            return base.ExecuteQuery();
        }

        public DataTable SelectAll(int page = 0)
        {
            if (page == 0)
            {
                Query = string.Format("SELECT * FROM {0}", Table);
                return ExecuteQuery();
            }
            else
            {
                SelectPage(page);
                return SelectQuery();
            }
        }

        public virtual DataTable SelectByID(string id)
        {
            Query = string.Format("SELECT * FROM {0} WHERE ID={1}", Table, id);
            return ExecuteQuery();
        }

        public virtual DataTable SelectBySecureID(string id)
        {
            Query = string.Format("SELECT * FROM {0} WHERE secureid='{1}'", Table, id);
            return ExecuteQuery();
        }

        public virtual DataTable SelectDistinct(string column)
        {
            Query = string.Format("SELECT DISTINCT({1}) AS {1} FROM {0} ORDER BY {1} ASC", Table, column);
            return ExecuteQuery();
        }

        public virtual DataTable SelectQuery()
        {
            string column = null;

            if (Columns.Length == 0)
                column = "*";
            else
                column = Columns.ToString();

            if (Where.Length == 0)
                Where.Append(" WHERE deleted = '0'");
            else
                Where.Append(" AND deleted = '0'");

            Query = string.Format("SELECT {4} {5}{0}{5}FROM {1}{2}{3}", column, Table, Where.ToString(), Order.ToString(), Limit.ToString(), Environment.NewLine);

            Clear();

            return ExecuteQuery();
        }

        public virtual int SelectCountQuery()
        {
            string column = "COUNT(*)";

            if (Where.Length == 0)
                Where.Append(" WHERE deleted = '0'");
            else
                Where.Append(" AND deleted = '0'");

            Query = string.Format("SELECT {5}{0}{5}FROM {1}{2}{3}{4}", column, Table, Where.ToString(), Order.ToString(), Limit.ToString(), Environment.NewLine);
            Clear();

            DataTable data = ExecuteQuery();

            if (data.Rows.Count != 0)
                return int.Parse(data.Rows[0][0].ToString());
            else
                return 0;
        }

        public void GenerateQuery()
        {
            string column = null;

            if (Columns.Length == 0)
                column = "*";
            else
                column = Columns.ToString();

            Query = string.Format("SELECT {5}{0}{5}FROM {1}{2}{3}{4}", column, Table, Where.ToString(), Order.ToString(), Limit.ToString(), Environment.NewLine);
            Clear();
        }

        public string SelectLastID()
        {
            Clear();
            SelectColumn("ID");
            SelectLimit(1);
            SelectOrder("ID", EnumOrder.DESCENDING);

            DataTable dResult = SelectQuery();

            if (dResult.Rows.Count != 0)
                return dResult.Rows[0].ItemArray[0].ToString();

            return "0";
        }

        public DataTable SelectLastRow()
        {
            Clear();
            SelectLimit(1);
            SelectOrder("ID", EnumOrder.DESCENDING);

            return SelectQuery();
        }

        public void SelectColumn(List<PropertyColumn> columns)
        {
            if (Columns.Length == 0)
                Columns.Append("*");

            foreach (PropertyColumn column in columns)
            {
                if (column.Type == EnumInputType.COMBOBOX)
                {
                    var combo = (PropertyComboBox)column;
                    SelectTableColumn(combo.Select.Table, combo.Select.Column, combo.Name);
                }
                else if (column.Type == EnumInputType.HIDDEN)
                {
                    PropertyHidden hidden = column as PropertyHidden;

                    if (hidden != null)
                        SelectTableColumn(hidden.Table, hidden.Column, hidden.Name);
                }
                else if (column.Type == EnumInputType.CHILD)
                {
                    var combo = (PropertyChild)column;
                    SelectTableColumn(combo.Table, combo.Column, combo.Name);
                }
            }
        }

        public void SelectColumn(string column)
        {
            if (Columns.Length == 0)
                Columns.Append(column);
            else
                Columns.Append("," + column);
        }

        public void SelectTableColumn(string table, string column, string name)
        {
            string query = string.Format("(SELECT {2} FROM {1} WHERE {0}.{1}_id={1}.id LIMIT 1) AS {3}", Table, table, column, name);

            if (Columns.Length == 0)
                Columns.Append(query);
            else
                Columns.Append("," + query);
        }

        public virtual void SelectPage(int page)
        {
            if (PageLimit != 0 || page != 0)
                SelectLimit((page - 1) * PageLimit, PageLimit);
        }

        public void SelectLimit(int limit)
        {
            Limit.Clear();
            Limit.Append(string.Format(" TOP {0}", limit));
        }

        public void SelectLimit(int dFrom, int dSize)
        {
            Limit.Clear();
            Limit.Append(string.Format(" LIMIT {0}, {1}", dFrom, dSize));
        }

        public virtual void SelectFilterAND(string sql)
        {
            if (Where.Length == 0)
                Where.Append(" WHERE " + sql);
            else
                Where.Append(" AND " + sql + " ");
        }

        public virtual void SelectFilter(string sql)
        {
            if (Where.Length == 0)
                Where.Append(" WHERE " + sql);
            else
                Where.Append(" " + sql + " ");
        }

        public virtual void SelectFilterRaw(string filter)
        {
            StringBuilder filters = new StringBuilder();
            DataTable columns = ShowColumns();

            foreach (DataRow row in columns.Rows)
            {
                if (filters.Length == 0)
                    filters.AppendFormat("{0} LIKE '%{1}%'", row[0].ToString(), filter);
                else
                    filters.AppendFormat(" OR {0} LIKE '%{1}%'", row[0].ToString(), filter);
            }

            SelectFilter("(" + filters.ToString() + ")");
        }

        public void SelectFilter(string column, string value)
        {
            if ((value != null) && !string.IsNullOrEmpty(value.Trim()))
            {
                string where = string.Format("{0}='{1}'", column, CleanValues(value));

                if (Where.Length == 0)
                    Where.Append(" WHERE " + where);
                else
                    Where.Append(" AND " + where);
            }
        }

        public void SelectOrder(string column, EnumOrder order)
        {
            Order.Clear();

            if (Order.Length == 0)
                Order.Append(" ORDER BY " + column);
            else
                Order.Append(", " + column);

            if (Order.Length != 0)
            {
                switch (order)
                {
                    case EnumOrder.ASCENDING:
                        Order.Append(" ASC");
                        break;
                    case EnumOrder.DESCENDING:
                        Order.Append(" DESC");
                        break;
                }
            }
        }

        #endregion

        #region ----- Insert Query -----

        public void InsertValue(string column, string value)
        {
            if (Columns.Length == 0)
            {
                Columns.Append(column);
                Value.Append(string.Format("'{0}'", CleanValues(value)));
            }
            else
            {
                Columns.Append("," + column);
                Value.Append(string.Format(", '{0}'", CleanValues(value)));
            }
        }

        public bool InsertQuery()
        {
            Query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", Table, Columns, Value);
            Clear();
            bool result = ExecuteNonQuery();

            InsertLog(Table, SelectLastID(), "", "", "INSERT");
            return result;
        }

        #endregion

        #region ----- Updated Query -----

        public void UpdateValue(string column, string value)
        {
            if (value == null)
            {
                return;
            }

            if (Columns.Length == 0)
            {
                Columns.Append(string.Format("{0} = '{1}'", column, CleanValues(value)));
            }
            else
            {
                Columns.Append(string.Format(", {0} = '{1}'", column, CleanValues(value)));
            }
        }

        public bool UpdateQuery(string ID)
        {
            if (Columns.Length != 0)
            {
                string action = Columns.ToString();

                Query = string.Format("UPDATE {0} SET {1} WHERE ID = '{2}'", Table, action, ID);
                Clear();

                ExecuteNonQuery();
                return true;
            }
            else
                return false;
        }

        public bool UpdateSecureQuery(string ID)
        {
            if (Columns.Length != 0)
            {
                string action = Columns.ToString();

                Query = string.Format("UPDATE {0} SET {1} WHERE secureid = '{2}'", Table, action, ID);
                Clear();

                if (ExecuteNonQuery())
                {
                    if (action == "deleted = '1'")
                        InsertLog(Table, ID, "", "", "DELETE");
                    else if (action == "deleted = '0'")
                        InsertLog(Table, ID, "", "", "UNDELETE");
                    else
                    {
                        foreach (ColumnUpdate update in Updates)
                            InsertLog(Table, ID, update.column, update.value, "UPDATE");
                    }

                    Updates.Clear();

                    return true;
                }

                return false;
            }
            else
                return false;
        }

        public bool UpdateQuery()
        {
            Query = string.Format("UPDATE {0} SET {1} WHERE {2}", Table, Columns, Where);
            Clear();
            return ExecuteNonQuery();
        }

        #endregion

        #region ----- Delete Query -----

        public bool DeleteQuery(string ID, string value)
        {
            Query = string.Format("UPDATE {0} SET deleted={1}  WHERE ID = '{2}'", Table, value, ID);
            Clear();

            ExecuteNonQuery();

            InsertLog(Table, ID, "deleted", value, "DELETE");
            return true;
        }

        #endregion

        #region ----- Additional Functionalities -----

        public virtual int Count()
        {
            Query = string.Format("SELECT COUNT(*) FROM {0}", Table);
            DataTable data = ExecuteQuery();

            if (data.Rows.Count != 0)
                return int.Parse(data.Rows[0][0].ToString());
            else
                return 0;
        }

        public int CountQuery()
        {
            Query = string.Format("SELECT COUNT(*) FROM {1}{2}{3}{4}", 0, Table, Where.ToString(), Order.ToString(), Limit.ToString(), Environment.NewLine);
            Clear();

            DataTable data = ExecuteQuery();

            if (data.Rows.Count != 0)
                return int.Parse(data.Rows[0][0].ToString());
            else
                return 0;
        }

        public DataTable Search(string rawfilter, int page = 0)
        {
            if (page == 0 && rawfilter.Trim() == "")
            {
                Query = string.Format("SELECT * FROM {0}", Table);
                return ExecuteQuery();
            }
            else
            {
                SelectPage(page);

                if (rawfilter != null && rawfilter.Trim() != "")
                {
                    StringBuilder filters = new StringBuilder();
                    DataTable columns = ShowColumns();

                    foreach (DataRow row in columns.Rows)
                    {
                        if (filters.Length == 0)
                            filters.AppendFormat("{0} LIKE '%{1}%'", row[0].ToString(), rawfilter);
                        else
                            filters.AppendFormat(" OR {0} LIKE '%{1}%'", row[0].ToString(), rawfilter);
                    }

                    SelectFilter(filters.ToString());
                }

                return SelectQuery();
            }
        }

        public int SearchCount(string rawfilter)
        {
            DataTable data = null;

            StringBuilder filters = new StringBuilder();
            DataTable columns = ShowColumns();

            foreach (DataRow row in columns.Rows)
            {
                if (filters.Length == 0)
                    filters.AppendFormat("{0} LIKE '%{1}%'", row[0].ToString(), rawfilter);
                else
                    filters.AppendFormat(" OR {0} LIKE '%{1}%'", row[0].ToString(), rawfilter);
            }

            filters.Append(" AND deleted='0'");

            Query = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}", Table, filters.ToString());
            data = ExecuteQuery();

            if (data.Rows.Count != 0)
                return int.Parse(data.Rows[0][0].ToString());
            else
                return 0;
        }

        public string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public string GetCurrentDateTime()
        {
            return FormatDateTime(DateTime.Now);
        }

        public string FormatDateTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string FormatDateTime(string date)
        {
            DateTime datetime = DateTime.Parse(date);
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetMonth(int month)
        {
            string[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return months[month - 1];
        }

        public static string CleanValues(string value)
        {
            if ((value != null))
            {
                value = value.Replace("\\", "\\\\");
                value = value.Replace("'", "\\'");
                value = value.Replace("%", "\\%");
                value = value.Replace(",", "\\,");

                value = value.Replace((char)(0), '\0');
                value = value.Replace((char)(8), '\b');
                value = value.Replace((char)(9), '\t');
                value = value.Replace((char)(10), '\n');
                value = value.Replace((char)(13), '\r');

                value = value.Replace("\\r", "");
                value = value.Replace("\\n", "");

                value = value.Replace("]", ">");
                value = value.Replace("[/", "</");
                value = value.Replace("[", "<");

                return value;
            }
            return "";
        }

        #endregion

        #region ----- Supporting Functions -----

        private void InsertLog(string table, string id, string column, string value, string action)
        {
            if (column.ToLower() == "deleted")
            {
                if (value == "0")
                    action = "DELETE";
                else
                    action = "RESTORE";
            }

            value = CleanValues(value);

            string columns = "tablename,tableid,columnname,value,action,user_id,username,logdate";
            string values = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", table, id, column, value, action, UserID, Username, FormatDateTime(DateTime.Now));

            Query = string.Format("INSERT INTO _logs ({0}) VALUES ({1})", columns, values);
            ExecuteNonQuery();
        }

        public void Clear()
        {
            Columns.Clear();
            Value.Clear();

            Where.Clear();
            Order.Clear();
            Limit.Clear();
        }

        #endregion
    }
}