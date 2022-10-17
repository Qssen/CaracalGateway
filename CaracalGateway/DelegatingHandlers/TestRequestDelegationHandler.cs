using CaracalGateway.Infrastructure;
using CaracalGateway.Model;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace CaracalGateway.DelegatingHandlers
{
    /// <summary>
    /// Test DelegatingHandler, required for basic healthcheck 
    /// </summary>
    public class TestRequestDelegationHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        public TestRequestDelegationHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            #region test
            var accessToken = new AccessToken()
            {
                Token = "749fead8-1894-4d02-9507-b5e2c1b03fcf",
                ExpireDate = DateTime.ParseExact("20.12.2029", "dd.MM.yyyy", CultureInfo.CurrentCulture),
                IssuedDate = DateTime.Now,
                Kind = TokenKind.Developer,
                UserName = "Kussen",
                EndpointToRequestsCount = new Dictionary<string, int>()
                {
                    { "jsonplaceholder.typicode.com", 2 }
                }
            };

            await _tokenService.CreateAccessTokenAsync(accessToken);
            #endregion

            var res = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var responseObject = new { Status = "OK" };
            res.Content = new StringContent(JsonSerializer.Serialize(responseObject), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);
            return res;
        }
    }
}
