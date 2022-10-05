using LuxuryBiker.Data.Entities.Productos;
using LuxuryBiker.Data.Interfaces.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Entities.Ventas
{
    public class VentasDetails : VentasDetailsInterface
    {
        public int Id { get; set; }
        public int ProductoIdProducto { get; set; }
        public int VentaIdVenta { get; set; }
        public decimal ValorProducto { get; set; }
        public decimal Cantidad { get; set; }

        public Producto Producto { get; set; }
        public Venta Venta { get; set; }
    }
}