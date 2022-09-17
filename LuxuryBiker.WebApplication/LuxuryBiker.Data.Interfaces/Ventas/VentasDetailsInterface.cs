using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Ventas
{
    public interface VentasDetailsInterface
    {
        int Id { get; set; }
        int ProductoIdProducto { get; set; }
        int VentaIdVenta { get; set; }
        decimal ValorProducto {get; set; }
        decimal Cantidad { get; set; }
    }
}
