namespace LuxuryBiker.Application.Thirds.Queries.GetThirds
{
    public class ThirdBriefDto
    {
        public int Id { get; set; }
        public string Identification { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public string? Email { get; set; }
        public string? CellPhone { get; set; }
        public string? Address { get; set; }
        public bool? Active { get; set; }
        public int TypeId { get; set; }
        public string? TypeName { get; set; }
    }
}
