﻿using LuxuryBiker.Logic.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Security
{
    public class LoginLogic
    {
        private readonly UsersLogic _usersLogic;
        public LoginLogic()
        {
            _usersLogic = new UsersLogic();
        }
        public Data.CustomTypes.Users.Users CheckLogin(string username, string password)
        {
            try
            {
                Data.CustomTypes.Users.Users usuario = null;
                var validUser = CheckPassword(username, password);
                if (validUser)
                {
                    usuario = _usersLogic.getUserByEmail(username);
                    usuario.Token = GetToken(usuario);
                }

                return usuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool CheckPassword(string userName, string password)
        {
            try
            {
                string passwordDB = null;
                bool loggedIn = false;
                passwordDB = _usersLogic.getPasswordByEmail(userName);
                PasswordHasher<string> pw = new PasswordHasher<string>();
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
        private string GetToken(Data.CustomTypes.Users.Users usuario)
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
                        new Claim(ClaimTypes.Email, usuario.Email)
                    }),
                Expires = DateTime.UtcNow.AddDays(60),
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
    }
}