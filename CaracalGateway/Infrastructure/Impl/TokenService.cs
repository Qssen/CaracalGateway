using CaracalGateway.Model;
using Ocelot.Responses;

namespace CaracalGateway.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IKeyValueStorage _keyValueStorage;

        public TokenService(IKeyValueStorage keyValueStorage)
        {
            _keyValueStorage = keyValueStorage;
        }

        public async Task<ServiceResponse> CreateAccessTokenAsync(AccessToken accessToken)
        {
            var response = new ServiceResponse();
            try
            {
                await _keyValueStorage.AddEntranceAsync(accessToken.Token, accessToken);
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Error = $"Unhandled Exception. {e.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<AccessToken>> GetAccessTokenAsync(string token)
        {
            var response = new ServiceResponse<AccessToken>();
            
            if (string.IsNullOrEmpty(token))
            {
                response.Success = false;
                response.Error = $"Error. No token provided";
                return response;
            }

            var accessToken = await _keyValueStorage.GetValueAsync<AccessToken>(token);
           
            if(accessToken == null)
            {
                response.Success = false;
                response.Error = $"Could not find access token";
                return response;
            }
            response.Data = accessToken;
            return response;
        }
        public async Task<ServiceResponse> DecrementRequestCountFromToken(AccessToken accessToken, string endpointKey)
        {
            if(accessToken.EndpointToRequestsCount.TryGetValue(endpointKey, out var count))
            {
                accessToken.EndpointToRequestsCount[endpointKey]--;

                return await CreateAccessTokenAsync(accessToken);
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Error = $"Endpoint could not be find"
                };
            }
        }

        public ServiceResponse ValidateAccessToken(AccessToken accessToken, string endpoint)
        {
            var response = new ServiceResponse();
            var dtNow = DateTime.Now;
            if (accessToken.ExpireDate <= dtNow)
            {
                response.Success = false;
                response.Error = $"Token is expired";
            }
            if (accessToken.IssuedDate > dtNow)
            {
                response.Success = false;
                response.Error = $"Token is not activated yet";
            }
            var endpointFound = accessToken.EndpointToRequestsCount.TryGetValue(endpoint, out var requestsCount);
            if (!endpointFound)
            {
                response.Success = false;
                response.Error = $"User has no access to given endpoint";
            }
            if(endpointFound && requestsCount <= 0)
            {
                response.Success = false;
                response.Error = $"User has reached limit to given endpoint";
            }
            return response;
        }
    }
}
