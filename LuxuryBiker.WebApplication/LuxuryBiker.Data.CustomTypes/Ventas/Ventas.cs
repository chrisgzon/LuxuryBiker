using LuxuryBiker.Data.Interfaces.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Ventas
{
    public class Ventas : VentasInterface
    {
        public int IdVenta { get; set; }
        public string CodVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public int TerceroIdTercero { get; set; } // Usuario al que se le realiza la venta
        public string UsuarioIdUsuario { get; set; } // Usuario que realiza la venta
        public bool Estado { get; set; }
        public decimal Total { get; set; }

        public Users.Users Usuario { get; set; }
        public Terceros.Terceros Tercero { get; set; }
    }
}
