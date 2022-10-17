using CaracalGateway.Model;

namespace CaracalGateway.Infrastructure
{
    public interface ITokenService
    {
        Task<ServiceResponse<AccessToken>> GetAccessTokenAsync(string token);
        Task<ServiceResponse> CreateAccessTokenAsync(AccessToken accessToken);
        // validates - if token is expired, invalid or not valid for this endpoint  
        ServiceResponse ValidateAccessToken(AccessToken accessToken, string endpointKey);
        Task<ServiceResponse> DecrementRequestCountFromToken(AccessToken accessToken, string endpointKey);
    }
}
