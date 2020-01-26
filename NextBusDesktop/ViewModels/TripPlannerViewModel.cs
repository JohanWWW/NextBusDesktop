using NextBusDesktop.DataProvider;
using NextBusDesktop.Models;
using NextBusDesktop.Models.TripPlanner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NextBusDesktop.ViewModels
{
    public class TripPlannerViewModel : ViewModelBase
    {
        private CurrentFocus _currentFocus;
        private IEnumerable<TripViewModel> _cachedTrips;

        private DateTime _givenDate;
        public DateTime GivenDate
        {
            get => _givenDate;
            set => SetProperty(ref _givenDate, value);
        }

        private TimeSpan _givenTime;
        public TimeSpan GivenTime
        {
            get => _givenTime;
            set => SetProperty(ref _givenTime, value);
        }

        private bool _isGivenDateTimeForArrivals;
        /// <summary>
        /// Given date and time represents arrival time.
        /// If false it represents departure time.
        /// </summary>
        public bool IsGivenDateTimeForArrivals
        {
            get => _isGivenDateTimeForArrivals;
            set => SetProperty(ref _isGivenDateTimeForArrivals, value);
        }

        private bool _isSearchResultPaneOpen;
        public bool IsSearchResultPaneOpen
        {
            get => _isSearchResultPaneOpen;
            set => SetProperty(ref _isSearchResultPaneOpen, value);
        }


        private string _originSearchQuery;
        public string OriginSearchQuery
        {
            get => _originSearchQuery;
            set => SetProperty(ref _originSearchQuery, value);
        }

        private string _destinationSearchQuery;
        public string DestinationSearchQuery
        {
            get => _destinationSearchQuery;
            set => SetProperty(ref _destinationSearchQuery, value);
        }

        private ObservableCollection<StopLocationViewModel> _originStopLocations;
        public ObservableCollection<StopLocationViewModel> OriginStopLocations
        {
            get => _originStopLocations;
            set => SetProperty(ref _originStopLocations, value);
        }

        private StopLocation _origin;
        public StopLocation Origin
        {
            get => _origin;
            set => SetProperty(ref _origin, value);
        }

        private ObservableCollection<StopLocationViewModel> _destinationStopLocations;
        public ObservableCollection<StopLocationViewModel> DestinationStopLocations
        {
            get => _destinationStopLocations;
            set => SetProperty(ref _destinationStopLocations, value);
        }

        private StopLocation _destination;
        public StopLocation Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        private ObservableCollection<StopLocationViewModel> _stopLocations;
        public ObservableCollection<StopLocationViewModel> StopLocations
        {
            get => _stopLocations;
            set => SetProperty(ref _stopLocations, value);
        }

        private StopLocation _selectedStopLocation;
        public StopLocation SelectedStopLocation
        {
            get => _selectedStopLocation;
            set
            {
                if (_stopLocations != null)
                    SetProperty(ref _selectedStopLocation, value);
            }
        }

        private int _selectedStopLocationIndex;
        public int SelectedStopLocationIndex
        {
            get => _selectedStopLocationIndex;
            set
            {
                SetProperty(ref _selectedStopLocationIndex, value);

                if (value is -1) _selectedStopLocation = null;
                else
                {
                    _selectedStopLocation = _stopLocations.ElementAt(value);
                    switch (_currentFocus)
                    {
                        case CurrentFocus.Origin:
                            OriginSearchQuery = _selectedStopLocation.Name;
                            _origin = _selectedStopLocation;
                            break;
                        case CurrentFocus.Destination:
                            DestinationSearchQuery = _selectedStopLocation.Name;
                            _destination = _selectedStopLocation;
                            break;
                        default:
                            throw new NotImplementedException($"No case provided for '{_currentFocus}'.");
                    }
                    IsSearchResultPaneOpen = false;
                }
            }
        }

        private ObservableCollection<TripViewModel> _trips;
        public ObservableCollection<TripViewModel> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }

        public TripPlannerViewModel()
        {
            _currentFocus = CurrentFocus.Destination;
            var dateTime = DateTime.Now;
            _givenDate = dateTime.Date;
            _givenTime = dateTime.TimeOfDay;
            _originStopLocations = new ObservableCollection<StopLocationViewModel>();
            _destinationStopLocations = new ObservableCollection<StopLocationViewModel>();
            _stopLocations = new ObservableCollection<StopLocationViewModel>();
            _selectedStopLocationIndex = -1;
            _trips = new ObservableCollection<TripViewModel>();
        }

        protected override void Deconstruct()
        {
        }

        public async Task GetOriginLocationList()
        {
            IsLoading = true;

            LocationList originSearchResult = null;
            try
            {
                originSearchResult = await GetLocationList(_originSearchQuery);
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"Failed to get location list for origin. Exception: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            StopLocations.Clear();

            foreach (var stop in originSearchResult.StopLocations.ToList())
            {
                var stopLocationVm = new StopLocationViewModel(stop);
                StopLocations.Add(stopLocationVm);
            }

            IsSearchResultPaneOpen = true;

            if (_currentFocus != CurrentFocus.Origin)
                _currentFocus = CurrentFocus.Origin;
        }

        public async Task GetDestinationLocationList()
        {
            IsLoading = true;

            LocationList destinationSearchResult = null;
            try
            {
                destinationSearchResult = await GetLocationList(_destinationSearchQuery);
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"Failed to get location list for destination. Exception: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            StopLocations.Clear();

            foreach (var stop in destinationSearchResult.StopLocations.ToList())
            {
                var stopLocationVm = new StopLocationViewModel(stop);
                StopLocations.Add(stopLocationVm);
            }

            IsSearchResultPaneOpen = true;

            if (_currentFocus != CurrentFocus.Destination)
                _currentFocus = CurrentFocus.Destination;
        }

        public async Task GetTripList()
        {
            Log("Attempting to get trip list.");
            if (_origin is null || _destination is null)
            {
                Log("Could not get trip list because origin and destination is not specified.", "Warning");
                return;
            }

            IsLoading = true;

            TripList tripList = null;
            try
            {
                DateTime dateTime = _givenDate.AddHours(_givenTime.Hours).AddMinutes(_givenTime.Minutes);
                tripList = await TripPlannerProviderProxy.GetTripList(_origin.Id, _destination.Id, dateTime, _isGivenDateTimeForArrivals);
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"An error occurred when attempting to get trip list: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            Trips.Clear();
            _cachedTrips = tripList.Trips.Select(trip => new TripViewModel(trip)).ToList();

            PopulateTripList();
            Log("Get trip list done.");
        }

        public async Task RefreshTripList()
        {
            Log("Attempting to refresh trip list.");
            if (_origin is null || _destination is null)
            {
                Log("Could not refresh trip list because origin and destination is not specified.", "Warning");
                return;
            }

            IsLoading = true;

            TripList tripList = null;
            try
            {
                DateTime dateTime = DateTime.Now;
                tripList = await TripPlannerProviderProxy.GetTripList(_origin.Id, _destination.Id, dateTime, _isGivenDateTimeForArrivals);
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"An error occurred when attempting to refresh trip list: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            Trips.Clear();
            _cachedTrips = tripList.Trips.Select(trip => new TripViewModel(trip)).ToList();

            PopulateTripList();
            Log("Refresh trip list done.");
        }

        public void SwapSearchQueries()
        {
            string swapStr = OriginSearchQuery;
            OriginSearchQuery = DestinationSearchQuery;
            DestinationSearchQuery = swapStr;

            StopLocation swapSl = Origin;
            Origin = Destination;
            Destination = swapSl;
        }

        private async Task<LocationList> GetLocationList(string query) =>
            await TripPlannerProviderProxy.GetLocationList(query);

        private void PopulateTripList(Func<TripViewModel, bool> selector)
        {
            foreach (var trip in _cachedTrips)
            {
                if (selector(trip))
                    Trips.Add(trip);
            }
        }

        private void PopulateTripList() => PopulateTripList(trip => true);

        private enum CurrentFocus { Origin, Destination }
    }
}
