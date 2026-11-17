namespace CatalogClientApp.Models
{
    public class LoginResult
    {
        public string Token { get; set; } = "";
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime ExpiresUtc { get; set; }
    }
}

