using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.UI.WebControls;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Documents
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public int ID { get; set; }

        public int CountryID { get; set; } 

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string Area { get; set; }

        public List<Document> DocumentList { get; set; }

        public List<Country> Countries { get; set; }

        public List<string> Years { get; set; }
        
        public List<string> Publishers { get; set; }

        public List<string> Areas { get; set; }

        public Pagination PaginationSetting { get; set; } = new Pagination();

        public Documents(string type, int id) {
            this.ID = id;
            this.CountryID = 0;
            this.Year = 0;
            this.Publisher = "All";
            this.Area = "";

            switch (type)
            {
                case "3Rpromar":
                    Refresh3RpromarData();
                    break;
                case "Publications":
                    RefreshPublicationData();
                    break;
            }
        }

        public Documents(string type, int countryID, int year, string publisher, string searchText, int pageIndex)
        {
            this.ID = 0;
            this.CountryID = countryID;
            this.Year = year;
            this.Publisher = publisher;
            this.Area = "";
            PaginationSetting.SearchText = searchText;
            PaginationSetting.PageIndex = pageIndex;

            switch (type)
            {
                case "3Rpromar":
                    Refresh3RpromarData();
                    break;
                case "Publications":
                    RefreshPublicationData();
                    break;
            }
        }

        public Documents(string area)
        {
            this.ID = 0;
            this.CountryID = 0;
            this.Year = 0;
            this.Publisher = "All";
            this.Area = area;

            RefreshThematicData();
        }

        public void Refresh3RpromarData()
        {
            try
            {
                if (ID > 0)
                {
                    DocumentList = (from a in db.documents
                                    join b in db.countries on a.Country_ID equals b.ID.ToString()
                                    join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                    join d in db.geothemes on a.Geotheme_ID equals d.ID
                                    where (a.Documentcategory_ID == 4 || a.Is3rpromar == true) && a.Deleted == 0 && b.Deleted == 0 && a.ID == ID
                                    orderby a.Year descending
                                    select new Document
                                    {
                                        ID = a.ID,
                                        Title = a.Title,
                                        Country = (from x in db.countries where a.Country_ID.Contains(x.ID.ToString()) select new ViewModels.Country { ID = x.ID, Name = x.Name }).ToList(),
                                        Year = a.Year,
                                        Publisher = a.Publisher,
                                        CategoryID = c.ID,
                                        Category = c.Name,
                                        GeoTheme = d.Name,
                                        Keywords = a.Keyword,
                                        Description = a.Description,
                                        Thumbnail = a.Thumbnail,
                                        Attachment = a.Attachment,
                                        Source = a.Datasource,
                                    }).ToList();
                }
                else
                {
                    DocumentList = (from a in db.documents
                                    join b in db.countries on a.Country_ID equals b.ID.ToString()
                                    join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                    join d in db.geothemes on a.Geotheme_ID equals d.ID
                                    where (a.Documentcategory_ID == 4 || a.Is3rpromar == true) && a.Deleted == 0 && b.Deleted == 0
                                    orderby a.Year descending
                                    select new Document
                                    {
                                        ID = a.ID,
                                        Title = a.Title,
                                        Country = (from x in db.countries where a.Country_ID.Contains(x.ID.ToString()) select new ViewModels.Country { ID = x.ID, Name = x.Name }).ToList(),
                                        Year = a.Year,
                                        Publisher = a.Publisher,
                                        CategoryID = c.ID,
                                        Category = c.Name,
                                        GeoTheme = d.Name,
                                        Keywords = a.Keyword,
                                        Description = a.Description,
                                        Thumbnail = a.Thumbnail,
                                        Attachment = a.Attachment,
                                        Source = a.Datasource,
                                    }).ToList();
                }


                Countries = new List<Country>();
                foreach (ViewModels.Document item in DocumentList)
                {
                    if (item.Country.Count > 0)
                    {
                        Countries.AddRange(item.Country);
                    }
                }

                Countries = Countries
                            .GroupBy(c => new { c.ID, c.Name })
                            .Select(g => g.First())
                            .OrderBy(c => c.Name)
                            .ToList();

                if (CountryID > 0)
                {
                    DocumentList = DocumentList.Where(p => p.Country.Select(x => x.ID).ToList().Contains(CountryID)).ToList();
                }

                Years = DocumentList.Select(p => p.Year).Distinct().OrderByDescending(a => a).ToList();

                if (!Years.Contains(Year.ToString()))
                {
                    Year = 0;
                }

                if (Year > 0)
                {
                    DocumentList = DocumentList.Where(p => p.Year == Year.ToString()).ToList();
                }

                Publishers = DocumentList.Select(p => p.Publisher).Distinct().ToList();

                if (!Publishers.Contains(Publisher.ToString()))
                {
                    Publisher = "All";
                }

                if (Publisher != "All")
                {
                    DocumentList = DocumentList.Where(p => p.Publisher == Publisher).ToList();
                }

                // Apply pagination and search
                // Get base query
                var query = DocumentList.AsQueryable();

                // Apply search
                if (!string.IsNullOrWhiteSpace(PaginationSetting.SearchText))
                {
                    query = query.Where(p => p.Title.ToLower().Contains(PaginationSetting.SearchText.ToLower()));
                }

                // Compute total count BEFORE pagination
                PaginationSetting.TotalCount = query.Count();

                // Apply pagination
                //DocumentList = query
                //    .Skip(PaginationSetting.PageIndex * PaginationSetting.PageCount)
                //    .Take(PaginationSetting.PageCount)
                //    .ToList();

                // Compute total pages
                PaginationSetting.TotalPages = (int)Math.Ceiling((double)PaginationSetting.TotalCount / PaginationSetting.PageCount);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RefreshPublicationData()
        {
            try { 
                if (ID > 0)
                {
                    DocumentList = (from a in db.documents
                                    join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                    join d in db.geothemes on a.Geotheme_ID equals d.ID
                                    where (a.Documentcategory_ID == 4 || a.Publisher.Contains("RRC.AP") || a.Publisher.Contains("ERIA") || a.Publisher.Contains("NIVA") || a.Publisher.Contains("GIZ") || a.IsPublications == true) && a.Deleted == 0 && a.ID == ID
                                    orderby a.Year descending
                                    select new Document
                                    {
                                        ID = a.ID,
                                        Title = a.Title,
                                        Country = (from x in db.countries where a.Country_ID.Contains(x.ID.ToString()) select new ViewModels.Country { ID = x.ID, Name = x.Name }).ToList(),
                                        Year = a.Year,
                                        Publisher = a.Publisher,
                                        CategoryID = c.ID,
                                        Category = c.Name,
                                        GeoTheme = d.Name,
                                        Keywords = a.Keyword,
                                        Description = a.Description,
                                        Thumbnail = a.Thumbnail,
                                        Attachment = a.Attachment,
                                        Source = a.Datasource,
                                    }).ToList();
                }
                else
                {
                    DocumentList = (from a in db.documents
                                    join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                    join d in db.geothemes on a.Geotheme_ID equals d.ID
                                    where (a.Documentcategory_ID == 4 || a.Publisher.Contains("RRC.AP") || a.Publisher.Contains("ERIA") || a.Publisher.Contains("NIVA") || a.Publisher.Contains("GIZ") || a.IsPublications == true) && a.Deleted == 0
                                    orderby a.Year descending
                                    select new Document
                                    {
                                        ID = a.ID,
                                        Title = a.Title,
                                        Country = (from x in db.countries where a.Country_ID.Contains(x.ID.ToString()) select new ViewModels.Country { ID = x.ID, Name = x.Name }).ToList(),
                                        Year = a.Year,
                                        Publisher = a.Publisher,
                                        CategoryID = c.ID,
                                        Category = c.Name,
                                        GeoTheme = d.Name,
                                        Keywords = a.Keyword,
                                        Description = a.Description,
                                        Thumbnail = a.Thumbnail,
                                        Attachment = a.Attachment,
                                        Source = a.Datasource,
                                    }).ToList();
                }

                Countries = new List<Country>();
                foreach (ViewModels.Document item in DocumentList)
                {
                    if(item.Country.Count > 0)
                    {
                        Countries.AddRange(item.Country);
                    }
                }

                Countries = Countries
                            .GroupBy(c => new { c.ID, c.Name })
                            .Select(g => g.First())
                            .OrderBy(c => c.Name)
                            .ToList();

                if (CountryID > 0)
                {
                    DocumentList = DocumentList.Where(p => p.Country.Select(x => x.ID).ToList().Contains(CountryID)).ToList();
                }

                Years = DocumentList.Select(p => p.Year).Distinct().OrderByDescending(a => a).ToList();

                if (!Years.Contains(Year.ToString()))
                {
                    Year = 0;
                }

                if (Year > 0)
                {
                    DocumentList = DocumentList.Where(p => p.Year == Year.ToString()).ToList();
                }

                Publishers = DocumentList.Select(p => p.Publisher).Distinct().ToList();

                if (!Publishers.Contains(Publisher.ToString()))
                {
                    Publisher = "All";
                }

                if (Publisher != "All")
                {
                    DocumentList = DocumentList.Where(p => p.Publisher == Publisher).ToList();
                }

                // Apply pagination and search
                // Get base query
                var query = DocumentList.AsQueryable();

                // Apply search
                if (!string.IsNullOrWhiteSpace(PaginationSetting.SearchText))
                {
                    query = query.Where(p => p.Title.ToLower().Contains(PaginationSetting.SearchText.ToLower()));
                }

                // Compute total count BEFORE pagination
                PaginationSetting.TotalCount = query.Count();

                // Apply pagination
                //DocumentList = query
                //    .Skip(PaginationSetting.PageIndex * PaginationSetting.PageCount)
                //    .Take(PaginationSetting.PageCount)
                //    .ToList();

                // Compute total pages
                PaginationSetting.TotalPages = (int)Math.Ceiling((double)PaginationSetting.TotalCount / PaginationSetting.PageCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RefreshThematicData()
        {
            DocumentList = (from a in db.documents
                            join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                            join d in db.geothemes on a.Geotheme_ID equals d.ID
                            where a.Documentcategory_ID != 4 && a.Deleted == 0
                            orderby a.Year descending
                            select new Document
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Country = (from x in db.countries where a.Country_ID.Contains(x.ID.ToString()) select new ViewModels.Country { ID = x.ID, Name = x.Name }).ToList(),
                                Year = a.Year,
                                Publisher = a.Publisher,
                                CategoryID = c.ID,
                                Category = c.Name,
                                GeoTheme = d.Name,
                                Keywords = a.Keyword,
                                Description = a.Description,
                                Thumbnail = a.Thumbnail,
                                Attachment = a.Attachment,
                                Source = a.Datasource,
                            }).ToList();

            if (CountryID > 0)
            {
                DocumentList = DocumentList.Where(p => p.Country.Select(x => x.ID).ToList().Contains(CountryID)).ToList();
            }

            if (Year > 0)
            {
                DocumentList = DocumentList.Where(p => p.Year == Year.ToString()).ToList();
            }

            if (Publisher != "All")
            {
                DocumentList = DocumentList.Where(p => p.Publisher == Publisher).ToList();
            }

            Countries = new List<Country>();
            foreach (ViewModels.Document item in DocumentList)
            {
                if (item.Country.Count > 0)
                {
                    Countries.AddRange(item.Country);
                }
            }

            Countries = Countries
                        .GroupBy(c => new { c.ID, c.Name })
                        .Select(g => g.First())
                        .OrderBy(c => c.Name)
                        .ToList();

            Years = DocumentList.Select(p => p.Year).Distinct().OrderByDescending(a => a).ToList(); 
            Publishers = DocumentList.Select(p => p.Publisher).Distinct().ToList();
            Areas = DocumentList.Select(p => p.GeoTheme).Distinct().ToList();

            if (string.IsNullOrEmpty(Area))
            {
                Area = Areas.First();
            }
            DocumentList = DocumentList.Where(p => p.GeoTheme == Area).ToList();
        }
    }
}

namespace ViewModels
{
    public class Document
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<ViewModels.Country> Country { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string GeoTheme { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Attachment { get; set; }
        public string Source { get; set; }
    }
}