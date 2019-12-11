using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.DataProvider
{
    public interface ITripPlannerProvider
    {
        LocationList GetLocationList(string query);
        DepartureBoard GetDepartureBoard(string stopId);
        DepartureBoard GetDepartureBoard(string stopId, DateTime dateTime);     
    }
}
