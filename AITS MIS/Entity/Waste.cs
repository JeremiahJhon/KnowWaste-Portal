using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UCOnline.Entity
{
    public class Waste
    {
        public string Country { get; set; }
        public string ID { get; set; }
        public string Category { get; set; }
        public int Type { get; set; }
        public int Generated { get; set; }
        public int Hazardous { get; set; }
        public int Collected { get; set; }
        public int Recycled { get; set; }
        public int Recovered { get; set; }
        public int Disposal { get; set; }
        public int Treatment { get; set; }
        public int Reuse { get; set; }
        public int Sludge { get; set; }
        public int Year { get; set; }
    }
}