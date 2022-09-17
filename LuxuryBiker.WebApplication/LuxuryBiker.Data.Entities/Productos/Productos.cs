using LuxuryBiker.Data.Interfaces.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Entities.Productos
{
    public class Productos : ProductosInterface
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal Stock { get; set; }
        public decimal ValorProducto { get; set; }

        public List<Compras.ComprasDetails> DetallesCompra { get; set; }
        public List<Ventas.VentasDetails> DetallesVenta { get; set; }
    }
}
