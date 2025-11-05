using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class GoodPractices
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public int ID { get; set; }

        public int CountryID { get; set; }

        public List<GoodPractice> BlogList { get; set; }

        public List<Country> Countries { get; set; }

        public GoodPractices() {
            ID = 0;
            CountryID = 0;
            Refresh(); 
        }

        public GoodPractices(int id) { 
            ID = id;
            Refresh(); 
        }

        public GoodPractices(int id, int countryID) { 
            ID = id;
            CountryID = countryID;
            Refresh(); 
        }

        public void Refresh()
        {
            if(ID > 0)
            {
                BlogList = (from a in db.blogs
                            join b in db.countries on a.Country_ID equals b.ID
                            where a.Blogscategory_ID == 1
                                  && a.Deleted == 0
                                  && b.Deleted == 0
                                  && a.ID == ID
                            select new GoodPractice
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Author = a.Author,
                                Date = a.Blogsdate.ToString(),
                                Description = a.Description,
                                Country = b.Name,
                                CountryID = b.ID,
                                Thumbnail = a.Photo,
                            }).ToList();
            }
            else
            {
                BlogList = (from a in db.blogs
                            join b in db.countries on a.Country_ID equals b.ID
                            where a.Blogscategory_ID == 1
                                  && a.Deleted == 0
                                  && b.Deleted == 0
                            select new GoodPractice
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Author = a.Author,
                                Date = a.Blogsdate.ToString(),
                                Description = a.Description,
                                Country = b.Name,
                                CountryID = b.ID,
                                Thumbnail = a.Photo
                            }).ToList();
            }

            if(CountryID > 0)
            {
                BlogList = BlogList.Where(p => p.CountryID == CountryID).ToList();
            }

            if (BlogList.Count == 1)
            {
                BlogList[0].RelatedTopics = (from a in db.blogs
                                             join b in db.countries on a.Country_ID equals b.ID
                                             where a.Blogscategory_ID == 1
                                                   && a.Deleted == 0
                                                   && b.Deleted == 0
                                                   && a.ID != ID
                                             select new GoodPractice
                                             {
                                                 ID = a.ID,
                                                 Title = a.Title,
                                                 Author = a.Author,
                                                 Date = a.Blogsdate.ToString(),
                                                 Description = a.Description,
                                                 Country = b.Name,
                                                 CountryID = b.ID,
                                                 Thumbnail = a.Photo
                                             }).Take(3).ToList();
            }

            Countries = BlogList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .ToList();
        }
    }
}

namespace ViewModels
{
    public class GoodPractice
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public string Thumbnail { get; set; }
        public List<GoodPractice> RelatedTopics { get; set; }
    }
}