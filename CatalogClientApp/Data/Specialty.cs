namespace CatalogClientApp.Data
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int DirectionId { get; set; }
        public string DirectionName { get; set; } = "";
    }
}

