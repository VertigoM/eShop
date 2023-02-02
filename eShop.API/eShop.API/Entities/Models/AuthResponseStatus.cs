namespace eShop.API.Entities.Models
{
    public class AuthStatus
    {
        public LoginStatus LoginStatus { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }

    public enum LoginStatus
    {
        NoRoleToUser = 0,
        LoginFailed = 1,
        LoginSuccessful = 2,
    }

    public class ResponseStatus
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
