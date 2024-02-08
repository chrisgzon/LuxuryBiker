using LuxuryBiker.Contracts.DTOs.Compras;
using LuxuryBiker.Contracts.DTOs.Products;
using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.Repositry.Compras;
using LuxuryBiker.Logic.Productos;
using LuxuryBiker.Logic.Terceros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuxuryBiker.Logic.Compras
{
    public class ComprasLogic
    {
        private readonly ProductosLogic _productosLogic;
        private readonly TercerosLogic _tercerosLogic;
        private readonly ComprasRepository _comprasRepository;
        private static decimal impuesto = 19;
        public ComprasLogic()
        {
            _productosLogic = new ProductosLogic();
            _tercerosLogic = new TercerosLogic();
            _comprasRepository = new ComprasRepository();
        }
        public ResponseGeneric<List<Producto>> GetProductsAndProviders()
        {
            try
            {
                var productos = _productosLogic.GetProductos();
                var providers = _tercerosLogic.GetProviders();

                return new ResponseGeneric<List<Producto>>()
                {
                    Error = false,
                    Result = productos,
                    ExtraData = providers
                };
            }
            catch (Exception)
            {

                return new ResponseGeneric<List<Producto>>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al obtener los datos."
                };
            }
        }
        public ResponseGeneric<bool> RegisterNewCompra(Compra compra)
        {
            try
            {
                compra.CodCompra = generateCodeCompra();
                compra.Total = CalculateTotalsCompra(compra);

                var result = _comprasRepository.RegisterNewCompra(compra);

                if (result)
                {
                    var productos = new List<Producto>();
                    compra.DetallesCompra.ForEach(x=> {
                        var producto = new Producto();
                        producto.Stock = x.cantidad;
                        producto.ValorProducto = x.ValorProducto;
                        producto.IdProducto = x.ProductoIdProducto;
                        productos.Add(producto);
                    });
                    new ProductosLogic().UpdateValueProducts(productos);
                    new ProductosLogic().UpdateStockProducts(productos, true);
                }

                return new ResponseGeneric<bool>()
                {
                    Error = !result,
                    Mensaje = result ? "Se registro la compra de forma correcta." : "Ocurrio un error al registrar la compra."
                };
            }
            catch (Exception)
            {

                return new ResponseGeneric<bool>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al registrar la compra"
                };
            }
        }
        private string generateCodeCompra()
        {
            try
            {
                string ultimoCodigoCompra = _comprasRepository.GetUltimoCodigoCompra();
                int numeroCodigo = Int32.Parse(Regex.Match(ultimoCodigoCompra, @"\d+").Value);
                return String.Format("CLB{0}", numeroCodigo + 1);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private static decimal CalculateTotalsCompra(Compra compra)
        {
            try
            {
                decimal total = 0;
                decimal totalImpuesto = 0;
                foreach (var detalleCompra in compra.DetallesCompra)
                {
                    decimal subtotal = detalleCompra.cantidad * detalleCompra.ValorProducto;
                    total += subtotal;
                } 
                totalImpuesto = compra.AplicaIva ? (total * (impuesto / 100)) : 0;
                total += totalImpuesto;
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ResponseGeneric<List<Compra>> GetCompras(ParamsWebMethod oParams)
        {
            try
            {
                return new ResponseGeneric<List<Compra>>()
                {
                    Error = false,
                    Result = _comprasRepository.GetCompras(oParams)
                };
            }
            catch (Exception)
            {
                return new ResponseGeneric<List<Compra>>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al obtener las compras."
                };
            }
        }
        public ResponseGeneric<bool> ChangeStatus(Compra compra)
        {
            try
            {
                var result = _comprasRepository.ChangeStatus(compra);
                return new ResponseGeneric<bool>()
                {
                    Error = !result
                };
            }
            catch (Exception)
            {

                return new ResponseGeneric<bool>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al intentar actualizar el estado de la compra."
                };
            }
        }
        public ResponseGeneric<Dictionary<string, object>> GetData()
        {
            try
            {
                return new ResponseGeneric<Dictionary<string, object>>()
                {
                    Result = _comprasRepository.GetData(),
                    Error = false
                };
            } catch (Exception ex)
            {
                return new ResponseGeneric<Dictionary<string, object>>()
                {
                    Error = true
                };
            }
        }
    }
}
