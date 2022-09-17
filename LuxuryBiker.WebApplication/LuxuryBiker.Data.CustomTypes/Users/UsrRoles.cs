using LuxuryBiker.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Users
{
    public class UsrRoles : UsrRolesInterface
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public bool SenActivo { get; set; }
    }
}
