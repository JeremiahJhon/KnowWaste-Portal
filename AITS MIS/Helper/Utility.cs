using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace kNowaste.Helper
{
    public static class Utility
    {
        public static DataTable LinqToDataTable<T>(IEnumerable<T> items)
        {
            //Createa DataTable with the Name of the Class i.e. Customer class.
            DataTable dt = new DataTable(typeof(T).Name);

            //Read all the properties of the Class i.e. Customer class.
            PropertyInfo[] propInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //Loop through each property of the Class i.e. Customer class.
            foreach (PropertyInfo propInfo in propInfos)
            {
                //Add Columns in DataTable based on Property Name and Type.
                //dt.Columns.Add(new DataColumn(propInfo.Name, propInfo.PropertyType));
                dt.Columns.Add(new DataColumn(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
            }

            //Loop through the items if the Collection.
            foreach (T item in items)
            {
                //Add a new Row to DataTable.
                DataRow dr = dt.Rows.Add();

                //Loop through each property of the Class i.e. Customer class.
                foreach (PropertyInfo propInfo in propInfos)
                {
                    object value = propInfo.GetValue(item, null);

                    // Check if the value is null, and if so, use DBNull.Value
                    if (value == null)
                    {
                        dr[propInfo.Name] = DBNull.Value;
                    }
                    else
                    {
                        dr[propInfo.Name] = value;
                    }
                }
            }

            return dt;
        }
    }
}