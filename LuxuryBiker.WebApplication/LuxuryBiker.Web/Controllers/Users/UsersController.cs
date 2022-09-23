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
        public ActionResult<Data.CustomTypes.Users.Users> Login(Data.CustomTypes.Users.Users user)
        {
             var usuario = new LoginLogic().CheckLogin(user.UserName, user.PasswordHash, user.Rememberme);
            if (usuario != null) return Ok(usuario);

            return BadRequest("Usuario y/o Contraseña invalidas");
        }
        [Route("Usuarios/Whoami")]
        [HttpGet]
        public ActionResult whoami()
        {
            var user = new LoginLogic().Whoami();
            if (user == null) return BadRequest(null);
            return Ok(user);
        }
        [Route("Usuarios/register")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(Data.CustomTypes.Users.Users usuario)
        {
            var user = _usersLogic.getPasswordByEmail(usuario.UserName);
            if (user != null)
            {
                return BadRequest("Ya existe un usuario registrado con este email");
            }
            return Ok(user);
        }
    }
}
