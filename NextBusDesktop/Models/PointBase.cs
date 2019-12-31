using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Models
{
    /// <summary>
    /// Represents a location point. Provides base functionality and customization for the derived type.
    /// </summary>
    public class PointBase : IPointBase
    {
        string IPointBase.Name { get; set; }
        string IPointBase.Id { get; set; }
        string IPointBase.Track { get; set; }
        int? IPointBase.RouteIndex { get; set; }
        LocationType IPointBase.LocationType { get; set; }
        DateTime IPointBase.ActionDateTime { get; set; }
        DateTime? IPointBase.RealisticActionDateTime { get; set; }

        /// <summary>
        /// Returns the interface instance of the derived type
        /// </summary>
        protected IPointBase IBase => this;

        public PointBase(ResponseModels.TripPlanner.PointResponse derived)
        {
            IBase.Name = derived.Name;
            IBase.Id = derived.Id;
            IBase.Track = derived.Track;
            IBase.ActionDateTime = DateTime.ParseExact(string.Format("{0} {1}", derived.Date, derived.Time), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(derived.RealisticDate) && !string.IsNullOrEmpty(derived.RealisticTime))
                IBase.RealisticActionDateTime = DateTime.ParseExact(string.Format("{0} {1}", derived.RealisticDate, derived.RealisticTime), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            else
                IBase.RealisticActionDateTime = null;

            if (int.TryParse(derived.RouteIndex, out int parsedRouteIndex))
                IBase.RouteIndex = parsedRouteIndex;
            else
                IBase.RouteIndex = null;

            switch (derived.Type)
            {
                case "ST":
                    IBase.LocationType = LocationType.Stop;
                    break;
                case "ADR":
                    IBase.LocationType = LocationType.Address;
                    break;
                case "POI":
                    IBase.LocationType = LocationType.PointOfInterest;
                    break;
                default:
                    IBase.LocationType = LocationType.Unknown;
                    break;
            }
        }

        public PointBase()
        {
        }

        public override string ToString() => IBase.Name;
    }
}
