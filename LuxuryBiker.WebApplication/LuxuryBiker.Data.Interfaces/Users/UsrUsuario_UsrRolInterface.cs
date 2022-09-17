using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Users
{
    public interface UsrUsuario_UsrRolInterface
    {
        int Id { get; set; }
        string UsuarioIdUsuario { get; set; }
        int RolIdRol { get; set; }
    }
}
