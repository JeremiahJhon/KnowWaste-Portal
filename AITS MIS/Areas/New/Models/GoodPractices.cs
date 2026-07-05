using System;
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

        public Pagination PaginationSetting { get; set; } = new Pagination();

        public GoodPractices() {
            ID = 0;
            CountryID = 0;
            Refresh(); 
        }

        public GoodPractices(int id) { 
            ID = id;
            Refresh(); 
        }

        public GoodPractices(int id, int countryID, string searchText, int pageIndex) { 
            ID = id;
            CountryID = countryID;
            PaginationSetting.SearchText = searchText;
            PaginationSetting.PageIndex = pageIndex;
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
                                ResultsAchieved = a.ResultsArchieved,
                                ChallengesLessonsLearned = a.ChallengesLessonLearned,
                                Replicability = a.Replicability,
                                Sources = a.Sources,
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
                                ResultsAchieved = a.ResultsArchieved,
                                ChallengesLessonsLearned = a.ChallengesLessonLearned,
                                Replicability = a.Replicability,
                                Sources = a.Sources,
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
                                                 ResultsAchieved = a.ResultsArchieved,
                                                 ChallengesLessonsLearned = a.ChallengesLessonLearned,
                                                 Replicability = a.Replicability,
                                                 Sources = a.Sources,
                                                 Country = b.Name,
                                                 CountryID = b.ID,
                                                 Thumbnail = a.Photo
                                             }).Take(4).ToList();
            }

            Countries = BlogList
                        .GroupBy(a => new { a.CountryID, a.Country })
                        .Select(g => new ViewModels.Country
                        {
                            ID = g.Key.CountryID,
                            Name = g.Key.Country
                        })
                        .ToList();

            // Apply pagination and search
            // Get base query
            var query = BlogList.AsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(PaginationSetting.SearchText))
            {
                query = query.Where(p => p.Title.ToLower().Contains(PaginationSetting.SearchText.ToLower()));
            }

            // Compute total count BEFORE pagination
            PaginationSetting.TotalCount = query.Count();

            // Apply pagination
            BlogList = query
                .Skip(PaginationSetting.PageIndex * PaginationSetting.PageCount)
                .Take(PaginationSetting.PageCount)
                .ToList();

            // Compute total pages
            PaginationSetting.TotalPages = (int)Math.Ceiling((double)PaginationSetting.TotalCount / PaginationSetting.PageCount);
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
        public string ResultsAchieved { get; set; }
        public string ChallengesLessonsLearned { get; set; }
        public string Replicability { get; set; }
        public string Sources { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public string Thumbnail { get; set; }
        public List<GoodPractice> RelatedTopics { get; set; }
    }
}