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
        public Data.CustomTypes.Productos.Productos getProductoByNombreOrReferencia(string nombre, string referencia, string codigo)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Productos.Where(x => x.Nombre.Equals(nombre) || x.Referencia.Equals(referencia) || x.Codigo.Equals(codigo))
                                    .Select(x => new Data.CustomTypes.Productos.Productos()
                                    {
                                        Codigo = x.Codigo,
                                        Descripcion = x.Descripcion,
                                        Estado = x.Estado,
                                        FechaRegistro = x.FechaRegistro,
                                        IdProducto = x.IdProducto
                                    }).FirstOrDefault();
            }
        }
        public bool registerProducto(Data.CustomTypes.Productos.Productos producto)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                var entitie = new Data.Entities.Productos.Productos()
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
    }
}
