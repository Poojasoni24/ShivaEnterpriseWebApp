namespace ShivaEnterpriseWebApp.DTOs
{
    public class AuthDAOs
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresTimeStamp { get; set; }
        public string PreferredLanguageCode { get; set; }
        public List<string> Roles { get; set; }
    }
}
