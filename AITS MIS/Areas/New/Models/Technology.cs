using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Technology
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public int ID { get; set; }

        public int CountryID { get; set; }

        public List<ViewModels.Technology> TechnologyList { get; set; }

        public List<Country> Countries { get; set; }

        public Technology(int id) {
            ID = id;
            CountryID = 0;
            Refresh();
        }

        public Technology(int id, int countryID)
        {
            ID = id;
            CountryID = countryID;
            Refresh();
        }

        public void Refresh()
        {
            if(ID > 0)
            {
                TechnologyList = (from a in db.blogs
                            join b in db.countries on a.Country_ID equals b.ID
                                  where a.Blogscategory_ID == 3
                                        && a.Deleted == 0
                                        && b.Deleted == 0
                                        && a.ID == ID
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
            }
            else
            {
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
            }

            if (CountryID > 0)
            {
                TechnologyList = TechnologyList.Where(p => p.CountryID == CountryID).ToList();
            }

            Countries = TechnologyList
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
    public class Technology
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public string Thumbnail { get; set; }
        public string Source { get; set; }
    }
}