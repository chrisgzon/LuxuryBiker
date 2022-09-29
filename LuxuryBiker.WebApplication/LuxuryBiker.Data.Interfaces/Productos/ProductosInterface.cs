using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Interfaces.Productos
{
    public interface ProductosInterface
    {
        int IdProducto { get; set; }
        string Nombre { get; set; }
        string Codigo { get; set; }
        string Referencia { get; set; }
        string Descripcion { get; set; }
        bool Estado { get; set; }
        DateTime FechaRegistro { get; set; }
        decimal Stock { get; set; }
        decimal ValorProducto { get; set; }
    }
}
