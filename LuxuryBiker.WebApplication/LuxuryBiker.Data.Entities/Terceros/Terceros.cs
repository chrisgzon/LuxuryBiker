using LuxuryBiker.Data.Interfaces.Terceros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuxuryBiker.Data.Entities.Terceros
{
    public class Terceros : TercerosInterface
    {
        public int IdTercero { get; set; }
        public string Email { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Direccion { get; set; }
        public bool SenActivo { get; set; }
        public int TipoIdTipo { get; set; }

        public TiposTercero Tipo { get; set; }
        public List<Compras.Compras> VentasProveedor { get; set; } // Ventas del proveedor a la empresa
        public List<Ventas.Ventas> ComprasCliente { get; set; } // Compras de los clientes a la empresa
    }
}
