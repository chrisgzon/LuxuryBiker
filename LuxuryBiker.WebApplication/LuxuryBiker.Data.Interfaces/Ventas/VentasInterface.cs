using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Ventas
{
    public interface VentasInterface
    {
        int IdVenta { get; set; }
        string CodVenta { get; set; }
        DateTime FechaVenta { get; set; }
        int TerceroIdTercero { get; set; }
        string UsuarioIdUsuario { get; set; }
        bool Estado { get; set; }
        decimal Total { get; set; }
    }
}
