using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Data
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public int CountryID { get; set; }

        public int CityID { get; set; }

        public int WasteTypeID { get; set; }

        public int Year { get; set; }

        public List<ViewModels.Data> DataList { get; set; }

        public List<Country> Countries { get; set; }

        public List<ViewModels.City> Cities { get; set; }

        public List<WasteType> WasteTypes { get; set; }

        public List<int> Years { get; set; }


        public Data(int countryID, int cityID, int wasteTypeID, int year)
        {
            CountryID = countryID;
            CityID = cityID;
            WasteTypeID = wasteTypeID;
            Year = year;
            Refresh();
        }

        public void Refresh()
        {
            var rawList = (from a in db.countrywastestreams
                           join b in db.countries on a.Country_ID equals b.ID.ToString()
                           join d in db.wastecategories on a.Wastecategory_ID equals d.ID.ToString()
                           join c in db.cities on b.ID equals c.ID into cityGroup
                           from c in cityGroup.DefaultIfEmpty()
                           join e in db.subregions on b.SubRegion_ID equals e.ID.ToString()
                           where a.Deleted == false
                                 && (d.ID == 1 || d.ID == 14)
                                 && e.Region_id == "3" //Asian Countries
                                 && b.Deleted == 0
                           select new { a, b, c, d }).ToList();

            DataList = rawList.Select(x => new ViewModels.Data
            {
                ID = x.a.ID,
                CountryID = x.b.ID,
                Country = x.b.Name,

                CityID = x.c?.ID ?? 0,
                City = x.c?.Name ?? "",

                WasteTypeID = x.d.ID,
                WasteType = x.d.Name,

                Year = string.IsNullOrWhiteSpace(x.a.Year) ? 0 : Convert.ToInt32(x.a.Year),

                TotalGenerated = x.a.Totalgenerated,
                Hazardous = x.a.Hazardous,
                TotalCollected = x.a.Totalcollected,
                Recycled = x.a.Recycled,
                Recovered = x.a.Recovered,
                Disposal = x.a.Disposal,
                Treatment = x.a.Treatment,
                Reuse = x.a.Reuse,
                Sludge = x.a.Sludge,
                Reference = x.a.reference,
            }).ToList();

            Countries = DataList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .OrderBy(x => x.Name)
                        .Distinct()
                        .ToList();

            if (CountryID > 0)
            {
                Cities = new KnowWaste.Models.City(CountryID).Cities;
            }
            else
            {
                Cities = new KnowWaste.Models.City().Cities;
            }

            WasteTypes = DataList
                        .GroupBy(a => new { a.WasteTypeID, a.WasteType })
                        .Select(g => new ViewModels.WasteType
                        {
                            ID = g.Key.WasteTypeID,
                            Name = g.Key.WasteType
                        })
                        .ToList();

            Years = DataList.Select(p => p.Year).Distinct().OrderByDescending(a => a).ToList();

            if (CountryID > 0)
            {
                DataList = DataList.Where(p => p.CountryID == CountryID).ToList();
            }

            if (CityID > 0)
            {
                DataList = DataList.Where(p => p.CityID == CityID).ToList();
            }

            if (WasteTypeID > 0)
            {
                DataList = DataList.Where(p => p.WasteTypeID == WasteTypeID).ToList();
            }

            if (Year > 0)
            {
                DataList = DataList.Where(p => p.Year == Year).ToList();
            }
        }
    }
}

namespace ViewModels
{
    public class Data
    {
        public int ID { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int WasteTypeID { get; set; }
        public string WasteType { get; set; }
        public int Year { get; set; }
        public decimal? TotalGenerated { get; set; }
        public decimal? Hazardous { get; set; }
        public decimal? TotalCollected { get; set; }
        public decimal? Recycled { get; set; }
        public decimal? Recovered { get; set; }
        public decimal? Disposal { get; set; }
        public decimal? Treatment { get; set; }
        public decimal? Reuse { get; set; }
        public decimal? Sludge { get; set; }
        public string Reference { get; set; }
    }
}