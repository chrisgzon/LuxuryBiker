using LuxuryBiker.Data.Interfaces.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Compras
{
    public class ComprasDetails : CompraDetailsInterface
    {
        public int Id { get; set; }
        public int CompraIdCompra { get; set; }
        public int ProductoIdProducto { get; set; }
        public decimal ValorProducto { get; set; }
        public decimal cantidad { get; set; }

        public Compras Compra { get; set; }
        public Productos.Productos Producto  { get; set; }
    }
}
