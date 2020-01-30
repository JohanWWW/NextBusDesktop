using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class AccessTokenResponse
    {
        [DeserializeAs(Name = "scope")] public string Scope { get; set; }
        [DeserializeAs(Name = "token_type")] public string TokenType { get; set; }
        [DeserializeAs(Name = "expires_in")] public int Expires { get; set; }
        [DeserializeAs(Name = "access_token")] public Guid Token { get; set; }
    }
}
