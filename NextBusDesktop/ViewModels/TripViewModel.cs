using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models.TripPlanner;
using Windows.UI.Xaml;

namespace NextBusDesktop.ViewModels
{
    public class TripViewModel : ViewModelBase
    {
        private Translator _translator;
        private DispatcherTimer _timer;

        public TripViewModel This => this;

        private ObservableCollection<StepViewModel> _steps;
        public ObservableCollection<StepViewModel> Steps
        {
            get => _steps;
            set => SetProperty(ref _steps, value);
        }

        public Origin Origin => _steps.First().Origin;
        public Destination Destination => _steps.Last().Destination;

        private DateTime ScheduledDeparture => Origin.DepartureDateTime;
        private DateTime? RealisticDeparture => Origin.RealisticDepartureDateTime;
        private DateTime ScheduledArrival => Destination.ArrivalDateTime;
        private DateTime? RealisticArrival => Destination.RealisticArrivalDateTime;

        private TimeSpan ScheduledDuration => ScheduledArrival - ScheduledDeparture;
        private TimeSpan RealisticDuration => (RealisticArrival ?? ScheduledArrival) - (RealisticDeparture ?? ScheduledDeparture);

        public TripViewModel(Trip trip)
        {
            _steps = new ObservableCollection<StepViewModel>();

            foreach (var step in trip.Steps)
            {
                _steps.Add(new StepViewModel(step) { IsLastStep = trip.Steps.Last().Direction == step.Direction });
            }
        }

        public override string ToString() => $"{Origin.StopName} -> {Destination.StopName}";
    }
}
