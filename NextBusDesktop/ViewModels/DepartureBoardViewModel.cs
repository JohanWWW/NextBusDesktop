using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;
using NextBusDesktop.DataProvider;

namespace NextBusDesktop.ViewModels
{
    public class DepartureBoardViewModel : NotificationBase
    {
        private Translator _translator;

        private bool _errorOnGetLocationList;
        public bool ErrorOnGetLocationList
        {
            get => _errorOnGetLocationList;
            set => SetProperty(ref _errorOnGetLocationList, value);
        }

        private bool _errorOnGetDepartureBoard;
        public bool ErrorOnGetDepartureBoard
        {
            get => _errorOnGetDepartureBoard;
            set => SetProperty(ref _errorOnGetDepartureBoard, value);
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

        public DepartureBoardViewModel()
        {
            _translator = new Translator("DeparturesWindow");
            _stopLocations = new ObservableCollection<StopLocationViewModel>();
            _departures = new ObservableCollection<DepartureViewModel>();
            _selectedStopLocationIndex = -1;
            _selectedStopLocation = null;
            _searchResultPaneIsOpen = false;
            DateTime now = DateTime.Now;
            DepartureTime = new TimeSpan(now.Hour, now.Minute, now.Second);
            _trackFilter = new TrackFilterListViewModel();
        }

        public async void GetLocationList()
        {
            LocationList locations = await TripPlannerProviderContainer.TripPlannerProvider.GetLocationListAsync(_searchQuery);
            ErrorOnGetLocationList = locations?.ErrorMessage != null;
            if (locations.StopLocations is null)
                return;

            StopLocations.Clear();

            foreach (var stopLocation in locations.StopLocations)
            {
                var viewModel = new StopLocationViewModel(stopLocation);
                StopLocations.Add(viewModel);
            }
            SearchResultPaneIsOpen = true;
        }

        public async void GetDepartures()
        {
            TrackFilter.Clear();
            if (_selectedStopLocation is null)
                return;

            SearchQuery = _selectedStopLocation.Name;
            SearchResultPaneIsOpen = false;

            DateTime today = DateTime.Today;
            DepartureBoard departureBoard = await TripPlannerProviderContainer.TripPlannerProvider.GetDepartureBoardAsync(_selectedStopLocation.Id, new DateTime(today.Year, today.Month, today.Day, _departureTime.Hours, _departureTime.Minutes, _departureTime.Seconds));
            _cachedDepartures = departureBoard.Departures?.Select(d => new DepartureViewModel(d));

            TrackFilter.Add(new TrackViewModel(_translator["All"], "*"));
            foreach (var track in _cachedDepartures.Select(d => d.Track).Distinct().OrderBy(t => t))
            {
                TrackViewModel viewModel = new TrackViewModel(track);
                TrackFilter.Add(viewModel);
            }

            ErrorOnGetDepartureBoard = departureBoard?.ErrorMessage != null;

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

        private void PopulateDepartureBoard(Func<DepartureViewModel, bool> where)
        {
            foreach (var departure in _cachedDepartures)
            {
                if (where(departure))
                    Departures.Add(departure);
            }
        }

        private void PopulateDepartureBoard() => PopulateDepartureBoard(departure => true);
    }
}
