using CaracalGateway.Infrastructure;
using CaracalGateway.Infrastructure.Helpers;
using System.Text;
using System.Text.Json;

namespace CaracalGateway.DelegatingHandlers
{
    public class BaseApiTokenCheckDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public BaseApiTokenCheckDelegatingHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, string endpointKey)
        {
            var apiTokenParam = HttpHelper.GetQueryParamValue(request.RequestUri.Query, HttpHelper.ApiTokenKey);

            var getTokenServiceResult = await _tokenService.GetAccessTokenAsync(apiTokenParam);

            if (!getTokenServiceResult.Success)
            {
                var errorMessage = JsonSerializer.Serialize(getTokenServiceResult);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(errorMessage, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json)
                };
            }

            var validateTokenServiceResult = _tokenService.ValidateAccessToken(getTokenServiceResult.Data, endpointKey);
            
            if (!validateTokenServiceResult.Success)
            {
                var errorMessage = JsonSerializer.Serialize(validateTokenServiceResult);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(errorMessage, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json)
                };
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await _tokenService.DecrementRequestCountFromToken(getTokenServiceResult.Data, endpointKey);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
