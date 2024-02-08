using LuxuryBiker.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Users
{
    public class UsrUsuario_UsrRol : UsrUsuario_UsrRolInterface
    {
        public int Id { get; set; }
        public string UsuarioIdUsuario { get; set; }
        public int RolIdRol { get; set; }

        public User Usuario { get; set; }
        public UsrRoles Rol { get; set; }
    }
}
