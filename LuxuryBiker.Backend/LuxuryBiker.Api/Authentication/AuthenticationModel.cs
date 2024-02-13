namespace LuxuryBiker.Api.Authentication
{
    public class AuthenticationModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Rememberme { get; set; }
    }
}