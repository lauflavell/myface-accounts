using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyFace.Models.Database;
using MyFace.Models.Request;
using Microsoft.AspNetCore.Mvc;


namespace MyFace.Helpers
{
   public static class AuthHelper
    {
        public static string DecodeString(string authorizationHeader)
        {
            string encodedUsernamePassword = authorizationHeader.Substring("Basic ".Length).Trim();
            var base64EncodedBytes = System.Convert.FromBase64String(encodedUsernamePassword);
            var decodedUsernamePassword = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            return decodedUsernamePassword;
        }
        
    }
}