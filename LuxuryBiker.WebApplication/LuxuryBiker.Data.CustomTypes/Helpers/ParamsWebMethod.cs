using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Helpers
{
    public class ParamsWebMethod
    {
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int IdTercero { get; set; }
        public string IdUsuario { get; set; }
    }
}
