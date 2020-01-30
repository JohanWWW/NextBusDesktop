using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;
using NextBusDesktop.Models.DepartureBoard;
using NextBusDesktop.DataProvider;
using Windows.UI.Xaml;

namespace NextBusDesktop.ViewModels
{
    public class DepartureBoardViewModel : ViewModelBase
    {
        private Translator _translator;

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        private TimeSpan _departureTime;
        public TimeSpan DepartureTime
        {
            get => _departureTime;
            set => SetProperty(ref _departureTime, value);
        }

        private TrackFilterListViewModel _trackFilter;
        public TrackFilterListViewModel TrackFilter
        {
            get => _trackFilter;
            set => SetProperty(ref _trackFilter, value);
        }

        private ObservableCollection<StopLocationViewModel> _stopLocations;
        public ObservableCollection<StopLocationViewModel> StopLocations
        {
            get => _stopLocations;
            set => SetProperty(ref _stopLocations, value);
        }

        private int _selectedStopLocationIndex;
        public int SelectedStopLocationIndex
        {
            get => _selectedStopLocationIndex;
            set
            {
                SetProperty(ref _selectedStopLocationIndex, value);

                if (value is -1) _selectedStopLocation = null;
                else _selectedStopLocation = _stopLocations.ElementAt(value);
            }
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

        private IEnumerable<DepartureViewModel> _cachedDepartures;

        private ObservableCollection<DepartureViewModel> _departures;
        public ObservableCollection<DepartureViewModel> Departures
        {
            get => _departures;
            set => SetProperty(ref _departures, value);
        }

        private bool _searchResultPaneIsOpen;
        public bool SearchResultPaneIsOpen
        {
            get => _searchResultPaneIsOpen;
            set => SetProperty(ref _searchResultPaneIsOpen, value);
        }

        private bool _selectTrackEnabled;
        public bool SelectTrackEnabled
        {
            get => _selectTrackEnabled;
            set => SetProperty(ref _selectTrackEnabled, value);
        }

        public DepartureBoardViewModel()
        {
            _translator = new Translator("DeparturesWindow");
            _stopLocations = new ObservableCollection<StopLocationViewModel>();
            _departures = new ObservableCollection<DepartureViewModel>();
            _selectedStopLocationIndex = -1;
            _selectedStopLocation = null;
            _searchResultPaneIsOpen = false;
            _selectTrackEnabled = false;
            DateTime now = DateTime.Now;
            DepartureTime = new TimeSpan(now.Hour, now.Minute, now.Second);
            _trackFilter = new TrackFilterListViewModel();
        }

        protected override void Deconstruct()
        {
            if (_cachedDepartures != null)
                DeconstructDepartures();
        }

        public async void GetLocationList()
        {
            IsLoading = true;
            LocationList locations;
            try
            {
                locations = await TripPlannerProviderPassthrough.GetLocationList(_searchQuery);
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"An error occurred: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            if (locations.StopLocations is null)
                return;

            StopLocations.Clear();

            foreach (var stopLocation in locations.StopLocations)
            {
                var viewModel = new StopLocationViewModel(stopLocation);
                StopLocations.Add(viewModel);
            }
            SearchResultPaneIsOpen = true;
            SelectTrackEnabled = false;
        }

        public async Task GetDepartures()
        {
            Log("Attempting to get departures.");
            if (_selectedStopLocation is null)
            {
                Log("Could not get departures because no stop location is specified.", "Warning");
                return;
            }

            IsLoading = true;

            DateTime today = DateTime.Today;
            DepartureBoard departureBoard = null;
            try
            {
                departureBoard = await TripPlannerProviderPassthrough.GetDepartureBoard(_selectedStopLocation.Id, new DateTime(today.Year, today.Month, today.Day, _departureTime.Hours, _departureTime.Minutes, _departureTime.Seconds));
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"An error occurred: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            TrackFilter.Clear();
            SearchQuery = _selectedStopLocation.Name;
            SearchResultPaneIsOpen = false;
            SelectTrackEnabled = true;

            if (_cachedDepartures != null)
                DeconstructDepartures(); // deconstruct existing departures before creating new ones.
            _cachedDepartures = departureBoard.Departures?.Select(d => new DepartureViewModel(d)).ToList();

            TrackFilter.Add(new TrackViewModel(_translator["All"], "*"));
            TrackFilter.SelectedIndex = 0;
            foreach (var track in _cachedDepartures.Select(d => d.Track).Distinct().OrderBy(t => t))
            {
                TrackViewModel viewModel = new TrackViewModel(track);
                TrackFilter.Add(viewModel);
            }

            if (departureBoard.Departures is null)
                return;

            Departures.Clear();

            PopulateDepartureBoard();
            Log("Get departures done.");
        }

        public void FilterDepartures()
        {
            var selectedTrack = _trackFilter.SelectedTrack;
            if (selectedTrack is null)
                return;

            if (selectedTrack.Type is "*")
            {
                Departures.Clear();
                PopulateDepartureBoard();
            }
            else
            {
                Departures.Clear();
                PopulateDepartureBoard(departure => departure.Track == selectedTrack.TrackNumber);
            }
        }

        private void DeconstructDepartures()
        {
            foreach (var departureVm in _cachedDepartures)
                departureVm.OnViewLeave();
            Log("Active resources deactivated", "Deconstruct");
        }

        private void PopulateDepartureBoard(Func<DepartureViewModel, bool> selector)
        {
            if (_cachedDepartures is null)
                return;

            foreach (var departure in _cachedDepartures)
            {
                if (selector(departure))
                    Departures.Add(departure);
            }
        }

        private void PopulateDepartureBoard() => PopulateDepartureBoard(departure => true);

        public async Task RefreshBoard()
        {
            Log("Attempting to refresh board", "Info");
            if (_selectedStopLocation is null)
            {
                Log("Could not refresh board because current stop is not specified", "Warning");
                return;
            }

            IsLoading = true;

            DateTime now = DateTime.Now;
            DepartureBoard departureBoard;
            try
            {
                departureBoard = await TripPlannerProviderPassthrough.GetDepartureBoard(_selectedStopLocation.Id, now);
                HasErrorOccurred = false;
            }
            catch (Exception e)
            {
                HasErrorOccurred = true;
                Log($"An error occurred: {e.Message}", "Error");
                return;
            }
            finally
            {
                IsLoading = false;
            }

            if (_cachedDepartures != null)
                DeconstructDepartures();

            _cachedDepartures = departureBoard.Departures.Select(departure => new DepartureViewModel(departure)).ToList();

            Departures.Clear();
            FilterDepartures();
            Log("Refresh departure board done", "Info");
        }
    }
}
