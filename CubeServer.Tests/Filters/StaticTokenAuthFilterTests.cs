using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CubeServer.Tests.Filters
{
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.Results;
    using System.Web.Http.Routing;
    using CubeServer.Filters;

    [TestClass]
    public class StaticTokenAuthFilterTests
    {
        /// <summary>
        /// Creates the most basic HttpAuthenticationContext
        /// </summary>
        /// <returns></returns>
        private HttpAuthenticationContext CreateTestAuthContext()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            HttpControllerContext controllerContext = new HttpControllerContext();
            controllerContext.Request = request;
            HttpActionContext context = new HttpActionContext();
            context.ControllerContext = controllerContext;
            HttpAuthenticationContext authContext = new HttpAuthenticationContext(context, null);
            return authContext;
        }



        [TestMethod]
        public async Task NoAuthHeaderIsIgnored()
        {
            var authContext = CreateTestAuthContext();
            var staticTokenFilter = new StaticTokenAuthenticationAttribute();
            var cts = new CancellationTokenSource();
            await staticTokenFilter.AuthenticateAsync(authContext, cts.Token);

            Assert.IsNull(authContext.Principal, "Principal should not be set");
            Assert.IsNull(authContext.ErrorResult, "Error result should be null");
        }

        [TestMethod]
        public async Task DifferentAuthSchemeIsIgnored()
        {
            var authContext = CreateTestAuthContext();
            authContext.Request.Headers.Authorization = new AuthenticationHeaderValue(StaticTokenAuthenticationAttribute.Scheme + "delta");
            var staticTokenFilter = new StaticTokenAuthenticationAttribute();
            var cts = new CancellationTokenSource();
            await staticTokenFilter.AuthenticateAsync(authContext, cts.Token);

            Assert.IsNull(authContext.Principal, "Principal should not be set");
            Assert.IsNull(authContext.ErrorResult, "Error result should be null");
        }

        [TestMethod]
        public async Task MissingAuthParameterIsRejected()
        {
            var authContext = CreateTestAuthContext();
            authContext.Request.Headers.Authorization = new AuthenticationHeaderValue(StaticTokenAuthenticationAttribute.Scheme);
            var staticTokenFilter = new StaticTokenAuthenticationAttribute();
            var cts = new CancellationTokenSource();
            await staticTokenFilter.AuthenticateAsync(authContext, cts.Token);

            Assert.IsNull(authContext.Principal, "Principal should not be set");
            Assert.IsNotNull(authContext.ErrorResult, "Error result should be set");
            var unauthorizedResult = authContext.ErrorResult as UnauthorizedResult;
            Assert.IsNotNull(unauthorizedResult, "Error result was not an instnace of UnauthorizedResult");
            Assert.IsTrue(
                unauthorizedResult.Challenges.Contains(
                    new AuthenticationHeaderValue(StaticTokenAuthenticationAttribute.Scheme)));
        }

        [TestMethod]
        public async Task IncorrectAuthParameterIsRejected()
        {
            var authContext = CreateTestAuthContext();
            authContext.Request.Headers.Authorization = new AuthenticationHeaderValue(StaticTokenAuthenticationAttribute.Scheme, Configuration.StaticToken + "delta");
            var staticTokenFilter = new StaticTokenAuthenticationAttribute();
            var cts = new CancellationTokenSource();
            await staticTokenFilter.AuthenticateAsync(authContext, cts.Token);

            Assert.IsNull(authContext.Principal, "Principal should not be set");
            Assert.IsNotNull(authContext.ErrorResult, "Error result should be set");

        }

        [TestMethod]
        public async Task CorrectAuthSetsPrincipal()
        {
            var authContext = CreateTestAuthContext();
            authContext.Request.Headers.Authorization = new AuthenticationHeaderValue(StaticTokenAuthenticationAttribute.Scheme, Configuration.StaticToken);
            var staticTokenFilter = new StaticTokenAuthenticationAttribute();
            var cts = new CancellationTokenSource();
            await staticTokenFilter.AuthenticateAsync(authContext, cts.Token);

            Assert.IsNotNull(authContext.Principal, "Principal should be set");
            Assert.AreEqual(StaticTokenAuthenticationAttribute.AuthenticatedUsername, authContext.Principal.Identity.Name, "Identity name");
            Assert.AreEqual(StaticTokenAuthenticationAttribute.AuthenticationType, authContext.Principal.Identity.AuthenticationType);
            Assert.IsTrue(authContext.Principal.Identity.IsAuthenticated, "IsAuthenticated");
            Assert.IsNull(authContext.ErrorResult, "Error result should be set");
        }




    }
}
