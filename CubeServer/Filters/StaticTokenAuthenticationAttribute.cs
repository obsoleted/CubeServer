namespace CubeServer.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http.Filters;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Microsoft.WindowsAzure;
    using Results;

    public class StaticTokenAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public static readonly string Scheme = "StaticToken";

        public static readonly string AuthenticatedUsername = "StaticTokenUser";

        public static readonly string AuthenticationType = "StaticToken";
 

        public Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return Task.FromResult(0);
            }

            if (authorization.Scheme != Scheme)
            {
                return Task.FromResult(0);
            }

            if (authorization.Parameter != Configuration.StaticToken)
            {
                context.ErrorResult = new UnauthorizedResult(new [] { new AuthenticationHeaderValue(Scheme) }, request);
                return Task.FromResult(0);
            }

            context.Principal = new ClaimsPrincipal(new GenericIdentity(AuthenticatedUsername, AuthenticationType));
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue(Scheme);
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}