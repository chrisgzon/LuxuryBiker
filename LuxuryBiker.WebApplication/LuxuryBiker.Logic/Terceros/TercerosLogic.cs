using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Data.Repositry.Terceros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Logic.Terceros
{
    public class TercerosLogic
    {
        private readonly TercerosRepository _tercerosRepository;
        public TercerosLogic()
        {
            _tercerosRepository = new TercerosRepository();
        }

        public ResponseGeneric<bool> RegistrarTercero(Tercero tercero)
        {
            try
            {
                var existTercero = _tercerosRepository.getTerceroByIdentificacionAndTipo(tercero.Identificacion, tercero.TipoIdTipo) != null;
                if (existTercero)
                {
                    return new ResponseGeneric<bool>()
                    {
                        Error = true,
                        Mensaje = "El tercero ya existe."
                    };
                }

                var result = _tercerosRepository.registrarTercero(tercero);

                return new ResponseGeneric<bool>
                {
                    Error = !result,
                    Mensaje = result ? "Se registro el tercero de forma correcta" : "No se logro registrar el tercero, intentelo nuevamente"
                };
            }
            catch (Exception)
            {

                return new ResponseGeneric<bool>
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al registrar el tercero"
                };
            }
        }
        public List<Tercero> GetProviders()
        {
            try
            {
                return _tercerosRepository.GetProviders();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<Tercero> GetClients()
        {
            try
            {
                return _tercerosRepository.GetClients();
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
