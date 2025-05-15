using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eUseControl.BusinessLogic.Core
{
    public class UserApi
    {
        public string GetUserName(int userId)
        {
            
            return "User" + userId;
        }

        public bool AuthenticateUser(string username, string password)
        {
            
            return username == "admin" && password == "password";
        }

        public bool RegisterUser(string username, string password)
        {
            
            return true;
        }
    }

}
