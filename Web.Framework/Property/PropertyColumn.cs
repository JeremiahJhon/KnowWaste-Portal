using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.Framework.Enums;

namespace Web.Framework.Property
{
    public abstract class PropertyColumn
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public EnumInputType Type { get; set; }
        public EnumFormatType FormatType { get; set; }
        public bool ReadOnly { get; set; }

        public bool Visible { get; set; }

        public PropertyColumn(string name)
        {
            Name = name;
            Caption = name;
            ReadOnly = false;
            Visible = true;
            FormatType = EnumFormatType.GENERAL;
        }

        public PropertyColumn(string name, string caption)
        {
            Name = name;
            Caption = caption;
            ReadOnly = false;
            Visible = true;
            FormatType = EnumFormatType.GENERAL;
        }

        public virtual string GenerateInput(string value = "")
        {
            return "";
        }

        public virtual string FormatDisplay(string table, string id, DataRow row, string column)
        {
            return "";
        }
    }
}