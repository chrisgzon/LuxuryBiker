using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Application.Users.Queries.CheckLogin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.Api.Controllers.Users
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;
        public UsersController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseGeneric<UserAuthenticated>> Login(string username, string password, bool rememberme)
        {
            return await _mediator.Send(new CheckLoginQuery(username, password, rememberme));
        }
    }
}