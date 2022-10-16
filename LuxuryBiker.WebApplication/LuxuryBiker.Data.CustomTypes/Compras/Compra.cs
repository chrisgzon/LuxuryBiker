using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Data.CustomTypes.Users;
using LuxuryBiker.Data.Interfaces.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Compras
{
    public class Compra : CompraInterface
    {
        public int IdCompra { get; set; }
        public string CodCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public string UsuarioIdUsuario { get; set; } // Usuario que recibe la compra
        public int TerceroIdTercero { get; set; } // Usuario que realiza la venta
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public int CantidadProductos { get; set; }

        public List<ComprasDetails> DetallesCompra { get; set; }
        public User Usuario { get; set; }
        public Tercero Tercero { get; set; }
    }
}
