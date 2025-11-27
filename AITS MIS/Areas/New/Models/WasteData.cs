using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class WasteData
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public int CountryID { get; set; }

        public int SubRegionID { get; set; }

        public int Year { get; set; }

        public List<ViewModels.Data> DataList { get; set; }

        public List<ViewModels.Policy> PolicyList { get; set; }

        public List<ViewModels.PopulationData> PopulationList { get; set; }

        public List<Country> Countries { get; set; }

        public List<SubRegion> SubRegions { get; set; }

        public List<WasteType> WasteTypes { get; set; }

        public List<int> Years { get; set; }


        public WasteData(int subregionID, int countryID, int year)
        {
            CountryID = countryID;
            SubRegionID = subregionID;
            Year = year;
            Refresh();
        }

        public void Refresh()
        {
            var rawList = (from a in db.countrywastestreams
                           join b in db.countries on a.Country_ID equals b.ID.ToString()
                           join c in db.wastecategories on a.Wastecategory_ID equals c.ID.ToString()
                           where a.Deleted == false
                                 && (c.ID == 1 || c.ID == 14)
                           select new { a, b, c }).ToList();

            DataList = rawList.Select(x => new ViewModels.Data
            {
                ID = x.a.ID,
                CountryID = x.b.ID,
                Country = x.b.Name,

                CityID = 0,
                City = "",

                WasteTypeID = x.c.ID,
                WasteType = x.c.Name,

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

            PopulationList = db.countrypopulations
                .Where(p => p.Deleted == 0)
                .Select(p => new ViewModels.PopulationData { 
                    ID = p.ID,
                    Year = p.Year ?? 0,
                    CountryID = p.Country_ID ?? 0,
                    Population = p.Population,
                    UrbanPopulation = p.Urbanpopulation,
                    Area = p.Area,
                    IncomeLevel = p.Incomelevel,
                    Description = p.Description,
                    Source = p.Source,
                }).ToList();

            PolicyList = (from a in db.countrypolicies
                        join b in db.countries on a.Country_ID equals b.ID.ToString()
                        join c in db.countrypolicy_area on a.Area_ID equals c.ID
                        where a.Deleted == 0 && b.Deleted == 0 && a.WasteCategory_ID == 14
                        select new ViewModels.Policy
                        {
                            ID = a.ID,
                            Legal = a.Legal,
                            CountryID = b.ID,
                            Country = b.Name,
                            PolicyAreaID = c.ID,
                            PolicyArea = c.Name,
                            Year = a.Year,
                            Description = a.Description,
                            Source = a.Link
                        }).ToList();

            Countries = DataList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .OrderBy(x => x.Name)
                        .ToList();

            WasteTypes = db.wastecategories.Where(p => p.Deleted == 0 && p.Parent == 0).Select(a => new ViewModels.WasteType
            {
                ID = a.ID,
                Name = a.Name,
            }).ToList();

            var countryIds = Countries.Select(p => p.ID).ToList();
            SubRegions = (from a in db.subregions
                          where countryIds.Contains(a.ID) && a.Deleted == 0
                          select new ViewModels.SubRegion { 
                            ID = a.ID,
                            Name = a.Name,
                          }).Distinct().ToList();

            Years = DataList.Select(p => p.Year).Distinct().OrderByDescending(a => a).ToList();

            if (CountryID > 0)
            {
                DataList = DataList.Where(p => p.CountryID == CountryID).ToList();
                PopulationList = PopulationList.Where(p => p.CountryID == CountryID).ToList();
                PolicyList = PolicyList.Where(p => p.CountryID == CountryID).ToList();
            }

            if (Year > 0)
            {
                DataList = DataList.Where(p => p.Year == Year).ToList();
                PopulationList = PopulationList.Where(p => p.Year == Year).ToList();
                PolicyList = PolicyList.Where(p => p.Year == Year.ToString()).ToList();
            }
        }
    }
}

namespace ViewModels
{
    public class PopulationData
    {
        public int ID { get; set; }
        public int CountryID { get; set; }
        public int Year { get; set; }
        public string Population { get; set; }
        public string UrbanPopulation { get; set; }
        public string Area { get; set; }
        public string IncomeLevel { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }
    public class WasteData
    {
        public string WasteCategory { get; set; }
        public decimal Generated { get; set; }
        public decimal Hazardous { get; set; }
        public decimal Collected { get; set; }
        public decimal Recycled { get; set; }
        public decimal Recovered { get; set; }
        public decimal Disposal { get; set; }
        public decimal Treatment { get; set; }
        public decimal Reuse { get; set; }
        public decimal Sludge { get; set; }
        public string Ref { get; set; }
    }
}