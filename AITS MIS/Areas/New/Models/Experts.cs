using System.Collections.Generic;
using System.Linq;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class Experts
    {
        private KnowWasteEntities db = new KnowWasteEntities();

        public List<Expert> ExpertList { get; set; }

        public Experts() { }

        public void GetData()
        {
            ExpertList = (from a in db.expertrosters
                        where a.Deleted == 0
                        select new Expert
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
    public class Expert
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
    }
}