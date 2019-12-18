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
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if(SetProperty(ref _searchQuery, value))
                    GetLocationList();
            }
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
            _stopLocations = new ObservableCollection<StopLocationViewModel>();
            _departures = new ObservableCollection<DepartureViewModel>();
            _selectedStopLocationIndex = -1;
            _selectedStopLocation = null;
            _searchResultPaneIsOpen = false;
        }

        public async void GetLocationList()
        {
            LocationList locations = await TripPlannerProviderContainer.TripPlannerProvider.GetLocationListAsync(_searchQuery);
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
            DepartureBoard departureBoard = await TripPlannerProviderContainer.TripPlannerProvider.GetDepartureBoardAsync(_selectedStopLocation.Id, DateTime.Now);
            if (departureBoard.Departures is null)
                return;

            Departures.Clear();
            foreach (var departure in departureBoard.Departures)
            {
                var viewModel = new DepartureViewModel(departure);
                Departures.Add(viewModel);
            }
        }
    }
}
