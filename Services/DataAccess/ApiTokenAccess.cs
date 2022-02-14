using Microsoft.Extensions.Logging;
using OneAuthenticate;
using OneConnector.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneConnector.Services.DataAccess
{
    public sealed class ApiTokenAccess
    {
        private ILogger<ApiTokenAccess> Logger { get; }
        public string ApiToken { get; }
        public OneSoap<AuthenticateSoap> OneAuthenticateSoap { get; }

        public ApiTokenAccess(
            OneSoap<AuthenticateSoap> oneAuthenticateSoap,
            ILogger<ApiTokenAccess> logger
        )
        {
            OneAuthenticateSoap = oneAuthenticateSoap;
            Logger = logger;

            using (DisposableWrapper<AuthenticateSoap> authenticateDisposableClient = OneAuthenticateSoap.DisposableClient)
            {
                AuthenticateSoap authenticateClient = authenticateDisposableClient.Base;

                CreateApiKeyResponse authenticateResponse = authenticateClient.CreateApiKey(
                    new()
                    {
                        Body = new()
                        {
                            _password = OneAuthenticateSoap.Password,
                            _username = OneAuthenticateSoap.User
                        }
                    }
                );
                string apiKey = authenticateResponse.Body.CreateApiKeyResult;

                GetAuthenticationTokenResponse tokenResponse = authenticateClient.GetAuthenticationToken(
                    new()
                    {
                        Body = new()
                        {
                            _apiKey = apiKey
                        }
                    }
                );
                ApiToken = tokenResponse.Body.GetAuthenticationTokenResult;
            }
        }
    }
}
