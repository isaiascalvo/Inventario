using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Auth
{
    public class AuthService: IAuthService
    {
        IHttpContextAccessor _httpContextAccessor;

        public AuthService()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        public string GetCurrentUserId()
        {
            return GetSubFromToken(1);
        }

        private string GetSubFromToken(int subNumber)
        {
            var YY = _httpContextAccessor.HttpContext.Request.Headers;
            var allSubs = _httpContextAccessor.HttpContext.User.FindAll("sub");
            return allSubs.Count() < subNumber ? string.Empty : allSubs.ElementAt(subNumber - 1)?.Value;
        }
    }
}
