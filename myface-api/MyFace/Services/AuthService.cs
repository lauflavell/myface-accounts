using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Models.Database;
using MyFace.Repositories;
using MyFace.Helpers;

namespace MyFace.Services
{
    public interface IAuthService
    {
        public bool AuthenticateUser(string authorizationHeader);
        public ActionResult Authenticate(string authorizationHeader);
        public string [] userCredentials(string authorizationHeader);
    }
    public class AuthService : IAuthService
    {
        private readonly IUsersRepo _users;

        public AuthService(IUsersRepo users)
        {
            _users = users;
        }

        public ActionResult Authenticate(string authorizationHeader)
        {
            if (authorizationHeader is null)
            {
                return new UnauthorizedResult();
            }
            try
            {
                bool isAuthenticated = AuthenticateUser(authorizationHeader);
                if (!isAuthenticated)
                {
                    return new UnauthorizedResult();
                }
            }
            catch (System.InvalidOperationException)
            {
                return new UnauthorizedResult();
            }
            return null;
        }

       
        public string [] userCredentials(string authorizationHeader){
           //Decode authorization header and split into username and password.
            string decodedUsernamePassword = AuthHelper.DecodeString(authorizationHeader);
            string[] authDetailsArray = decodedUsernamePassword.Split(':');
            string username = authDetailsArray[0];
            string password = authDetailsArray[1];
            return authDetailsArray;
        }
        public bool AuthenticateUser(string authorizationHeader)
        {
            //Decode authorization header and split into username and password.
            string decodedUsernamePassword = AuthHelper.DecodeString(authorizationHeader);
            string[] authDetailsArray = decodedUsernamePassword.Split(':');
            string username = authDetailsArray[0];
            string password = authDetailsArray[1];

            //Get user information for the username supplied by authorization header.
            User user = _users.GetUserForAuthentication(username);

            //Hash the password supplied by the authorization header.
            string userHashedPassword = HashHelper.HashPassword(password, user.Salt);

            //If the supplied password matches the hased password in database retrun true.
            return userHashedPassword == user.HashedPassword;


        }
    }
}