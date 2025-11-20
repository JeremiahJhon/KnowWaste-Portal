using System;
using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class City
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public List<ViewModels.City> Cities { get; set; }

        public City() { Refresh(); }
        public City(int countryID) { 
            Refresh();
            Cities = Cities.Where(p => p.CountryID == countryID).ToList();
        }

        public void Refresh()
        {
            Cities = db.cities.Where(p => p.Deleted == 0).Select(a => new ViewModels.City { ID = a.ID, CountryID = a.Country_ID, Name = a.Name }).ToList();
        }
    }
}