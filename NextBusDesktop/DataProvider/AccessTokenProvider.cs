using NextBusDesktop.Models;
using NextBusDesktop.ResponseModels;
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

        public AccessTokenProvider() => _client = new RestClient("https://api.vasttrafik.se/token");

        public async Task<AccessToken> GetAccessTokenAsync()
        {
            Log($"Request -> {nameof(GetAccessTokenAsync)}: Attempting to generate new access token.");
            ApplicationDataContainer localStorage = ApplicationData.Current.LocalSettings;

            var request = new RestRequest();
            _client.Authenticator = new HttpBasicAuthenticator(
                localStorage.Values["Api_ClientId"].ToString(), 
                localStorage.Values["Api_Secret"].ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            var response = await _client.ExecuteTaskAsync<AccessTokenResponse>(request, Method.POST);
            var accessToken = new AccessToken(response.Data);

            Log($"Response -> {nameof(GetAccessTokenAsync)} {response.StatusCode}: Generated new access token (expires {accessToken.ExpiresDateTime}).");

            return accessToken;
        }

        public AccessToken GetAccessToken()
        {
            ApplicationDataContainer localStorage = ApplicationData.Current.LocalSettings;

            var request = new RestRequest();
            _client.Authenticator = new HttpBasicAuthenticator(
                localStorage.Values["Api_ClientId"].ToString(),
                localStorage.Values["Api_Secret"].ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            var response = _client.Execute<AccessTokenResponse>(request, Method.POST);

            return new AccessToken(response.Data);
        }

        private void Log(string message) => System.Diagnostics.Debug.WriteLine($"{nameof(AccessTokenProvider)}: {message}");
    }
}
