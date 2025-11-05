using System;
using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Events
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public List<ViewModels.Event> EventsList { get; set; }

        public Events() { }

        public void GetData()
        {
            EventsList = (from a in db.upcomingevents
                        join b in db.countries on a.Country_ID equals b.ID
                        where a.Deleted == 0 && 
                                b.Deleted == 0  &&
                                Convert.ToDateTime(a.StartDate) <= DateTime.Now &&
                                Convert.ToDateTime(a.EndDate) >= DateTime.Now
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

namespace ViewModels
{
    public class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string Thumbnail { get; set; }
        public string Organizer { get; set; }
        public string Location { get; set; }
    }
}