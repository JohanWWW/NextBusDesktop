using NextBusDesktop.Models;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.Models.TripPlanner;
using NextBusDesktop.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.DataProvider
{
    public class TripPlannerProviderMock : ITripPlannerProvider
    {
        public bool IsAccessTokenExpired => false;

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId)
        {
            throw new NotImplementedException();
        }

        public async Task<DepartureBoard> GetDepartureBoardAsync(string stopId, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public async Task<LocationList> GetLocationListAsync(string query)
        {
            await Task.Delay(10000);

            return new LocationList(new ResponseModels.LocationListResponse
            {
                ServerDate = "2020-01-24",
                ServerTime = "08:26",
                StopLocations = new[]
                {
                    new StopLocationResponse
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Street name, City",
                        Index = 1
                    },
                    new StopLocationResponse
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Göteborg Central, Göteborg",
                        Index = 2
                    },
                    new StopLocationResponse
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Svingeln, Göteborg",
                        Index = 3
                    },
                }
            });
        }

        public async Task<TripList> GetTripListAsync(string originStopId, string destinationStopId)
        {
            throw new NotImplementedException();
        }

        public async Task<TripList> GetTripListAsync(string originStopId, string destinationStopId, DateTime dateTime, bool isSearchForArrival = false)
        {
            throw new NotImplementedException();
        }


        public void SetToken(AccessToken newToken)
        {
        }
    }
}
