using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Home
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public List<ViewModels.GoodPractice> BlogList { get; set; }

        public List<ViewModels.Document> DocumentList { get; set; }

        public List<ViewModels.Expert> ExpertList { get; set; }

        public List<ViewModels.News> NewsList { get; set; }

        public Home() { }

        public void GetData()
        {
            BlogList = (from a in db.blogs
                        join b in db.countries on a.Country_ID equals b.ID
                        where a.Blogscategory_ID == 1 && a.Deleted == 0 && b.Deleted == 0
                        select new ViewModels.GoodPractice
                        {
                            ID = a.ID,
                            Title = a.Title,
                            Description = a.Description,
                            Country = b.Name,
                            Thumbnail = a.Photo,
                        }).ToList();

            var allCountriesForMatch = db.countries.Where(x => x.Deleted == 0).ToList();

            // _3RProMARList's criteria (CategoryID == 4 || Is3rpromar) is a subset of
            // DocumentList's broader criteria, so fetch the union of both once, build
            // each Document (including the country match) exactly once, then derive
            // both lists by filtering the shared in-memory set — instead of hitting
            // the database and rebuilding the same documents twice.
            var rawDocuments = (from a in db.documents
                                join c in db.documentcategories on a.Documentcategory_ID equals c.ID
                                join d in db.geothemes on a.Geotheme_ID equals d.ID
                                where a.Deleted == 0 &&
                                      (a.Documentcategory_ID == 4
                                       || a.Publisher.Contains("RRC.AP")
                                       || a.Publisher.Contains("ERIA")
                                       || a.Publisher.Contains("NIVA")
                                       || a.Publisher.Contains("GIZ")
                                       || a.IsPublications == true
                                       || a.Is3rpromar == true)
                                orderby a.Year descending
                                select new
                                {
                                    a.ID,
                                    a.Title,
                                    a.Country_ID,
                                    a.Year,
                                    a.Publisher,
                                    CategoryID = c.ID,
                                    Category = c.Name,
                                    GeoTheme = d.Name,
                                    a.Keyword,
                                    a.Description,
                                    a.Thumbnail,
                                    a.Attachment,
                                    a.Datasource,
                                    a.IsPublications,
                                    a.Is3rpromar,
                                }).ToList(); // materialized here — everything after this runs in memory, so Split() is fine

            var allDocuments = rawDocuments.Select(a => new
            {
                Raw = a,
                Document = new Document
                {
                    ID = a.ID,
                    Title = a.Title,
                    Country = allCountriesForMatch
                        .Where(x => !string.IsNullOrEmpty(a.Country_ID) &&
                                    a.Country_ID.Split(',')
                                        .Contains(x.ID.ToString()))
                        .Select(x => new ViewModels.Country { ID = x.ID, Name = x.Name })
                        .ToList(),
                    Year = a.Year,
                    Publisher = a.Publisher,
                    CategoryID = a.CategoryID,
                    Category = a.Category,
                    GeoTheme = a.GeoTheme,
                    Keywords = a.Keyword,
                    Description = a.Description,
                    Thumbnail = a.Thumbnail,
                    Attachment = a.Attachment,
                    Source = a.Datasource,
                }
            }).ToList();

            DocumentList = allDocuments
                .Where(x => x.Raw.CategoryID == 4
                         || x.Raw.Publisher.Contains("RRC.AP")
                         || x.Raw.Publisher.Contains("ERIA")
                         || x.Raw.Publisher.Contains("NIVA")
                         || x.Raw.Publisher.Contains("GIZ")
                         || x.Raw.IsPublications == true)
                .Select(x => x.Document)
                .ToList();

            ExpertList = (from a in db.expertrosters
                          where a.Deleted == 0
                          select new ViewModels.Expert
                          {
                              ID = a.ID,
                              Name = a.Firstname + " " + a.Lastname,
                              Description = a.Expertise,
                              Position = a.Position,
                              Thumbnail = a.Thumbnail,
                          }).ToList();

            NewsList = (from a in db.news
                        join b in db.countries on a.Country_ID equals b.ID
                        where a.Deleted == 0 && b.Deleted == 0 && a.Publish == true
                        select new ViewModels.News
                        {
                            ID = a.ID,
                            Title = a.Title,
                            Country = b.Name,
                            Photo = a.Photo,
                            Description = a.Description,
                            Date = a.StartDate
                        }).ToList();
        }
    }
}

namespace ViewModels
{
}