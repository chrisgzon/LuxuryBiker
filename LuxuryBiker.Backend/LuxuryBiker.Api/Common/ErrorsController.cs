using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.Api.Common
{
    public class ErrorsControler : ControllerBase
    {
        /// <summary>
        /// retrieve data of errors produced at consulting to others controllers
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return Problem();
        }
    }
}
