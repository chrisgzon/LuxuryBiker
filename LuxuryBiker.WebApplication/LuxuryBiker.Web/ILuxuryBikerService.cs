using FluentValidation;
using LuxuryBiker.Data.CustomTypes.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Web
{
    public interface ILuxuryBikerService
    {
        public ActionResult<Users> Login(string username, string password);
    }
}
