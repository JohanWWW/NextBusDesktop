using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels.TripPlanner
{
    public class LegResponse
    {
        [DeserializeAs(Name = "name")] public string Name { get; set; }
        [DeserializeAs(Name = "sname")] public string SName { get; set; }
        [DeserializeAs(Name = "journeyNumber")] public string JourneyNumber { get; set; }
        [DeserializeAs(Name = "type")] public string Type { get; set; }
        [DeserializeAs(Name = "id")] public string Id { get; set; }
        [DeserializeAs(Name = "direction")] public string Direction { get; set; }
        [DeserializeAs(Name = "fgColor")] public string ForegroundColor { get; set; }
        [DeserializeAs(Name = "bgColor")] public string BackgroundColor { get; set; }
        [DeserializeAs(Name = "stroke")] public string Stroke { get; set; }
        [DeserializeAs(Name = "accessibility")] public string Accessibility { get; set; }
        [DeserializeAs(Name = "Origin")] public PointResponse Origin { get; set; }
        [DeserializeAs(Name = "Destination")] public PointResponse Destination { get; set; }
    }
}
