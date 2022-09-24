using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.CustomTypes.Helpers
{
    public class Response
    {
        public bool Error { get; set; }

        public string Mensaje { get; set; }

        public long Entero { get; set; }

        //Excluye de grillas sin url para Web Methods
        public bool ExcludeGridMethod { get; set; }

        public Response()
        {
            Error = false;
            Mensaje = string.Empty;
        }

        public Response(bool error, Int64 entero, string mensaje)
        {
            Error = error;
            Entero = entero;
            Mensaje = mensaje;
        }

        public Response(bool error, string mensaje)
        {
            Error = error;
            Mensaje = mensaje;
        }

        public Response(string mensaje)
        {
            Error = !string.IsNullOrWhiteSpace(mensaje);
            Mensaje = mensaje;
        }
    }
    public class ResponseGeneric<T> : Response
    {
        public object ExtraData { get; set; }
        public T Result { get; set; }
        public T value { get; set; }
    }
}
