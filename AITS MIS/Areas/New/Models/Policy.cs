using System;
using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Policy
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public int CountryID { get; set; }

        public List<string> Keywords { get; set; }

        public List<ViewModels.Policy> Policies { get; set; }

        public List<Country> Countries { get; set; }

        public List<PolicyArea> Areas { get; set; }

        public Pagination PaginationSetting { get; set; } = new Pagination();

        public Policy()
        {
            CountryID = 0;
            RefreshData();
        }

        public Policy(int countryID, List<string> keywords)
        {
            CountryID = countryID;
            Keywords = keywords;
            RefreshData();
        }

        public Policy(int countryID, List<string> keywords, string searchText, int pageIndex)
        {
            CountryID = countryID;
            Keywords = keywords;            
            PaginationSetting.SearchText = searchText;
            PaginationSetting.PageIndex = pageIndex;
            RefreshData();
        }

        public void RefreshData()
        {
            Policies = (from a in db.countrypolicies
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

            if (CountryID > 0)
            {
                Policies = Policies.Where(p => p.CountryID == CountryID).ToList();
            }

            if (Keywords.Count > 0)
            {
                Policies = Policies.Where(p => Keywords.Contains(p.PolicyArea)).ToList();
            }

            Countries = Policies
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .ToList();

            Areas = Policies
                        .GroupBy(a => new { a.PolicyAreaID, a.PolicyArea })
                        .Select(g => new ViewModels.PolicyArea
                        {
                            ID = g.Key.PolicyAreaID,
                            Name = g.Key.PolicyArea
                        })
                        .ToList();

            // Apply pagination and search
            // Get base query
            var query = Policies.AsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(PaginationSetting.SearchText))
            {
                query = query.Where(p => p.Legal.ToLower().Contains(PaginationSetting.SearchText.ToLower()));
            }

            // Compute total count BEFORE pagination
            PaginationSetting.TotalCount = query.Count();

            // Apply pagination
            Policies = query
                .Skip(PaginationSetting.PageIndex * PaginationSetting.PageCount)
                .Take(PaginationSetting.PageCount)
                .ToList();

            // Compute total pages
            PaginationSetting.TotalPages = (int)Math.Ceiling((double)PaginationSetting.TotalCount / PaginationSetting.PageCount);
        }
    }
}

namespace ViewModels
{
    public class Policy
    {
        public int ID { get; set; }
        public string Legal { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public int PolicyAreaID { get; set; }
        public string PolicyArea { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }
}