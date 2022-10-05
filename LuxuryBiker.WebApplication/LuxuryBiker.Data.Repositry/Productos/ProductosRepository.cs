using LuxuryBiker.Data.CustomTypes.Productos;
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
        public Producto getProductoByNombreOrReferencia(string nombre, string referencia, string codigo)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Productos.Where(x => x.Nombre.Equals(nombre) || x.Referencia.Equals(referencia) || x.Codigo.Equals(codigo))
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
                return ctx.Productos.Where(x => x.Estado && x.Stock > 0).Select(x => new Producto()
                {
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    Nombre = x.Nombre,
                    Referencia = x.Referencia,
                    Stock = x.Stock,
                    ValorProducto = x.ValorProducto
                }).ToList();
            }
        }
    }
}
