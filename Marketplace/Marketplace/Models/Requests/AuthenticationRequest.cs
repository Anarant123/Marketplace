namespace Marketplace.Models.Requests
{
    public partial class UserAPIHandler
    {
        public class AuthenticationRequest
        {
            public string Email { get; set; }
            public string PasswordHash { get; set; }
        }
    }
}
