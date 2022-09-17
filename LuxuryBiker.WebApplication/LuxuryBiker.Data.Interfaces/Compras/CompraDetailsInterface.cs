using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Compras
{
    public interface CompraDetailsInterface
    {
        int Id { get; set; }
        int CompraIdCompra { get; set; }
        int ProductoIdProducto { get; set; }
        decimal ValorProducto { get; set; }
        decimal cantidad { get; set; }
    }
}
