using CaracalGateway.Infrastructure;

namespace CaracalGateway.DelegatingHandlers
{
    public class EndpointApiTokenCheckDelegatingHandler : BaseApiTokenCheckDelegatingHandler
    {
        public EndpointApiTokenCheckDelegatingHandler(ITokenService tokenService) : base(tokenService)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uri = request.RequestUri.GetLeftPart(UriPartial.Path);
        return await SendAsync(request, cancellationToken, uri);
    }
}
}
