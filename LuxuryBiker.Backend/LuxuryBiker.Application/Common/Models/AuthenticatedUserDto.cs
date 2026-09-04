namespace LuxuryBiker.Application.Common.Models
{
    /// <summary>
    /// Perfil del usuario autenticado que la API devuelve al cliente.
    /// Vive en Application para que la capa de presentación no dependa de Infrastructure.
    /// </summary>
    public class AuthenticatedUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset? FechaNacimiento { get; set; }
        public string Token { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
