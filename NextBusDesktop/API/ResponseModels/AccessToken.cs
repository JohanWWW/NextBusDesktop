using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.API.ResponseModels
{
    public class AccessToken
    {
        [DeserializeAs(Name = "scope")] public string Scope { get; set; }
        [DeserializeAs(Name = "token_type")] public string TokenType { get; set; }
        [DeserializeAs(Name = "expires_in")] public int Expires { get; set; }
        [DeserializeAs(Name = "access_token")] public Guid Token { get; set; }
    }
}
