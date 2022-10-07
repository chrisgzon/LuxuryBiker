using LuxuryBiker.Data.CustomTypes.Compras;
using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
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
    }
}
