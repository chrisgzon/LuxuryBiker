using LuxuryBiker.Data.CustomTypes.Helpers;
using LuxuryBiker.Data.CustomTypes.Users;
using LuxuryBiker.Logic.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LuxuryBiker.web.Security
{
    public class LoginLogic
    {
        private readonly UsersLogic _usersLogic;
        public LoginLogic()
        {
            _usersLogic = new UsersLogic();
        }
        public ResponseGeneric<User> CheckLogin(string username, string password, bool rememberme)
        {
            try
            {
                username = username.Trim();
                /* -------------------------------- Se valida si las credenciales son correctas ------------------------*/
                var validUser = CheckPassword(username, password);
                if (!validUser)
                {
                    return new ResponseGeneric<User>()
                    {
                        Error = true,
                        Mensaje = "Usuario y/o Contraseña invalidas"
                    };
                }

                /* ------------------------ Se obtienen datos del usuario autenticado y se crea Token con JWT -------------- */
                var usuario = _usersLogic.getUserByEmail(username);
                usuario.Token = GetToken(usuario, rememberme);
                return new ResponseGeneric<User>()
                {
                    Error = false,
                    Result = usuario
                };
            }
            catch (Exception)
            {
                return new ResponseGeneric<User>()
                {
                    Error = true,
                    Mensaje = "Error al realizar el ingreso"
                };
            }
        }
        public bool CheckPassword(string userName, string password)
        {
            try
            {
                string passwordDB = null;
                bool loggedIn = false;

                /* ---------------- Se obtiene contraseña registrada en BD del username que se haya recibido desde el login ------- */
                passwordDB = _usersLogic.getPasswordByEmail(userName);
                PasswordHasher<string> pw = new PasswordHasher<string>();

                /* ----------------------------- Se valida si las credenciales son correctas --------------------------------- */
                var verificationResult = pw.VerifyHashedPassword(userName, passwordDB, password);
                if (verificationResult == PasswordVerificationResult.Success)
                    loggedIn = true;
                else
                    loggedIn = false;
                return loggedIn;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string GetToken(User usuario, bool rememberme)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyToken = "1a2b3c4d5e6f7g8h9qwerty";
            var bytesKeyToken = Encoding.ASCII.GetBytes(keyToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario),
                        new Claim(ClaimTypes.Name, usuario.UserName)
                    }),
                Expires = rememberme ? DateTime.UtcNow.AddDays(2) : DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytesKeyToken), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        string PasswordHash(string userName, string password)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();
            string passwordHashed = pw.HashPassword(userName, password);
            return passwordHashed;
        }
        public ResponseGeneric<User> Whoami()
        {
            try
            {
                /*---------------------------------- Se busca IdUsuario en cookies -----------------------------*/
                var user = new HttpContextAccessor().HttpContext.User;
                var idUsuario = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                
                /*-------------------------- No se encontro un usuario autenticado -----------------------------*/
                if (String.IsNullOrEmpty(idUsuario)) {
                    return new ResponseGeneric<User>()
                    {
                        Error = true,
                        Mensaje = "No hay un usario autenticado"
                    };
                }

                /*--------------------------- Se Obtienen datos del usuario logueado para retornarlos --------------*/
                var usuario = _usersLogic.getUserById(idUsuario);
                usuario.Claims = user.Claims.Select(s => new Claim(s.Type, s.Value)).ToList();

                return new ResponseGeneric<User>()
                {
                    Error = false,
                    Result = usuario
                };
            }
            catch (Exception)
            {

                return new ResponseGeneric<User>()
                {
                    Error = true,
                    Mensaje = "Ocurrio un error al consultar cookies"
                };
            }
        }
    }
}