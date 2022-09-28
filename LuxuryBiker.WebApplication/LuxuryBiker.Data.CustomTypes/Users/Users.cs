using LuxuryBiker.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace LuxuryBiker.Data.CustomTypes.Users
{
    public class Users : UsersInterface
    {
        public string IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Identificacion {get;set;}
        public bool SenActivo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public bool Rememberme { get; set; }

        public List<Claim> Claims { get; set; }
        public List<UsrUsuario_UsrRol> Roles { get; set; }
    }
}
