//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UCOnline.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class expertroster
    {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Position { get; set; }
        public string Organization { get; set; }
        public string Thumbnail { get; set; }
        public string Expertise { get; set; }
        public string Country_id { get; set; }
        public Nullable<int> Expertscategory_ID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Viewsexpertrosters_ID { get; set; }
        public Nullable<System.DateTime> Dateentered { get; set; }
        public Nullable<byte> Deleted { get; set; }
        public string Social_Facebook { get; set; }
        public string Social_Twitter { get; set; }
        public string Social_Dribble { get; set; }
    }
}
