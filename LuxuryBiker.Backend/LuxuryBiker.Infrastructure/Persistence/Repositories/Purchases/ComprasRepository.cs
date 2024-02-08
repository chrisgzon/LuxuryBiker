using LuxuryBiker.Contracts.DTOs.Compras;
using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
                var code = ctx.Compras.OrderByDescending(x => x.IdCompra).Select(x => x.CodCompra).FirstOrDefault();
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
        public Dictionary<string, object> GetData()
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var dashboard = new Dictionary<string, object>();
                var dayNow = DateTime.Now;

                var totalCompraMes = ctx.Compras
                                .Where(c => c.FechaCompra.Month == dayNow.Month && c.Estado)
                                .GroupBy(c => c.FechaCompra.Month)
                                .Select(c => new
                                {
                                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Key),
                                    Total = c.Sum(x => x.Total)
                                }).FirstOrDefault();
                var totalVentaMes = ctx.Ventas
                                .Where(c => c.FechaVenta.Month == dayNow.Month && c.Estado)
                                .GroupBy(c => c.FechaVenta.Month)
                                .Select(c => new
                                {
                                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Key),
                                    Total = c.Sum(x => x.Total)
                                }).FirstOrDefault();
                var comprasMes = ctx.Compras
                                    .Where(c => c.Estado)
                                    .GroupBy(c => c.FechaCompra.Month)
                                    .OrderBy(c => c.Key)
                                    .Take(12)
                                    .Select(c => new
                                    {
                                        IndexMes = c.Key,
                                        Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Key),
                                        Total = c.Sum(x => x.Total)
                                    }).ToList();
                var ventasMes = ctx.Ventas
                                    .Where(c => c.Estado)
                                    .GroupBy(c => c.FechaVenta.Month)
                                    .OrderBy(c => c.Key)
                                    .Take(12)
                                    .Select(c => new
                                    {
                                        IndexMes = c.Key,
                                        Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Key),
                                        Total = c.Sum(x => x.Total)
                                    }).ToList();
                var ventasDia = ctx.Ventas
                                    .Where(v => v.Estado && v.FechaVenta >= dayNow.AddDays(-15))
                                    .GroupBy(v => v.FechaVenta.Day)
                                    .OrderBy(v => v.Key)
                                    .Take(15)
                                    .Select(v => new
                                    {
                                        Dia = v.Key.ToString(),
                                        Total = v.Sum(x => x.Total)
                                    }).ToList();
                var ventasHoy = ctx.Ventas
                                .Where(c => c.FechaVenta.Day == dayNow.Day && c.Estado)
                                .GroupBy(c => c.FechaVenta.Day)
                                .Select(c => new
                                {
                                    Total = c.Sum(x => x.Total)
                                }).FirstOrDefault();
                var productosMasVendidos = ctx.Productos
                                                .Join(ctx.DetailsVentas, p => p.IdProducto, dv => dv.ProductoIdProducto, (p, dv) => new { p, dv })
                                                .Join(ctx.Ventas.Where(v => v.Estado && v.FechaVenta.Year == dayNow.Year), @t => @t.dv.VentaIdVenta, v => v.IdVenta, (t, v) => new { t.dv, t.p, v })
                                                .GroupBy(x => new { x.p.Codigo, x.p.Nombre, x.p.IdProducto, x.p.Stock })
                                                .OrderByDescending(x => x.Sum(s => s.dv.Cantidad))
                                                .Take(10)
                                                .Select(x => new
                                                {
                                                    Codigo = x.Key.Codigo,
                                                    Cantidad = x.Sum(s => s.dv.Cantidad),
                                                    Nombre = x.Key.Nombre,
                                                    IdProducto = x.Key.IdProducto,
                                                    Stock = x.Key.Stock
                                                }).ToList();
            
                /* -----------------------  Se instancian datos por defecto en caso de que no se encuentren registros ----------------------------- */


                dashboard.Add("comprasMes", comprasMes.Count == 0
                    ?
                        new List<object>() { new
                        {
                            Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dayNow.Month),
                            Total = (decimal)0
                        } }
                    :
                        comprasMes
                    );
                dashboard.Add("ventasMes", ventasMes.Count == 0
                    ?
                        new List<object>() { new
                        {
                            Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dayNow.Month),
                            Total = 0
                        }}
                    :
                        ventasMes
                    );
                dashboard.Add("ventasDia", ventasDia.Count == 0
                    ?
                        new List<object>() { new
                        {
                            Dia = dayNow.Day,
                            Total = 0
                        } }
                    :
                        ventasDia
                    );
                dashboard.Add("totalCompraMes", totalCompraMes == null
                    ? 
                        new
                        {
                            Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dayNow.Month),
                            Total = 0
                        }
                    :
                        totalCompraMes
                    );
                dashboard.Add("totalVentaMes", totalVentaMes == null
                    ?
                        new
                        {
                            Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dayNow.Month),
                            Total = 0
                        }
                    :
                        totalVentaMes
                    );
                dashboard.Add("ventasHoy", ventasHoy == null
                    ?
                        new
                        {
                            Total = 0
                        }
                    :
                        ventasHoy
                    );
                dashboard.Add("productosMasVendidos",productosMasVendidos);

                return dashboard;
            }
        }
    }
}
