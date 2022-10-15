using LuxuryBiker.Data.CustomTypes.Compras;
using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Productos;
using LuxuryBiker.Data.Repositry.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Logic.Productos
{
    public class ProductosLogic
    {
        private readonly ProductosRepository _productosRepository;
        public ProductosLogic()
        {
            _productosRepository = new ProductosRepository();
        }
        public ResponseGeneric<bool> RegisterTercero(Producto producto)
        {
            try
            {
                producto.Codigo = generateCodeProduct(producto);
                var existProducto = _productosRepository.getProductoByNombreOrReferencia(producto.Referencia, producto.Codigo) != null;
                if (existProducto)
                {
                    return new ResponseGeneric<bool>
                    {
                        Error = true,
                        Mensaje = "Este producto ya se encuentra registrado."
                    };
                }

                var resultRegister = _productosRepository.registerProducto(producto);
                return new ResponseGeneric<bool>
                {
                    Error = !resultRegister,
                    Mensaje = resultRegister ? String.Format("Se registro el producto de forma correcta con el codigo: {0}", producto.Codigo) 
                                                : "No se logro registrar el producto, intente nuevamente mas tarde"
                };
            }
            catch (Exception)
            {
                return new ResponseGeneric<bool>
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al momento de registrar el producto, intente nuevamente más tarde."
                };
            }
        }
        private string generateCodeProduct(Producto producto)
        {
            try
            {
                string code = "P";

                string[] wordsNombre = producto.Nombre.ToUpper().Split(" ");
                if (wordsNombre.Length > 2)
                {
                    wordsNombre = wordsNombre.Take(2).ToArray();
                }

                foreach (var word in wordsNombre)
                {
                    code += word.Substring(0, 2);
                }

                return code + producto.Referencia.Replace(" ", String.Empty).ToUpper();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Producto> GetProductos()
        {
            try
            {
                return _productosRepository.GetProductos();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public void UpdateValueProducts(List<Producto> productos)
        {
            try
            {
                _productosRepository.UpdateValueProducts(productos);
            }
            catch (Exception)
            {

                return;
            }
        }
        public void UpdateStockProducts(List<Producto> productosCompra, bool suma)
        {
            try
            {
                _productosRepository.UpdateStockProducts(productosCompra, suma);
            }
            catch (Exception)
            {

                return;
            }
        }
    }
}
