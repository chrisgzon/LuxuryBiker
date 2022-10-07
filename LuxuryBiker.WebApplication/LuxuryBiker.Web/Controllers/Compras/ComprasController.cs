﻿using LuxuryBiker.Data.CustomTypes.Compras;
using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Productos;
using LuxuryBiker.Logic.Compras;
using LuxuryBiker.Logic.Productos;
using LuxuryBiker.Logic.Terceros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Controllers.Compras
{
    [Authorize]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ComprasLogic _comprasLogic;
        public ComprasController()
        {
            _comprasLogic = new ComprasLogic();
        }

        [Route("Compras/GetProductsAndProviders")]
        public ResponseGeneric<List<Producto>> GetProductsAndProviders()
        {
            return _comprasLogic.GetProductsAndProviders();
        }
        [Route("Compras/Register")]
        public ResponseGeneric<bool> RegisterNewCompra(Compra compra)
        {
            return _comprasLogic.RegisterNewCompra(compra);
        }
    }
}