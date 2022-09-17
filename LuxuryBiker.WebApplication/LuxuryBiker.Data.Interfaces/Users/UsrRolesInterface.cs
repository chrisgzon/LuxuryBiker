using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Users
{
    public interface UsrRolesInterface
    {
        int IdRol { get; set; }
        string NombreRol { get; set; }
        bool SenActivo { get; set; }
    }
}
