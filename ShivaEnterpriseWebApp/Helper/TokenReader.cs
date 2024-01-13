using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShivaEnterpriseWebApp.Helper
{
    public static class TokenReader
    {
        public static string GetExpDateTimeFromAuthToken(string authToken)
        {
            var stream = authToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            string expDateTime = tokenS.Claims.First(claim => claim.Type == "exp").Value;

            return expDateTime;
        }

        public static string GetAuthToken(this HttpContext httpContext)
        {
            string authToken = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash)?.Value;
            return authToken;
        }
    }
}
