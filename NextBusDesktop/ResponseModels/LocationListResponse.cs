﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class LocationListResponse
    {
        [DeserializeAs(Name = "serverTime")] public string ServerTime { get; set; }
        [DeserializeAs(Name = "serverDate")] public string ServerDate { get; set; }
        [DeserializeAs(Name = "StopLocation")] public IEnumerable<StopLocationResponse> StopLocations { get; set; }
    }
}