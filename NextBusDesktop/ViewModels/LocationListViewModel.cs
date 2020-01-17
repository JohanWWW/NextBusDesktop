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
    [Obsolete]
    public class LocationListViewModel : ViewModelBase
    {
        private bool _errorOnGetLocationList;
        public bool ErrorOnGetLocationList
        {
            get => _errorOnGetLocationList;
            set => SetProperty(ref _errorOnGetLocationList, value);
        }

        private ObservableCollection<StopLocationViewModel> _stopLocations;
        public ObservableCollection<StopLocationViewModel> StopLocations
        {
            get => _stopLocations;
            set => SetProperty(ref _stopLocations, value);
        }

        private StopLocationViewModel _currentStopLocation;
        public StopLocationViewModel CurrentStopLocation
        {
            get => _currentStopLocation;
            set
            {
                _currentStopLocation = value;
                _stopLocations.ElementAt(_selectedIndex);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public LocationListViewModel()
        {
            StopLocations = new ObservableCollection<StopLocationViewModel>();
            _isVisible = false;
            _selectedIndex = -1;
        }

        public void Add(StopLocationViewModel stopLocation)
        {
            StopLocations.Add(stopLocation);
        }

        public void Delete(StopLocationViewModel stopLocation)
        {
            if (StopLocations.Contains(stopLocation))
                StopLocations.Remove(stopLocation);
        }

        public void Clear() => StopLocations.Clear();
    }
}
