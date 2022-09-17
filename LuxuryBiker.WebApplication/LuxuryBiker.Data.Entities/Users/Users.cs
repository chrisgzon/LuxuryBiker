using LuxuryBiker.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuxuryBiker.Data.Entities.Users
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
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public List<UsrUsuario_UsrRol> Roles { get; set; }
        public List<Compras.Compras> Compras { get; set; } // Compras que realiza la empresa y que el usuario registra
        public List<Ventas.Ventas> Ventas { get; set; } // Ventas que hace la empresa a clientes
    }
}
