using System;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.Models
{
    public class AccessToken
    {
        public string Scope { get; set; }
        public string Type { get; set; }
        public Guid Token { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ExpiresDateTime { get; set; }

        public AccessToken(AccessTokenResponse accessTokenResponseModel)
        {
            Scope = accessTokenResponseModel.Scope;
            Type = accessTokenResponseModel.TokenType;
            Token = accessTokenResponseModel.Token;
            DateTime now = DateTime.Now;
            CreatedDateTime = now;
            ExpiresDateTime = now.AddSeconds(accessTokenResponseModel.Expires);
        }

        // Used on file retrieval
        public AccessToken()
        {
        }
    }
}
