using System;
using System.Web;
using System.Web.Security;
using LNP.Core.Interfaces.Services;

namespace LNP.BuisnessLogic.Services
{
    public class UserService: IUserService
    {
        public Guid GetCurrentUserId()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated");

            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                throw new UnauthorizedAccessException("Authentication cookie not found");

            try
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || string.IsNullOrEmpty(ticket.UserData))
                    throw new InvalidOperationException("Invalid authentication ticket");

                var userDataParts = ticket.UserData.Split('|');
                if (userDataParts.Length < 1)
                    throw new InvalidOperationException("User data format incorrect");

                if (!Guid.TryParseExact(userDataParts[0], "N", out var userId))
                    throw new InvalidOperationException("Invalid GUID format in user data");

                return userId;
            }
            catch (Exception ex)
            {
                throw new UserContextException("Error retrieving user data", ex);
            }
        }
    }

    public class UserContextException : Exception
    {
        public UserContextException(string message, Exception inner) 
            : base(message, inner) { }
    }
}