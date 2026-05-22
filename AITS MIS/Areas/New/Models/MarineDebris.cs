using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class MarineDebris
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public List<GoodPractice> BlogList { get; set; }

        public List<ViewModels.Document> DocumentList { get; set; }

        public List<ViewModels.Document> _3RProMARList { get; set; }

        public List<ViewModels.Expert> ExpertList { get; set; }

        public List<ViewModels.News> NewsList { get; set; }

        public List<ViewModels.Technology> TechnologyList { get; set; }

        public MarineDebris() { }

        public void GetData()
        {
            BlogList = (from a in db.blogs
                        join b in db.countries on a.Country_ID equals b.ID
                        where a.Blogscategory_ID == 1 && a.Deleted == 0 && b.Deleted == 0
                        select new GoodPractice
                        {
                            ID = a.ID,
                            Title = a.Title,
                            Description = a.Description,
                            Country = b.Name,
                            Thumbnail = a.Photo,
                        }).ToList();

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

            TechnologyList = (from a in db.blogs
                              join b in db.countries on a.Country_ID equals b.ID
                              where a.Blogscategory_ID == 3
                                    && a.Deleted == 0
                                    && b.Deleted == 0
                              select new ViewModels.Technology
                              {
                                  ID = a.ID,
                                  Title = a.Title,
                                  Author = a.Author,
                                  Date = a.Blogsdate.ToString(),
                                  Description = a.Description,
                                  CountryID = b.ID,
                                  Country = b.Name,
                                  Thumbnail = a.Photo,
                                  Source = a.Sources,
                              }).ToList();

            _3RProMARList = (from a in db.documents
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
    }
}

namespace ViewModels
{
}