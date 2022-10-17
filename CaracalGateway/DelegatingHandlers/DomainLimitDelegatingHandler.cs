using CaracalGateway.Infrastructure;

namespace CaracalGateway.DelegatingHandlers
{
    public class DomainLimitDelegatingHandler : BaseApiTokenCheckDelegatingHandler
    {
        public DomainLimitDelegatingHandler(ITokenService tokenService) : base(tokenService)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri.Host;
            return await SendAsync(request, cancellationToken, uri);
        }
    }
}
