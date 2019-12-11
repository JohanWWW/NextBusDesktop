using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.DataProvider
{
    public interface IApiHelper
    {
        System.Net.Http.HttpClient Client { get; }
        void InitializeClient();
    }
}
