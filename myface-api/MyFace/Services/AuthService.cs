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
        public string[] userCredentials(string authorizationHeader);
        public ActionResult isUserAdmin(string authorizationHeader);
        public ActionResult isUserMember(string authorizationHeader, int id);
    }
    public class AuthService : IAuthService
    {
        private readonly IUsersRepo _users;
        private readonly IPostsRepo _posts;

        public AuthService(IUsersRepo users, IPostsRepo posts)
        {
            _users = users;
            _posts = posts;
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


        public string[] userCredentials(string authorizationHeader)
        {
            //Decode authorization header and split into username and password.
            string decodedUsernamePassword = AuthHelper.DecodeString(authorizationHeader);
            string[] userCredentialsArray = decodedUsernamePassword.Split(':');
            string username = userCredentialsArray[0];
            string password = userCredentialsArray[1];



            return userCredentialsArray;
        }

        public ActionResult isUserAdmin(string authorizationHeader)
        {
            //Decode authorization header and split into username and password.
            string decodedUsernamePassword = AuthHelper.DecodeString(authorizationHeader);
            string[] userCredentialsArray = decodedUsernamePassword.Split(':');
            string username = userCredentialsArray[0];

            User user = _users.GetUserForAuthentication(username);

            bool isAdmin = user.Role == RoleType.ADMIN;
            if (!isAdmin)
            {
                return new StatusCodeResult(403);
            }
            return null;
        }
        public ActionResult isUserMember(string authorizationHeader, int id)
        {
            //Decode authorization header and split into username and password.
            string decodedUsernamePassword = AuthHelper.DecodeString(authorizationHeader);
            string[] userCredentialsArray = decodedUsernamePassword.Split(':');
            string username = userCredentialsArray[0];

            User user = _users.GetUserForAuthentication(username);
            Post post = _posts.GetUserByPostId(id);
            bool isMember = post.UserId == user.Id;
            if (!isMember)
            {
                return new StatusCodeResult(403);
            }
            return null;
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