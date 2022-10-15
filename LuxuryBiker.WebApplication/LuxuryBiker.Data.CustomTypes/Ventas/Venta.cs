using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Data.CustomTypes.Users;
using LuxuryBiker.Data.Interfaces.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Ventas
{
    public class Venta : VentaInterface
    {
        public int IdVenta { get; set; }
        public string CodVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public int? TerceroIdTercero { get; set; } // Usuario al que se le realiza la venta
        public string UsuarioIdUsuario { get; set; } // Usuario que realiza la venta
        public bool Estado { get; set; }
        public decimal Total { get; set; }

        public User Usuario { get; set; }
        public Tercero Tercero { get; set; }
        public List<VentasDetails> DetallesVenta { get; set; }
    }
}
