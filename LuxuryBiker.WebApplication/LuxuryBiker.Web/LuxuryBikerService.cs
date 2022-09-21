using LuxuryBiker.Data.CustomTypes.Users;
using LuxuryBiker.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using LuxuryBiker.Logic.Users;
using Microsoft.AspNetCore.DataProtection;
using LuxuryBiker.web.Security;

namespace LuxuryBiker.web
{
    public class LuxuryBikerService : ILuxuryBikerService
    {
        private readonly UsersLogic _usersLogic;

        public LuxuryBikerService()
        {
            _usersLogic = new UsersLogic();
        }

        public ActionResult<Users> Login(string username, string password)
        {
            return null;
        }
    }
}
