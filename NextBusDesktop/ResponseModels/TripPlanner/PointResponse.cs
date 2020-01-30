using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.TripPlanner
{
    /// <summary>
    /// Represents a place of interest such as origin or destination
    /// </summary>
    public class PointResponse
    {
        [DeserializeAs(Name = "name")] public string Name { get; set; }
        [DeserializeAs(Name = "type")] public string Type { get; set; }
        [DeserializeAs(Name = "id")] public string Id { get; set; }
        [DeserializeAs(Name = "routeIdx")] public string RouteIndex { get; set; }
        [DeserializeAs(Name = "time")] public string Time { get; set; }
        [DeserializeAs(Name = "rtTime")] public string RealisticTime { get; set; }
        [DeserializeAs(Name = "date")] public string Date { get; set; }
        [DeserializeAs(Name = "rtDate")] public string RealisticDate { get; set; }
        [DeserializeAs(Name = "track")] public string Track { get; set; }
    }
}
