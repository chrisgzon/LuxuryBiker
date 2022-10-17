using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Productos;
using LuxuryBiker.Data.CustomTypes.Ventas;
using LuxuryBiker.Logic.Ventas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Controllers.Ventas
{
    [Authorize]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly VentasLogic _ventasLogic;
        public VentasController()
        {
            _ventasLogic = new VentasLogic();
        }
        [Route("Ventas/GetProductsAndClients")]
        public ResponseGeneric<List<Producto>> GetProductsAndClients()
        {
            return _ventasLogic.GetProductsAndClients();
        }
        [Route("Ventas/Register")]
        [HttpPost]
        public ResponseGeneric<bool> RegisterNewVenta(Venta venta)
        {
            return _ventasLogic.RegisterNewVenta(venta);
        }
        [Route("Ventas/GetVentas")]
        public ResponseGeneric<List<Venta>> GetVentas()
        {
            return _ventasLogic.GetVentas();
        }
        [Route("Ventas/ChangeStatus")]
        public ResponseGeneric<bool> ChangeStatus(Venta venta)
        {
            return _ventasLogic.ChangeStatus(venta);
        }
    }
}
