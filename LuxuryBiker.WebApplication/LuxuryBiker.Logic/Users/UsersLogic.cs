using LuxuryBiker.Data.CustomTypes.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxuryBiker.Data.Repositry.Users;
using LuxuryBiker.Data.CustomTypes.Helpers;
using Microsoft.AspNetCore.Identity;

namespace LuxuryBiker.Logic.Users
{
    public class UsersLogic
    {
        private readonly UsersRepository _usersRepository;
        public UsersLogic()
        {
            _usersRepository = new UsersRepository();
        }
        public string getPasswordByEmail(string username)
        {
            try
            {
                return _usersRepository.getPasswordByEmail(username);
            } catch (Exception)
            {
                return null;
            }
        }
        public Data.CustomTypes.Users.Users getUserByEmail(string username)
        {
            try
            {
                return _usersRepository.getUserByEmail(username);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Data.CustomTypes.Users.Users getUserById(string idUsuario)
        {
            try
            {
                return _usersRepository.getUserById(idUsuario);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ResponseGeneric<Data.CustomTypes.Users.Users> RegisterNewUser(Data.CustomTypes.Users.Users usuario)
        {
            try
            {
                /* ----------------------- Se valida si el email ingresado ya se encuentra registrado ------------------ */
                var existUser = _usersRepository.getUserByEmail(usuario.UserName);
                if (existUser != null)
                {
                    return new ResponseGeneric<Data.CustomTypes.Users.Users>()
                    {
                        Error = true,
                        Mensaje = "El email ingresado ya se encuentra registrado"
                    };
                }

                /* -------------------------------------- Se trimean datos ---------------------------------------- */
                usuario.UserName = usuario.UserName.Trim();
                usuario.Nombres = usuario.Nombres.Trim();
                usuario.Apellidos = usuario.Apellidos.Trim();
                usuario.Identificacion = usuario.Identificacion.Trim();

                /* ------------------------ Se procede a realizar Hasheo de password y registro de datos ----------------- */
                usuario.PasswordHash = PasswordHash(usuario.UserName, usuario.PasswordHash);
                var resultRegister = _usersRepository.registerNewUser(usuario);

                /* -------------------- Se valida estado de la transaccion y se retorna resultado ---------------- */
                return new ResponseGeneric<Data.CustomTypes.Users.Users>()
                {
                    Error = !resultRegister,
                    Mensaje = resultRegister ? "Usuario creado correctamente" : "Ocurrio un error al registrar el usuario"
                };
            }
            catch (Exception ex)
            {

                return new ResponseGeneric<Data.CustomTypes.Users.Users>()
                {
                    Error = true,
                    Mensaje = "Ocurrrio un error al registrar el usuario"
                };
            }
        }
        string PasswordHash(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
                throw new Exception();

            PasswordHasher<string> pw = new PasswordHasher<string>();
            string passwordHashed = pw.HashPassword(userName, password);
            return passwordHashed;
        }
    }
}