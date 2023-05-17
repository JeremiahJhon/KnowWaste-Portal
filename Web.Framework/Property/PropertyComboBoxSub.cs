using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Web.Framework.Enums;
using Web.Framework.General;
using Web.Framework.Server;

namespace Web.Framework.Property
{
    public class PropertyComboBoxSub : PropertyComboBox
    {
        public PropertyComboBoxSub(string name) : base(name)
        {
            Type = EnumInputType.COMBOBOXSUB;
        }

        public PropertyComboBoxSub(string name, string caption) : base(name)
        {
            Type = EnumInputType.COMBOBOXSUB;
        }
    }
}