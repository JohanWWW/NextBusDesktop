using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models.TripPlanner;

namespace NextBusDesktop.ViewModels
{
    [Obsolete]
    public class TripViewModel : ViewModelBase<Trip>
    {
        public IEnumerable<Step> Steps
        {
            get => Model.Steps;
            set => SetProperty(Model.Steps, value, () => Model.Steps = value);
        }

        public TripViewModel(Trip trip) : base(trip)
        {
        }
    }
}
