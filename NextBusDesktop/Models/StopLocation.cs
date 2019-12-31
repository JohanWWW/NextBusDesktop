using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.ResponseModels;

namespace NextBusDesktop.Models
{
    public class StopLocation : PointBase
    {
        public string Name
        {
            get => IBase.Name;
            set => IBase.Name = value;
        }

        public string Id
        {
            get => IBase.Id;
            set => IBase.Id = value;
        }

        public int Index { get; set; }

        public StopLocation(StopLocationResponse stopLocationResponseModel)
        {
            IBase.Name = stopLocationResponseModel.Name;
            IBase.Id = stopLocationResponseModel.Id;
            Index = stopLocationResponseModel.Index;
        }

        public StopLocation()
        {
        }

        public override string ToString() => Name;
    }
}
