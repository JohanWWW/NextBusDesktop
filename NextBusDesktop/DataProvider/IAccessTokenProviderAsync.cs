using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.DataProvider
{
    public interface IAccessTokenProviderAsync
    {
        Task<AccessToken> GetAccessTokenAsync();
    }
}
