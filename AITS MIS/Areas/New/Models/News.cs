using System;
using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class News
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public List<ViewModels.News> NewsList { get; set; }

        public List<ViewModels.Event> EventsList { get; set; }

        public News() { }

        public void GetData(int? id)
        {
            if (id == null)
            {
                NewsList = (from a in db.news
                            join b in db.countries on a.Country_ID equals b.ID
                            where a.Deleted == 0 && b.Deleted == 0
                            select new ViewModels.News
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Country = b.Name,
                                Photo = a.Photo,
                                Description = a.Description,
                                Date = a.StartDate
                            }).ToList();

                EventsList = (from a in db.upcomingevents
                              join b in db.countries on a.Country_ID equals b.ID
                              where a.Deleted == 0 &&
                                      b.Deleted == 0 //&&
                                                     //a.Datestart <= DateTime.Now &&
                                                     //a.Dateend >= DateTime.Now
                              select new ViewModels.Event
                              {
                                  ID = a.ID,
                                  Title = a.Title,
                                  StartDate = a.StartDate,
                                  EndDate = a.EndDate,
                                  Country = b.Name,
                                  Thumbnail = a.Thumbnail,
                                  Description = a.Description,
                                  Detail = a.Detail,
                                  Location = a.Location
                              }).ToList();
            }
            else
            {
                NewsList = (from a in db.news
                            join b in db.countries on a.Country_ID equals b.ID
                            where a.Deleted == 0 && b.Deleted == 0 && a.ID == id
                            select new ViewModels.News
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Country = b.Name,
                                Photo = a.Photo,
                                Description = a.Description,
                                Date = a.StartDate
                            }).ToList();

                EventsList = (from a in db.upcomingevents
                              join b in db.countries on a.Country_ID equals b.ID
                              where a.Deleted == 0 &&
                                      b.Deleted == 0 //&&
                                                     //a.Datestart <= DateTime.Now &&
                                                     //a.Dateend >= DateTime.Now
                                    && a.ID == id
                              select new ViewModels.Event
                              {
                                  ID = a.ID,
                                  Title = a.Title,
                                  StartDate = a.StartDate,
                                  EndDate = a.EndDate,
                                  Country = b.Name,
                                  Thumbnail = a.Thumbnail,
                                  Description = a.Description,
                                  Detail = a.Detail,
                                  Location = a.Location
                              }).ToList();
            }
        }
    }
}

namespace ViewModels
{
    public class News
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Date { get; set; }
    }
}