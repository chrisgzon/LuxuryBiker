using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Entities.Terceros
{
    public class TiposTercero
    {
        public int IdTipo { get; set; }
        public string Nombre { get; set; }
        public bool SenActivo { get; set; }

        public List<Tercero> Terceros { get; set; }
    }
}
