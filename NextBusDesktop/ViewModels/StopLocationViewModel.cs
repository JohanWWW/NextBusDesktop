using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.ViewModels
{
    public class StopLocationViewModel : ViewModelBase<StopLocation>
    {
        public string Name
        {
            get => Model.Name;
            set => SetProperty(Model.Name, value, () => Model.Name = value);
        }

        public string Id
        {
            get => Model.Id;
            set => SetProperty(Model.Id, value, () => Model.Id = value);
        }

        public int Index
        {
            get => Model.Index;
            set => SetProperty(Model.Index, value, () => Model.Index = value);
        }

        public StopLocationViewModel(StopLocation stopLocation) : base(stopLocation)
        {
        }
    }
}
