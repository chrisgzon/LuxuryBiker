namespace LuxuryBiker.Application.Thirds.Commands.CreateThird
{
    public class ThirdDto
    {
        public string? Email { get; set; }
        public string? Identification { get; set; }
        public string? Address { get; set; }
        public bool? Active { get; set; }
        public string? CellPhone { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public int TypeId { get; set; }
    }

    public class ThirdTypeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
    }
}
