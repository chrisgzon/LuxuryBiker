using LuxuryBiker.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Repositry.Users
{
    public class UsersRepository
    {
        public string getPasswordByEmail(string username)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Users.Where(x => username.Equals(x.UserName) && x.SenActivo).Select(x => x.PasswordHash).FirstOrDefault();
            }
        }
        public CustomTypes.Users.Users getUserByEmail(string username)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                
                return ctx.Users.Where(x => x.UserName == username)
                    .Select(x => new CustomTypes.Users.Users
                    {
                        IdUsuario = x.IdUsuario,
                        UserName = x.UserName,
                        Email = x.Email,
                        Roles = x.Roles.Select(r => new CustomTypes.Users.UsrUsuario_UsrRol
                        {
                            Rol = new CustomTypes.Users.UsrRoles
                            {
                                NombreRol = r.Rol.NombreRol,
                                IdRol = r.Rol.IdRol
                            }
                        }).ToList(),

                    })
                    .FirstOrDefault();
            }
        }
    }
}