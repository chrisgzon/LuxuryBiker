namespace LuxuryBiker.Application.Common.Models
{
    public class UserAuthenticated
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
