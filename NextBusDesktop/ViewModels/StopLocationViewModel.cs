using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.ViewModels
{
    public class StopLocationViewModel : NotificationBase<StopLocation>
    {
        public string Name
        {
            get => This.Name;
            set => SetProperty(This.Name, value, () => This.Name = value);
        }

        public string Id
        {
            get => This.Id;
            set => SetProperty(This.Id, value, () => This.Id = value);
        }

        public int Index
        {
            get => This.Index;
            set => SetProperty(This.Index, value, () => This.Index = value);
        }

        public StopLocationViewModel(StopLocation stopLocation) : base(stopLocation)
        {
        }
    }
}
