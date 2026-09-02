using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UCOnline.Data;
using ViewModels;

namespace KnowWaste.Models
{
    public class News
    {
        private KnowWasteEntities db = new KnowWasteEntities();
        public int ID { get; set; }

        public List<ViewModels.News> NewsList { get; set; }

        public List<ViewModels.Event> EventsList { get; set; }

        public Pagination PaginationSetting { get; set; } = new Pagination();

        public News() {
            ID = 0;
            Refresh();
        }

        public News(int id)
        {
            ID = id;
            Refresh();
        }

        public News(int id, string searchText, int pageIndex)
        {
            ID = id;
            PaginationSetting.SearchText = searchText;
            PaginationSetting.PageIndex = pageIndex;
            Refresh();
        }

        public void Refresh()
        {
            if (ID > 0)
            {
                NewsList = (from a in db.news
                            join b in db.countries on a.Country_ID equals b.ID
                            where a.Deleted == 0
                                  && b.Deleted == 0
                                  && a.ID == ID
                            select new ViewModels.News
                            {
                                ID = a.ID,
                                Title = a.Title,
                                Country = b.Name,
                                Photo = a.Photo,
                                Description = a.Description,
                                Date = a.StartDate,
                            }).ToList();

                EventsList = db.upcomingevents
                           .Where(a => a.Deleted == 0 && a.ID == ID)
                           .Join(db.countries.Where(c => c.Deleted == 0),
                               a => a.Country_ID,
                               c => c.ID,
                               (a, c) => new { a, c })
                           .AsEnumerable() // switch to in-memory processing (because of Split)
                           .Where(x =>
                           {
                               var dates = x.a.StartDate.Split('-');
                               var start = DateTime.Parse(dates[0].Trim());
                               var end = DateTime.Parse(dates[1].Trim());
                               return start <= DateTime.Now && end >= DateTime.Now;
                           })
                           .Select(x => new ViewModels.Event
                           {
                               ID = x.a.ID,
                               Title = x.a.Title,
                               StartDate = x.a.StartDate,
                               EndDate = x.a.EndDate,
                               Country = x.c.Name,
                               Thumbnail = x.a.Thumbnail,
                               Description = x.a.Description,
                               Detail = x.a.Detail,
                               Location = x.a.Location
                           })
                           .OrderByDescending(p => p.StartDate)
                           .ToList();
            }
            else
            {
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
                                Date = a.StartDate,
                            }).ToList();

                EventsList = db.upcomingevents
                            .Where(a => a.Deleted == 0)
                            .Join(db.countries.Where(c => c.Deleted == 0),
                                a => a.Country_ID,
                                c => c.ID,
                                (a, c) => new { a, c })
                            .AsEnumerable() // switch to in-memory processing (because of Split)
                            .Where(x =>
                            {
                                var dates = x.a.StartDate.Split('-');
                                var start = DateTime.Parse(dates[0].Trim());
                                var end = DateTime.Parse(dates[1].Trim());
                                return start <= DateTime.Now && end >= DateTime.Now;
                            })
                            .Select(x => new ViewModels.Event
                            {
                                ID = x.a.ID,
                                Title = x.a.Title,
                                StartDate = x.a.StartDate,
                                EndDate = x.a.EndDate,
                                Country = x.c.Name,
                                Thumbnail = x.a.Thumbnail,
                                Description = x.a.Description,
                                Detail = x.a.Detail,
                                Location = x.a.Location
                            })
                            .OrderByDescending(p => p.StartDate)
                            .ToList();
            }

            foreach (var news in NewsList)
            {
                if (string.IsNullOrWhiteSpace(news.Date))
                {
                    continue;
                }

                var dates = news.Date.Split(
                    new[] { " - " },
                    StringSplitOptions.None
                );

                DateTime parsedDate;

                if (dates.Length > 0 &&
                    DateTime.TryParse(dates[0].Trim(), out parsedDate))
                {
                    news.StartDate = parsedDate;
                }
            }

            NewsList = NewsList.OrderByDescending(p => p.StartDate).ToList();

            if (NewsList.Count == 1)
            {
                NewsList[0].RelatedNews = (from a in db.news
                                           join b in db.countries on a.Country_ID equals b.ID
                                           where a.Deleted == 0 && b.Deleted == 0 && a.ID != ID
                                           select new ViewModels.News
                                           {
                                               ID = a.ID,
                                               Title = a.Title,
                                               Country = b.Name,
                                               Photo = a.Photo,
                                               Description = a.Description,
                                               Date = a.StartDate
                                           }).Take(4).ToList();
            }

            // Apply pagination and search
            // Get base query
            var query = NewsList.AsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(PaginationSetting.SearchText))
            {
                query = query.Where(p => p.Title.ToLower().Contains(PaginationSetting.SearchText.ToLower()));
            }

            // Compute total count BEFORE pagination
            PaginationSetting.TotalCount = query.Count();

            // Apply pagination
            NewsList = query
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
    public class News
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Date { get; set; }
        public DateTime? StartDate { get; set; }
        public List<News> RelatedNews { get; set; }
    }
}