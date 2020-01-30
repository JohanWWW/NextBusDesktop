using System.Threading.Tasks;
using System;
using NextBusDesktop.Models;

namespace NextBusDesktop.DataProvider
{
    public interface IAccessTokenProvider
    {
        Task<AccessToken> GetAccessTokenAsync();
    }
}
