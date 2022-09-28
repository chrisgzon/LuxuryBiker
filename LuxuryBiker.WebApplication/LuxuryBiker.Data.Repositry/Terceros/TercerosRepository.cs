using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Repositry.Terceros
{
    public class TercerosRepository
    {
        public Data.CustomTypes.Terceros.Terceros getTerceroByIdentificacionAndTipo(string identificacion, int IdTipo)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Terceros.Where(x=>x.Identificacion.Equals(identificacion) && x.TipoIdTipo.Equals(IdTipo)).Select(x=>new Data.CustomTypes.Terceros.Terceros()
                {
                    IdTercero = x.IdTercero,
                    Direccion = x.Direccion,
                    Email = x.Email,
                    FechaCreacion = x.FechaCreacion,
                    Identificacion = x.Identificacion,
                    SenActivo = x.SenActivo,
                    Tipo = new Data.CustomTypes.Terceros.TiposTercero()
                    {
                        Nombre = x.Tipo.Nombre
                    },
                    Celular = x.Celular
                }).FirstOrDefault();
            }
        }
        public bool registrarTercero(Data.CustomTypes.Terceros.Terceros tercero)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entitie = new LuxuryBiker.Data.Entities.Terceros.Terceros()
                {
                    Direccion = tercero.Direccion,
                    Email = tercero.Email,
                    FechaCreacion = DateTime.Now,
                    Identificacion = tercero.Identificacion,
                    SenActivo = true,
                    TipoIdTipo = tercero.TipoIdTipo,
                    Celular = tercero.Celular
                };

                ctx.Terceros.Add(entitie);

                return ctx.SaveChanges() > 0;
            }
        }
    }
}
