using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Productos;
using LuxuryBiker.Logic.Productos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Controllers.Productos
{
    [Authorize]
    [ApiController]
    public class ProductosController : ControllerBase
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
