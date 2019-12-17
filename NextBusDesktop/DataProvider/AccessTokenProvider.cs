using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using RestSharp;
using RestSharp.Authenticators;
using Windows.Storage;

namespace NextBusDesktop.DataProvider
{
    /// <summary>
    /// Provides an access token for <see cref="TripPlannerProvider"/>
    /// </summary>
    public class AccessTokenProvider : IAccessTokenProvider, IAccessTokenProviderAsync
    {
        private readonly IRestClient _client;

        public AccessTokenProvider() => _client = new RestClient("https://api.vasttrafik.se/token");

        public async Task<AccessTokenResponse> GetAccessTokenAsync()
        {
            ApplicationDataContainer localStorage = ApplicationData.Current.LocalSettings;

            IRestRequest request = new RestRequest();
            _client.Authenticator = new HttpBasicAuthenticator(
                localStorage.Values["Api_ClientId"].ToString(), 
                localStorage.Values["Api_Secret"].ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            IRestResponse<AccessTokenResponse> result = await _client.ExecuteTaskAsync<AccessTokenResponse>(request, Method.POST);

            return result.Data;
        }

        public AccessTokenResponse GetAccessToken()
        {
            ApplicationDataContainer localStorage = ApplicationData.Current.LocalSettings;

            IRestRequest request = new RestRequest();
            _client.Authenticator = new HttpBasicAuthenticator(
                localStorage.Values["Api_ClientId"].ToString(),
                localStorage.Values["Api_Secret"].ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            IRestResponse<AccessTokenResponse> response = _client.Execute<AccessTokenResponse>(request, Method.POST);

            return response.Data;
        }
    }
}
