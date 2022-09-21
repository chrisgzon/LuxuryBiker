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
        [Route("getUsers")]
        [HttpGet]
        public ActionResult<List<string>> getUsers()
        {
            var list = new List<string>();
            list.Add("Jose");
            list.Add("Andres");
            list.Add("Manuel");
            return Ok(list);
        }
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<Data.CustomTypes.Users.Users> Login(Data.CustomTypes.Users.Users user)
        {
            var usuario = new LoginLogic().CheckLogin(user.UserName, user.PasswordHash);
            if (usuario != null) return Ok(usuario);

            return BadRequest("Usuario y/o contraseña ivalidas");
        }
    }
}
