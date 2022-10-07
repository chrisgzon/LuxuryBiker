using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Users;
using LuxuryBiker.Logic.Users;
using LuxuryBiker.web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Controllers.Users
{
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersLogic _usersLogic;
        public UsersController()
        {
            _usersLogic = new UsersLogic();
        }
        [Route("Usuarios/Login")]
        [HttpPost]
        [AllowAnonymous]
        public ResponseGeneric<User> Login(User user)
        {
             return new LoginLogic().CheckLogin(user.UserName, user.PasswordHash, user.Rememberme);
        }
        [Route("Usuarios/Whoami")]
        [HttpGet]
        public ResponseGeneric<User> whoami()
        {
            return new LoginLogic().Whoami();
        }
        [Route("Usuarios/register")]
        [AllowAnonymous]
        [HttpPost]
        public ResponseGeneric<User> Register(User usuario)
        {
            return _usersLogic.RegisterNewUser(usuario);
        }
    }
}
