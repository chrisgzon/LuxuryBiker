using LuxuryBiker.Data.Interfaces.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Entities.Compras
{ 
    public class Compras : ComprasInterface
    {
        public int IdCompra { get; set; }
        public string CodCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public string UsuarioIdUsuario { get; set; } // Usuario que recibe la compra
        public int TerceroIdTercero { get; set; } // Usuario que realiza la venta
        public decimal Total { get; set; }
        public bool Estado { get; set; }

        public Users.Users Usuario { get; set; }
        public Terceros.Terceros Tercero { get; set; }
        public List<ComprasDetails> DetallesCompra { get; set; }
    }
}
