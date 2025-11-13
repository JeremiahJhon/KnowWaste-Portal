namespace ViewModels
{
    public class Pagination
    {
        public string SearchText { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageCount { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

    }
    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class PolicyArea
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}