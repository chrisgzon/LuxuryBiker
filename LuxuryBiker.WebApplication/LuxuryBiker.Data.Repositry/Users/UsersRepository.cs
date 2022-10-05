using LuxuryBiker.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxuryBiker.Data.CustomTypes.Users;

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
        public User getUserByEmail(string username)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                
                return ctx.Users.Where(x => x.UserName == username)
                    .Select(x => new User
                    {
                        IdUsuario = x.IdUsuario,
                        UserName = x.UserName,
                        Email = x.Email,
                        Apellidos = x.Apellidos,
                        Nombres = x.Nombres,
                        Roles = x.Roles.Select(r => new UsrUsuario_UsrRol
                        {
                            Rol = new UsrRoles
                            {
                                NombreRol = r.Rol.NombreRol,
                                IdRol = r.Rol.IdRol
                            }
                        }).ToList(),
                        isAdministrador = (x.Roles.Where(r => r.Rol.NombreRol.Equals("Super Administrador")).FirstOrDefault() != null)
                    })
                    .FirstOrDefault();
            }
        }
        public User getUserById(string idUsuario)
        {
            using (var ctx = new LuxuryBikerDBContext())
            {
                return ctx.Users.Where(x => x.IdUsuario.Equals(idUsuario)).Select(x => new User
                    {
                        IdUsuario = x.IdUsuario,
                        UserName = x.UserName,
                        Email = x.Email,
                        Apellidos = x.Apellidos,
                        Nombres = x.Nombres,
                        Roles = x.Roles.Select(r => new UsrUsuario_UsrRol
                                {
                                    Rol = new UsrRoles
                                    {
                                        NombreRol = r.Rol.NombreRol,
                                        IdRol = r.Rol.IdRol
                                    }
                            }).ToList(),
                        isAdministrador = (x.Roles.Where(r=>r.Rol.NombreRol.Equals("Super Administrador")).FirstOrDefault() != null)
                    }).FirstOrDefault();
            }
        }
        public bool registerNewUser(User usuario)
        {
            using (var ctx = new LuxuryBikerDBContext()) {
                var username = new SqlParameter
                {
                    ParameterName = "@USERNAME",
                    Value = usuario.UserName,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    DbType = System.Data.DbType.String
                };
                var password = new SqlParameter
                {
                    ParameterName = "@PASSWORD",
                    Value = usuario.PasswordHash,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    DbType = System.Data.DbType.String
                };
                var nombres = new SqlParameter
                {
                    ParameterName = "@NOMBRES",
                    Value = usuario.Nombres,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    DbType = System.Data.DbType.String
                };
                var apellidos = new SqlParameter
                {
                    ParameterName = "@APELLIDOS",
                    Value = usuario.Apellidos,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    DbType = System.Data.DbType.String
                };
                var identificacion = new SqlParameter
                {
                    ParameterName = "@IDENTIFICACION",
                    Value = usuario.Identificacion,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    DbType = System.Data.DbType.String
                };

                String sql = "EXEC REGISTER_NEW_USER @USERNAME, @PASSWORD, @NOMBRES, @APELLIDOS, @IDENTIFICACION";

                var c = ctx.Database.ExecuteSqlRaw(sql, username, password, nombres, apellidos, identificacion);
                return true;
            }
        }
    }
}