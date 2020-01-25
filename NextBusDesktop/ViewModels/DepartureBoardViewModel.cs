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

        private bool _errorOccurred;
        public bool ErrorOccurred
        {
            get => _errorOccurred;
            set => SetProperty(ref _errorOccurred, value, nameof(ErrorOccurred));
        }

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
            EnableLogging = true;
            _translator = new Translator("DeparturesWindow");
            _errorOccurred = false;
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
            LocationList locations = null;

            try
            {
                //locations = await TripPlannerProviderContainer.TripPlannerProvider.GetLocationListAsync(_searchQuery);
                locations = await TripPlannerProviderProxy.GetLocationList(_searchQuery);
                ErrorOccurred = false;
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                System.Diagnostics.Debug.WriteLine($"{nameof(DepartureBoardViewModel)} an error occurred: {e.Message}", "Error");
                return;
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
            if (_selectedStopLocation is null)
                return;

            DateTime today = DateTime.Today;
            DepartureBoard departureBoard = null;

            try
            {
                //departureBoard = await TripPlannerProviderContainer.TripPlannerProvider.GetDepartureBoardAsync(_selectedStopLocation.Id, new DateTime(today.Year, today.Month, today.Day, _departureTime.Hours, _departureTime.Minutes, _departureTime.Seconds));
                departureBoard = await TripPlannerProviderProxy.GetDepartureBoard(_selectedStopLocation.Id, new DateTime(today.Year, today.Month, today.Day, _departureTime.Hours, _departureTime.Minutes, _departureTime.Seconds));
                ErrorOccurred = false;
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                System.Diagnostics.Debug.WriteLine($"{nameof(DepartureBoardViewModel)} an error occurred: {e.Message}", "Error");
                return;
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
            System.Diagnostics.Debug.WriteLine($"Deconstruct -> departures", "Info");
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
            System.Diagnostics.Debug.WriteLine($"{nameof(DepartureBoardViewModel)} attempting to refresh board", "Info");
            if (_selectedStopLocation is null)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(DepartureBoardViewModel)} could not refresh board because current stop is not specified", "Error");
                return;
            }

            DateTime now = DateTime.Now;
            DepartureBoard departureBoard;
            try
            {
                departureBoard = await TripPlannerProviderProxy.GetDepartureBoard(_selectedStopLocation.Id, now);
                ErrorOccurred = false;
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                System.Diagnostics.Debug.WriteLine($"{nameof(DepartureBoardViewModel)} an error occurred: {e.Message}", "Error");
                return;
            }

            if (_cachedDepartures != null)
                DeconstructDepartures();

            _cachedDepartures = departureBoard.Departures.Select(departure => new DepartureViewModel(departure)).ToList();

            Departures.Clear();
            FilterDepartures();
            System.Diagnostics.Debug.WriteLine($"{nameof(DepartureBoardViewModel)} refreshed departure board", "Info");
        }
    }
}
