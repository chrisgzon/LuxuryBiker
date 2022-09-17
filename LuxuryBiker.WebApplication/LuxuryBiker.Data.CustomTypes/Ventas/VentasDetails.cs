using LuxuryBiker.Data.Interfaces.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Ventas
{
    public class VentasDetails : VentasDetailsInterface
    {
        public int Id { get; set; }
        public int ProductoIdProducto { get; set; }
        public int VentaIdVenta { get; set; }
        public decimal ValorProducto { get; set; }
        public decimal Cantidad { get; set; }

        public Productos.Productos Producto { get; set; }
        public Ventas Venta { get; set; }
    }
}