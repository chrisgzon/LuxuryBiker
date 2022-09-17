using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Compras
{
    public interface ComprasInterface
    {
        int IdCompra { get; set; }
        string CodCompra { get; set; }
        DateTime FechaCompra { get; set; }
        string UsuarioIdUsuario { get; set; }
        int TerceroIdTercero { get; set; }
        decimal Total { get; set; }
        bool Estado { get; set; }
    }
}
