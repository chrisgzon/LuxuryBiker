using LuxuryBiker.Contracts.DTOs.Products;
using LuxuryBiker.Data.CustomTypes.Compras;
using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Repositry.Productos
{
    public class ProductosRepository
    {
        public Producto getProductoByNombreOrReferencia(string referencia, string codigo)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Productos.Where(x => x.Referencia.Equals(referencia) || x.Codigo.Equals(codigo))
                                    .Select(x => new Producto()
                                    {
                                        Codigo = x.Codigo,
                                        Descripcion = x.Descripcion,
                                        Estado = x.Estado,
                                        FechaRegistro = x.FechaRegistro,
                                        IdProducto = x.IdProducto
                                    }).FirstOrDefault();
            }
        }
        public bool registerProducto(Producto producto)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entitie = new Data.Entities.Productos.Producto()
                {
                    Descripcion = producto.Descripcion,
                    Nombre = producto.Nombre,
                    Estado = true,
                    FechaRegistro = DateTime.Now,
                    Referencia = producto.Referencia,
                    Codigo = producto.Codigo
                };

                ctx.Productos.Add(entitie);
                return ctx.SaveChanges() > 0;
            }
        }
        public List<Producto> GetProductos()
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Productos.Where(x => x.Estado).Select(x => new Producto()
                {
                    IdProducto = x.IdProducto,
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    Nombre = x.Nombre,
                    Referencia = x.Referencia,
                    Stock = x.Stock,
                    ValorProducto = x.ValorProducto
                }).ToList();
            }
        }
        public void UpdateValueProducts(List<Producto> productos)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entities = new List<Entities.Productos.Producto>();
                foreach (var producto in productos)
                {
                    var entitie = ctx.Productos.FirstOrDefault(p=>p.IdProducto==producto.IdProducto);
                    entitie.ValorProducto = producto.ValorProducto;
                    entities.Add(entitie);
                }
                ctx.Productos.UpdateRange(entities);
                ctx.SaveChanges();
            }
        }
        public void UpdateStockProducts(List<Producto> compraProductos, bool suma)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entities = new List<Entities.Productos.Producto>();
                foreach (var producto in compraProductos)
                {
                    var entitie = ctx.Productos.FirstOrDefault(p => p.IdProducto == producto.IdProducto);
                    entitie.Stock = suma ? entitie.Stock + producto.Stock : entitie.Stock - producto.Stock;
                    entities.Add(entitie);
                }
                ctx.Productos.UpdateRange(entities);
                ctx.SaveChanges();
            }
        }
    }
}
