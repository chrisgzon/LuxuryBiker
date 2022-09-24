using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Users
{
    public interface UsersInterface
    {
         string IdUsuario { get; set; }
         string Nombres { get; set; }
         string Apellidos { get; set; }
         string Email { get; set; }
         string PasswordHash { get; set; }
         string Identificacion { get; set; }
         bool SenActivo { get; set; }
         DateTime? FechaNacimiento { get; set; }
         DateTime FechaCreacion { get; set; }
         string UserName { get; set; }
         string PhoneNumber { get; set; }
    }
}
