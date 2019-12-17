using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class Departure
    {
        [DeserializeAs(Name = "name")] public string Name { get; set; }
        [DeserializeAs(Name = "sname")] public string SName { get; set; }
        [DeserializeAs(Name = "journeyNumber")] public string JourneyNumber { get; set; }
        [DeserializeAs(Name = "type")] public string Type { get; set; }
        [DeserializeAs(Name = "stopid")] public string StopId { get; set; }
        [DeserializeAs(Name = "stop")] public string Stop { get; set; }
        [DeserializeAs(Name = "time")] public string ScheduledTime { get; set; }
        [DeserializeAs(Name = "rtTime")] public string RealisticTime { get; set; }
        [DeserializeAs(Name = "date")] public string ScheduledDate { get; set; }
        [DeserializeAs(Name = "rtDate")] public string RealisticDate { get; set; }
        [DeserializeAs(Name = "journeyid")] public string JourneyId { get; set; }
        [DeserializeAs(Name = "direction")] public string Direction { get; set; }
        [DeserializeAs(Name = "track")] public string Track { get; set; }
        [DeserializeAs(Name = "fgColor")] public string ForegroundColor { get; set; }
        [DeserializeAs(Name = "bgColor")] public string BackgroundColor { get; set; }
        [DeserializeAs(Name = "stroke")] public string Stroke { get; set; }
        [DeserializeAs(Name = "accessibility")] public string Accessibility { get; set; }
    }
}
