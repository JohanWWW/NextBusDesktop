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
        private Translator _translator;
        private DispatcherTimer _timer;

        private bool _errorOccurred;
        public bool ErrorOccurred
        {
            get => _errorOccurred;
            set => SetProperty(ref _errorOccurred, value, nameof(ErrorOccurred));
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

        private StopLocation _selectedOrigin;
        public StopLocation SelectedOrigin
        {
            get => _selectedOrigin;
            set
            {
                if (_originStopLocations != null)
                    SetProperty(ref _selectedOrigin, value);
            }
        }

        private int _selectedOriginIndex;
        public int SelectedOriginIndex
        {
            get => _selectedOriginIndex;
            set
            {
                SetProperty(ref _selectedOriginIndex, value);

                if (value is -1) _selectedOrigin = null;
                else _selectedOrigin = _originStopLocations.ElementAt(value);
            }
        }

        private ObservableCollection<StopLocationViewModel> _destinationStopLocations;
        public ObservableCollection<StopLocationViewModel> DestinationStopLocations
        {
            get => _destinationStopLocations;
            set => SetProperty(ref _destinationStopLocations, value);
        }

        private StopLocation _selectedDestination;
        public StopLocation SelectedDestination
        {
            get => _selectedDestination;
            set
            {
                if (_destinationStopLocations != null)
                    SetProperty(ref _selectedDestination, value);
            }
        }

        private int _selectedDestinationIndex;
        public int SelectedDestinationIndex
        {
            get => _selectedDestinationIndex;
            set
            {
                SetProperty(ref _selectedDestinationIndex, value);

                if (value is -1) _selectedDestination = null;
                else _selectedDestination = _destinationStopLocations.ElementAt(value);
            }
        }

        private IEnumerable<Trip> _cachedTrips;

        private ObservableCollection<Trip> _trips;
        public ObservableCollection<Trip> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }

        public TripPlannerViewModel()
        {
            //_translator = new Translator(...);
            _errorOccurred = false;
            _originStopLocations = new ObservableCollection<StopLocationViewModel>();
            _destinationStopLocations = new ObservableCollection<StopLocationViewModel>();
            _selectedOriginIndex = -1;
            _selectedDestinationIndex = -1;
            _trips = new ObservableCollection<Trip>();
        }

        protected override void Deconstruct()
        {
        }

        public async Task GetOriginLocationList()
        {
            var locationList = await GetLocationList(_originSearchQuery);
            
            foreach (var stop in locationList.StopLocations.ToList())
            {
                var stopLocationVm = new StopLocationViewModel(stop);
                OriginStopLocations.Add(stopLocationVm);
            }
        }

        public async Task GetDestinationLocationList()
        {
            var locationList = await GetLocationList(_destinationSearchQuery);
            foreach (var stop in locationList.StopLocations.ToList())
            {
                var stopLocationVm = new StopLocationViewModel(stop);
                DestinationStopLocations.Add(stopLocationVm);
            }
        }

        public async Task GetTripList()
        {
            if (_selectedOrigin is null || _selectedDestination is null)
                return;

            TripList tripList = null;
            try
            {
                tripList = await TripPlannerProviderContainer.TripPlannerProvider.GetTripListAsync(_selectedOrigin.Id, _selectedDestination.Id);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(TripPlannerViewModel)} an error occurred: {e.Message}");
                return;
            }

            _cachedTrips = tripList.Trips.ToList();

            PopulateTripList();
        }

        private async Task<LocationList> GetLocationList(string query) =>
            await TripPlannerProviderContainer.TripPlannerProvider.GetLocationListAsync(query);

        private void PopulateTripList(Func<Trip, bool> selector)
        {
            foreach (var trip in _cachedTrips)
            {
                if (selector(trip))
                    Trips.Add(trip);
            }
        }

        private void PopulateTripList() => PopulateTripList(trip => true);
    }
}
