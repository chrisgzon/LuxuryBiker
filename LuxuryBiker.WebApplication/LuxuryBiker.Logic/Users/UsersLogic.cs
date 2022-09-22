using LuxuryBiker.Data.CustomTypes.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxuryBiker.Data.Repositry.Users;

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
    }
}