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

        public List<ViewModels.Expert> ExpertList { get; set; }

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
        }
    }
}

namespace ViewModels
{
}