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
        /// <summary>
        /// Compares current and previous query and returns true if they are equal.
        /// </summary>
        private bool OriginSearchQueryIsDuplicate
        {
            get
            {
                bool isDuplicate = _originSearchQuery != _cachedOriginSearchQuery;
                if (isDuplicate) _cachedOriginSearchQuery = _originSearchQuery; // Update
                return isDuplicate;
            }
        }

        /// <summary>
        /// Compares current and previous query and returns true if they are equal.
        /// </summary>
        private bool DestinationSearchQueryIsDuplicate
        {
            get
            {
                bool isDuplicate = _destinationSearchQuery != _cachedDestinationSearchQuery;
                if (isDuplicate) _cachedDestinationSearchQuery = _destinationSearchQuery; // Update
                return isDuplicate;
            }
        }

        private string _cachedOriginSearchQuery = string.Empty;
        private string _cachedDestinationSearchQuery = string.Empty;

        private bool _errorOccurred;
        public bool ErrorOccurred
        {
            get => _errorOccurred;
            set => SetProperty(ref _errorOccurred, value, nameof(ErrorOccurred));
        }

        private string _dateTimeTextBox;
        public string DateTimeTextBox // TODO: Change to date picker
        {
            get => _dateTimeTextBox;
            set => _dateTimeTextBox = value;
        }

        private DateTime _dateTime
        {
            get
            {
                if (DateTime.TryParse(_dateTimeTextBox, out DateTime parsed))
                    return parsed;
                else
                    return DateTime.Now;
            }

            set => _dateTimeTextBox = value.ToString("yyyy-MM-dd");
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
                else
                {
                    _selectedOrigin = _originStopLocations.ElementAt(value);
                    OriginSearchQuery = _cachedOriginSearchQuery = _selectedOrigin.Name;
                }
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
                else
                {
                    _selectedDestination = _destinationStopLocations.ElementAt(value);
                    DestinationSearchQuery = _cachedDestinationSearchQuery = _selectedDestination.Name;
                }
            }
        }

        private IEnumerable<TripViewModel> _cachedTrips;

        private ObservableCollection<TripViewModel> _trips;
        public ObservableCollection<TripViewModel> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }

        public TripPlannerViewModel()
        {
            _errorOccurred = false;
            _dateTime = DateTime.Now;
            _originStopLocations = new ObservableCollection<StopLocationViewModel>();
            _destinationStopLocations = new ObservableCollection<StopLocationViewModel>();
            _selectedOriginIndex = -1;
            _selectedDestinationIndex = -1;
            _trips = new ObservableCollection<TripViewModel>();
        }

        protected override void Deconstruct()
        {
        }

        public async Task GetOriginLocationList()
        {
            if (!OriginSearchQueryIsDuplicate)
                return;

            OriginStopLocations.Clear();

            var locationList = await GetLocationList(_originSearchQuery);
            
            foreach (var stop in locationList.StopLocations.ToList())
            {
                var stopLocationVm = new StopLocationViewModel(stop);
                OriginStopLocations.Add(stopLocationVm);
            }
        }

        public async Task GetDestinationLocationList()
        {
            if (!DestinationSearchQueryIsDuplicate)
                return;

            DestinationStopLocations.Clear();

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
                tripList = await TripPlannerProviderContainer.TripPlannerProvider.GetTripListAsync(_selectedOrigin.Id, _selectedDestination.Id, _dateTime);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(TripPlannerViewModel)} an error occurred: {e.Message}");
                return;
            }

            Trips.Clear();
            _cachedTrips = tripList.Trips.Select(trip => new TripViewModel(trip)).ToList();

            PopulateTripList();
        }

        public void SwapSearchQueries()
        {
            string swap = OriginSearchQuery;
            OriginSearchQuery = DestinationSearchQuery;
            DestinationSearchQuery = swap;

            StopLocation swapStopLocation = _selectedOrigin;
            SelectedOrigin = SelectedDestination;
            SelectedDestination = swapStopLocation;
        }

        private async Task<LocationList> GetLocationList(string query) =>
            await TripPlannerProviderContainer.TripPlannerProvider.GetLocationListAsync(query);

        private void PopulateTripList(Func<TripViewModel, bool> selector)
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
