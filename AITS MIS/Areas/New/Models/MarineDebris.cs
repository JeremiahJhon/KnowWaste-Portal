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
        }
    }
}

namespace ViewModels
{
}