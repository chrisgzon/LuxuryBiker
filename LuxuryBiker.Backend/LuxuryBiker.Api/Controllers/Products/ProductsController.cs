using LuxuryBiker.Logic.Productos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.web.Controllers.Productos
{
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductosLogic _productosLogic;
        public ProductosController()
        {
            _productosLogic = new ProductosLogic();
        }
        [Route("Productos/Register")]
        [HttpPost]
        public ResponseGeneric<bool> RegisterProducto(Producto producto)
        {
            return _productosLogic.RegisterTercero(producto);
        }
    }
}
