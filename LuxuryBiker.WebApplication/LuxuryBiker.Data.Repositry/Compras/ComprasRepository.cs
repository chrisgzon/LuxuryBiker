using LuxuryBiker.Data.CustomTypes.Compras;
using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Repositry.Compras
{
    public class ComprasRepository
    {
        public string GetUltimoCodigoCompra()
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var code = ctx.Compras.OrderByDescending(x => x.CodCompra).ThenBy(x => x.IdCompra).Select(x => x.CodCompra).FirstOrDefault();
                if (code == null)
                    code = "0";
                return code;
            }
        }
        public bool RegisterNewCompra(Compra compra)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                try
                {
                    var entitie = new Entities.Compras.Compra()
                    {
                        CodCompra = compra.CodCompra,
                        Estado = true,
                        FechaCompra = compra.FechaCompra,
                        TerceroIdTercero = compra.TerceroIdTercero,
                        Total = compra.Total,
                        UsuarioIdUsuario = compra.UsuarioIdUsuario,
                        DetallesCompra = compra.DetallesCompra.Select(x => new Entities.Compras.ComprasDetails()
                        {
                            cantidad = x.cantidad,
                            ProductoIdProducto = x.ProductoIdProducto,
                            ValorProducto = x.ValorProducto
                        }).ToList()
                    };

                    ctx.Compras.Add(entitie);
                    return ctx.SaveChanges() > 0; ;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public List<Compra> GetCompras(ParamsWebMethod oParams)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var compras = ctx.Compras.Select(c=> new Compra()
                {
                    IdCompra = c.IdCompra,
                    CodCompra = c.CodCompra,
                    Estado = c.Estado,
                    FechaCompra = c.FechaCompra,
                    Total = c.Total,
                    Tercero = new Tercero()
                    {
                        Nombres = c.Tercero.Nombres,
                        Apellidos = c.Tercero.Apellidos
                    },
                    CantidadProductos = (int)c.DetallesCompra.Sum(dc=>dc.cantidad)
                });

                if (oParams.FechaIni != null && oParams.FechaFinal != null)
                {
                    compras.Where(c=>DbFunctions.TruncateTime(oParams.FechaIni) <= DbFunctions.TruncateTime(c.FechaCompra)
                                        && DbFunctions.TruncateTime(oParams.FechaFinal) >= DbFunctions.TruncateTime(c.FechaCompra));
                }

                return compras.ToList();
            }
        }
        public bool ChangeStatus(Compra compra)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entitie = ctx.Compras.FirstOrDefault(c => c.IdCompra == compra.IdCompra);
                entitie.Estado = !compra.Estado;
                ctx.Compras.Update(entitie);
                return ctx.SaveChanges() > 0;
            }
        }
    }
}
