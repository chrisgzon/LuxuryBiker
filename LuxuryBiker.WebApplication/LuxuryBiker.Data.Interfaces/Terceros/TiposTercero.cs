using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Terceros
{
    public interface TiposTercero
    {
        int IdTipo { get; set; }
        string Nombre { get; set; }
        bool SenActivo { get; set; }
    }
}
