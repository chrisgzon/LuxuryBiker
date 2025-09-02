namespace LuxuryBiker.Api.Thirds
{
    public class ThirdModel
    {
        public string? Email { get; set; }
        public string? Identification { get; set; }
        public string? Address { get; set; }
        public bool? Active { get; set; }
        public string? CellPhone { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public ThirdTypeModel? Type { get; set; }
    }

    public class ThirdTypeModel
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
    }
}
