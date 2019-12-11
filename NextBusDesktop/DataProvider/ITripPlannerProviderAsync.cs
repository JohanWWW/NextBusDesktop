using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.DataProvider
{
    public interface ITripPlannerProviderAsync
    {
        Task<LocationList> GetLocationListAsync(string query);
        Task<DepartureBoard> GetDepartureBoardAsync(string stopId);
        Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime);
    }
}
