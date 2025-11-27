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

    public class City
    {
        public int ID { get; set; }
        public int? CountryID { get; set; }
        public string Name { get; set; }
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

    public class SubRegion
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class WasteType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}