using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Terceros
{
    public interface TerceroInterface
    {
        int IdTercero { get; set; }
        string Email { get; set; }
        string Identificacion { get; set; }
        DateTime FechaCreacion { get; set; }
        string Direccion { get; set; }
        bool SenActivo { get; set; }
        int TipoIdTipo { get; set; }
        string Celular { get; set; }
    }
}
