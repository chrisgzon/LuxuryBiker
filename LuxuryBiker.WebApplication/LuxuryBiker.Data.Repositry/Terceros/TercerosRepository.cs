using LuxuryBiker.Data.CustomTypes.Terceros;
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
        public Tercero getTerceroByIdentificacionAndTipo(string identificacion, int IdTipo)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Terceros.Where(x=>x.Identificacion.Equals(identificacion) && x.TipoIdTipo.Equals(IdTipo)).Select(x=>new Tercero()
                {
                    IdTercero = x.IdTercero,
                    Direccion = x.Direccion,
                    Email = x.Email,
                    FechaCreacion = x.FechaCreacion,
                    Identificacion = x.Identificacion,
                    SenActivo = x.SenActivo,
                    Tipo = new TiposTercero()
                    {
                        Nombre = x.Tipo.Nombre
                    },
                    Celular = x.Celular
                }).FirstOrDefault();
            }
        }
        public bool registrarTercero(Tercero tercero)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entitie = new Entities.Terceros.Tercero()
                {
                    Direccion = tercero.Direccion,
                    Email = tercero.Email,
                    FechaCreacion = DateTime.Now,
                    Nombres = tercero.Nombres,
                    Apellidos = tercero.Apellidos,
                    Identificacion = tercero.Identificacion,
                    SenActivo = true,
                    TipoIdTipo = tercero.TipoIdTipo,
                    Celular = tercero.Celular
                };

                ctx.Terceros.Add(entitie);

                return ctx.SaveChanges() > 0;
            }
        }
        public List<Tercero> GetProviders()
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Terceros.Where(x => x.Tipo.IdTipo.Equals(1) && x.SenActivo).Select(x => new Tercero()
                {
                    Nombres = x.Nombres,
                    Apellidos = x.Apellidos,
                    Celular = x.Celular,
                    IdTercero = x.IdTercero,
                    Email = x.Email,
                    Identificacion  = x.Identificacion
                }).ToList();
            }
        }
    }
}
