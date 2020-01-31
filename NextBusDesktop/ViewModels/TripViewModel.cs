using System;
using System.Collections.ObjectModel;
using System.Linq;
using NextBusDesktop.Models.TripPlanner;
using NextBusDesktop.Utilities;

namespace NextBusDesktop.ViewModels
{
    public class TripViewModel : ViewModelBase
    {
        private readonly Translator _translator;

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

        private bool IsRescheduled => RealisticDeparture != null && RealisticArrival != null;

        private string _timeLeftInfo;
        public string TimeLeftInfo
        {
            get => _timeLeftInfo;
            private set => SetProperty(ref _timeLeftInfo, value);
        }

        private string _tripDurationInfo;
        public string TripDurationInfo
        {
            get => _tripDurationInfo;
            private set => SetProperty(ref _tripDurationInfo, value);
        }

        public string DepartureTimeInfo => (RealisticDeparture ?? ScheduledDeparture).ToString("HH:mm");
        public string ArrivalTimeInfo => (RealisticArrival ?? ScheduledArrival).ToString("HH:mm");

        public TripViewModel(Trip trip)
        {
            _translator = new Translator("TripPlannerResources");
            _steps = new ObservableCollection<StepViewModel>();
            foreach (var step in trip.Steps)
            {
                _steps.Add(new StepViewModel(step) { IsLastStep = trip.Steps.Last().Direction == step.Direction });
            }
            TimeLeftInfo = GetActionTimeLeft();
            TripDurationInfo = GetTripDuration();
        }

        public string GetActionTimeLeft()
        {
            TimeSpan timeLeft = (RealisticDeparture ?? ScheduledDeparture) - DateTime.Now;
            return TimeDurationFormatter(timeLeft);
        }

        public string GetTripDuration() => TimeDurationFormatter(RealisticDuration);

        public void TriggerTimeUpdate() => TimeLeftInfo = GetActionTimeLeft();

        private string TimeDurationFormatter(TimeSpan duration)
        {
            if (duration.TotalMinutes < 1)
                return _translator["Now"];

            else if (duration.TotalMinutes < 60)
                return string.Format("{0} {1}", duration.ToString("mm"), _translator["MinutesAbbr"]).TrimStart('0');

            else if (duration.TotalHours < 24)
                return string.Format("{0}{1} {2}{3}", duration.ToString("hh"), _translator["HoursAbbr"], duration.ToString("mm"), _translator["MinutesAbbr"]).TrimStart('0');

            else
                return string.Format("{0} {1}", duration.ToString("dd"), _translator["DaysAbbr"]).TrimStart('0');
        }

        public override string ToString() => $"{Origin.StopName} -> {Destination.StopName}";
    }
}
