using System;
using NextBusDesktop.ResponseModels.TripPlanner;

namespace NextBusDesktop.Models.TripPlanner
{
    public class Origin : PointBase
    {
        public string StopName
        {
            get => IBase.Name;
            set => IBase.Name = value;
        }

        public string StopId
        {
            get => IBase.Id;
            set => IBase.Id = value;
        }

        public string Track
        {
            get => IBase.Track;
            set => IBase.Track = value;
        }

        public int? RouteIndex
        {
            get => IBase.RouteIndex;
            set => IBase.RouteIndex = value;
        }

        public LocationType Type
        {
            get => IBase.LocationType;
            set => IBase.LocationType = value;
        }

        public DateTime DepartureDateTime
        {
            get => IBase.ActionDateTime;
            set => IBase.ActionDateTime = value;
        }

        public DateTime? RealisticDepartureDateTime
        {
            get => IBase.RealisticActionDateTime;
            set => IBase.RealisticActionDateTime = value;
        }

        public Origin(PointResponse originResponseModel) : base(originResponseModel)
        {
        }

        public Origin() : base()
        {
        }
    }
}
