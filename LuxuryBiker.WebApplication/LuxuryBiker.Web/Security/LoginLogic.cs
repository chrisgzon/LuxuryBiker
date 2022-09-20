using LuxuryBiker.Logic.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    usuario = _usersLogic.getUserByEmail(username);

                return usuario;
            }
            catch (Exception)
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
        string PasswordHash(string userName, string password)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();
            string passwordHashed = pw.HashPassword(userName, password);
            return passwordHashed;
        }
    }
}