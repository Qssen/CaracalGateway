namespace CaracalGateway.Model
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public Dictionary<string, int> EndpointToRequestsCount { get; set; } = new Dictionary<string, int>();
        public TokenKind Kind { get; set; } = TokenKind.User;
    }

    public enum TokenKind
    {
        Developer,
        User
    }
}
