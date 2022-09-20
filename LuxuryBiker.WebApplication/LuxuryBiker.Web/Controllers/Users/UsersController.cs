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
        public List<string> getUsers()
        {
            var list = new List<string>();
            list.Add("Jose");
            list.Add("Andres");
            list.Add("Manuel");
            return list;
        }
    }
}
