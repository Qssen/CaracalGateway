using System.Web;

namespace CaracalGateway.Infrastructure.Helpers
{
    public static class HttpHelper
    {
        public const string ApiTokenKey = "caracal-token";
        public static string GetQueryParamValue(string queryString, string key)
        {
            if (!queryString.Contains(key))
                return null;

            var parsedQueryString = HttpUtility.ParseQueryString(queryString);
            if (parsedQueryString == null)
                return null;

            return parsedQueryString.Get(key);
        }
    }
}
