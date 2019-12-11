using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.DataProvider
{
    public interface IServerDateTimeParser
    {
        DateTime Parse(string date, string time);
    }
}
