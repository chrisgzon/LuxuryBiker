using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Terceros;
using LuxuryBiker.Logic.Terceros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Controllers.Terceros
{
    [Authorize]
    public class TercerosController : ControllerBase
    {
        private readonly TercerosLogic _tercerosLogic;
        public TercerosController()
        {
            _tercerosLogic = new TercerosLogic();
        }

        [Route("Terceros/Register")]
        [HttpPost]
        public ResponseGeneric<bool> Login(Tercero tercero)
        {
            return _tercerosLogic.RegistrarTercero(tercero);
        }
    }
}
