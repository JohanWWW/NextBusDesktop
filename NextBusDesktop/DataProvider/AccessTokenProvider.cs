using NextBusDesktop.Models;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace NextBusDesktop.DataProvider
{
    /// <summary>
    /// Provides an access token for <see cref="TripPlannerProvider"/>
    /// </summary>
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IRestClient _client;
        private ILog _logger;

        public ILog Logger
        {
            set => _logger = value;
        }

        public AccessTokenProvider() => _client = new RestClient("https://api.vasttrafik.se/token");

        public async Task<AccessToken> GetAccessTokenAsync()
        {
            Log($"{nameof(GetAccessTokenAsync)}: Requesting access token", "Request");
            ApplicationDataContainer localStorage = ApplicationData.Current.LocalSettings;

            var request = new RestRequest();
            _client.Authenticator = new HttpBasicAuthenticator(
                localStorage.Values["Api_ClientId"].ToString(), 
                localStorage.Values["Api_Secret"].ToString()
            );
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            var response = await _client.ExecuteTaskAsync<AccessTokenResponse>(request, Method.POST);
            var accessToken = new AccessToken(response.Data);

            Log($"{nameof(GetAccessTokenAsync)}: {response.StatusCode}");

            return accessToken;
        }

        private void Log(string message) => _logger?.Log(message);

        private void Log(string message, string category) => _logger?.Log(message, category);
    }
}
