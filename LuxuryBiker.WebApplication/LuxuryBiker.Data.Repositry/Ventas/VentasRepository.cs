using LuxuryBiker.Data.CustomTypes.Productos;
using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Data.CustomTypes.Ventas;
using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Repositry.Ventas
{
    public class VentasRepository
    {
        public string GetUltimoCodigoVenta()
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var code = ctx.Ventas.OrderByDescending(x => x.IdVenta).Select(x => x.CodVenta).FirstOrDefault();
                if (code == null)
                    code = "0";
                return code;
            }
        }
        public bool RegisterNewVenta(Venta venta)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                try
                {
                    var entitie = new Entities.Ventas.Venta()
                    {
                        CodVenta= venta.CodVenta,
                        Estado = true,
                        FechaVenta = DateTime.Now,
                        TerceroIdTercero = venta.TerceroIdTercero,
                        Total = venta.Total,
                        UsuarioIdUsuario = venta.UsuarioIdUsuario,
                        DetallesVentas = venta.DetallesVenta.Select(x => new Entities.Ventas.VentasDetails()
                        {
                            Cantidad = x.Cantidad,
                            ProductoIdProducto = x.ProductoIdProducto,
                            ValorProducto = x.ValorProducto
                        }).ToList()
                    };

                    ctx.Ventas.Add(entitie);
                    return ctx.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public List<Venta> GetVentas()
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var ventas = ctx.Ventas.Select(v => new Venta()
                {
                    IdVenta = v.IdVenta,
                    CodVenta = v.CodVenta,
                    Estado = v.Estado,
                    FechaVenta = v.FechaVenta,
                    Total = v.Total,
                    Tercero = v.Tercero != null ? new Tercero()
                    {
                        Nombres = v.Tercero.Nombres,
                        Apellidos = v.Tercero.Apellidos
                    } : null,
                    CantidadProductos = (int)v.DetallesVentas.Sum(dc => dc.Cantidad)
                });

                return ventas.ToList();
            }
        }
        public bool ChangeStatus(Venta venta)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entitie = ctx.Ventas.FirstOrDefault(c => c.IdVenta == venta.IdVenta);
                entitie.Estado = !venta.Estado;
                ctx.Ventas.Update(entitie);
                return ctx.SaveChanges() > 0;
            }
        }
    }
}
