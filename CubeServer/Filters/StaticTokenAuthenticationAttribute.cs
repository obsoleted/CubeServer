﻿namespace CubeServer.Filters
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
        private const string StaticTokenAuthenticationScheme = "StaticToken";

        public async Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != StaticTokenAuthenticationScheme)
            {
                return;
            }

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new UnauthorizedResult(new[] { new AuthenticationHeaderValue(StaticTokenAuthenticationScheme) }, request);
                return;
            }

            if (authorization.Parameter != Configuration.StaticToken)
            {
                context.ErrorResult = new UnauthorizedResult(new [] { new AuthenticationHeaderValue(StaticTokenAuthenticationScheme) }, request);
                return;
            }

            context.Principal = new ClaimsPrincipal(new GenericIdentity("StaticTokenUser", "StaticToken"));
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue(StaticTokenAuthenticationScheme);
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}