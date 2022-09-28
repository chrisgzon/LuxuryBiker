using LuxuryBiker.Data.Interfaces.Terceros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuxuryBiker.Data.CustomTypes.Terceros
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
        public string Celular { get; set; }

        public TiposTercero Tipo { get; set; }
    }
}
