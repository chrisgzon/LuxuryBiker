using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Productos;
using LuxuryBiker.Data.CustomTypes.Ventas;
using LuxuryBiker.Data.Repositry.Ventas;
using LuxuryBiker.Logic.Productos;
using LuxuryBiker.Logic.Terceros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuxuryBiker.Logic.Ventas
{
    public class VentasLogic
    {
        private readonly VentasRepository _ventasRepository;
        private readonly ProductosLogic _productosLogic;
        private readonly TercerosLogic _tercerosLogic;
        private static decimal impuesto = 19;
        public VentasLogic()
        {
            _ventasRepository = new VentasRepository();
            _productosLogic = new ProductosLogic();
            _tercerosLogic = new TercerosLogic();
        }
        public ResponseGeneric<List<Producto>> GetProductsAndClients()
        {
            try
            {
                var productos = _productosLogic.GetProductos();
                var clientes = _tercerosLogic.GetClients();

                return new ResponseGeneric<List<Producto>>()
                {
                    Error = false,
                    Result = productos,
                    ExtraData = clientes
                };
            }
            catch (Exception)
            {
                return new ResponseGeneric<List<Producto>>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al registrar la venta"
                };
            }
        }
        public ResponseGeneric<bool> RegisterNewVenta(Venta venta)
        {
            try
            {
                venta.CodVenta = generateCodeVenta();
                venta.Total = CalculateTotalsVenta(venta);

                var result = _ventasRepository.RegisterNewVenta(venta);

                if (result)
                {
                    var productos = new List<Producto>();
                    venta.DetallesVenta.ForEach(x => {
                        var producto = new Producto();
                        producto.Stock = x.Cantidad;
                        producto.ValorProducto = x.ValorProducto;
                        producto.IdProducto = x.ProductoIdProducto;
                        productos.Add(producto);
                    });
                    new ProductosLogic().UpdateStockProducts(productos, false);
                }

                return new ResponseGeneric<bool>()
                {
                    Error = !result,
                    Mensaje = result ? "Se registro la venta de forma correcta." : "Ocurrio un error al registrar la venta."
                };
            }
            catch (Exception)
            {

                return new ResponseGeneric<bool>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al registrar la venta"
                };
            }
        }
        private string generateCodeVenta()
        {
            try
            {
                string ultimoCodigoVenta = _ventasRepository.GetUltimoCodigoVenta();
                int numeroCodigo = Int32.Parse(Regex.Match(ultimoCodigoVenta, @"\d+").Value);
                return String.Format("VLB{0}", numeroCodigo + 1);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private static decimal CalculateTotalsVenta(Venta venta)
        {
            try
            {
                decimal total = 0;
                decimal totalImpuesto = 0;
                foreach (var detalleventa in venta.DetallesVenta)
                {
                    decimal subtotal = detalleventa.Cantidad * detalleventa.ValorProducto;
                    total += subtotal;
                }
                totalImpuesto = venta.AplicaIva ? (total * (impuesto / 100)) : 0;
                total += totalImpuesto;
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ResponseGeneric<List<Venta>> GetVentas()
        {
            try
            {
                return new ResponseGeneric<List<Venta>>()
                {
                    Error = false,
                    Result = _ventasRepository.GetVentas()
                };
            }
            catch (Exception)
            {
                return new ResponseGeneric<List<Venta>>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al obtener las ventas."
                };
            }
        }
        public ResponseGeneric<bool> ChangeStatus(Venta venta)
        {
            try
            {
                var result = _ventasRepository.ChangeStatus(venta);
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
    }
}