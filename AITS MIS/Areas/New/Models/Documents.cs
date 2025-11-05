using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

        public Documents(string type, int countryID, int year, string publisher)
        {
            this.ID = 0;
            this.CountryID = countryID;
            this.Year = year;
            this.Publisher = publisher;
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
            if (ID > 0)
            {
                DocumentList = (from a in db.documents
                                join b in db.countries on a.Country_ID equals b.ID.ToString()
                                join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                join d in db.geothemes on a.Geotheme_ID equals d.ID
                                where (a.Documentcategory_ID == 4 || a.Is3rpromar == true) && a.Deleted == 0 && b.Deleted == 0 && a.ID == ID
                                select new Document
                                {
                                    ID = a.ID,
                                    Title = a.Title,
                                    Country = b.Name,
                                    CountryID = b.ID,
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
                                select new Document
                                {
                                    ID = a.ID,
                                    Title = a.Title,
                                    Country = b.Name,
                                    CountryID = b.ID,
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

            if (CountryID > 0)
            {
                DocumentList = DocumentList.Where(p => p.CountryID == CountryID).ToList();
            }

            if (Year > 0)
            {
                DocumentList = DocumentList.Where(p => p.Year == Year.ToString()).ToList();
            }

            if (Publisher != "All")
            {
                DocumentList = DocumentList.Where(p => p.Publisher == Publisher).ToList();
            }

            Countries = DocumentList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .ToList();

            Years = DocumentList.Select(p => p.Year).Distinct().ToList();
            Publishers = DocumentList.Select(p => p.Publisher).Distinct().ToList();
        }

        public void RefreshPublicationData()
        {
            if (ID > 0)
            {
                DocumentList = (from a in db.documents
                                join b in db.countries on a.Country_ID equals b.ID.ToString()
                                join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                join d in db.geothemes on a.Geotheme_ID equals d.ID
                                where (a.Documentcategory_ID == 4 || a.Publisher.Contains("RRC.AP") || a.Publisher.Contains("ERIA") || a.Publisher.Contains("NIVA") || a.Publisher.Contains("GIZ") || a.IsPublications == true) && a.Deleted == 0 && b.Deleted == 0 && a.ID == ID
                                select new Document
                                {
                                    ID = a.ID,
                                    Title = a.Title,
                                    Country = b.Name,
                                    CountryID = b.ID,
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
                                where (a.Documentcategory_ID == 4 || a.Publisher.Contains("RRC.AP") || a.Publisher.Contains("ERIA") || a.Publisher.Contains("NIVA") || a.Publisher.Contains("GIZ") || a.IsPublications == true) && a.Deleted == 0 && b.Deleted == 0
                                select new Document
                                {
                                    ID = a.ID,
                                    Title = a.Title,
                                    Country = b.Name,
                                    CountryID = b.ID,
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

            if (CountryID > 0)
            {
                DocumentList = DocumentList.Where(p => p.CountryID == CountryID).ToList();
            }

            if (Year > 0)
            {
                DocumentList = DocumentList.Where(p => p.Year == Year.ToString()).ToList();
            }

            if (Publisher != "All")
            {
                DocumentList = DocumentList.Where(p => p.Publisher == Publisher).ToList();
            }

            Countries = DocumentList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .ToList();

            Years = DocumentList.Select(p => p.Year).Distinct().ToList();
            Publishers = DocumentList.Select(p => p.Publisher).Distinct().ToList();
        }

        public void RefreshThematicData()
        {
            DocumentList = (from a in db.documents
                            join b in db.countries on a.Country_ID equals b.ID.ToString()
                            join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                            join d in db.geothemes on a.Geotheme_ID equals d.ID
                            where a.Documentcategory_ID != 4 && a.Deleted == 0 && b.Deleted == 0
                            select new Document
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Country = b.Name,
                                CountryID = b.ID,
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
                DocumentList = DocumentList.Where(p => p.CountryID == CountryID).ToList();
            }

            if (Year > 0)
            {
                DocumentList = DocumentList.Where(p => p.Year == Year.ToString()).ToList();
            }

            if (Publisher != "All")
            {
                DocumentList = DocumentList.Where(p => p.Publisher == Publisher).ToList();
            }

            Countries = DocumentList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .ToList();

            Years = DocumentList.Select(p => p.Year).Distinct().ToList(); 
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
        public int CountryID { get; set; }
        public string Country { get; set; }
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