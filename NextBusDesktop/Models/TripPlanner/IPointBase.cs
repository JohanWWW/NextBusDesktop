using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Models.TripPlanner
{
    public interface IPointBase
    {
        string Name { get; set; }
        string Id { get; set; }
        string Track { get; set; }
        DateTime ActionDateTime { get; set; }
        DateTime? RealisticActionDateTime { get; set; }
        int? RouteIndex { get; set; }
        LocationType LocationType { get; set; }
    }
}
