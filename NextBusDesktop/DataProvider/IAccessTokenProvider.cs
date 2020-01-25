using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;
using NextBusDesktop.Models;
using RestSharp;

namespace NextBusDesktop.DataProvider
{
    public interface IAccessTokenProvider
    {
        Task<AccessToken> GetAccessTokenAsync();
    }
}
