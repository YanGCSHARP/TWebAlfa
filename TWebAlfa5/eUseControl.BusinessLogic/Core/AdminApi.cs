using System;
using System.Collections.Generic;
using System.Linq;

using eUseControl.Helpers;

namespace eUseControl.BusinessLogic.Core
{
    public class AdminApi
    {
        public bool Login(string username, string password)
        {
           
            return true;
        }

        public bool Logout()
        {
            
            return true;
        }

        public List<string> GetUsers()
        {
                
            return new List<string> { "User1", "User2" };
        }
    }

}
