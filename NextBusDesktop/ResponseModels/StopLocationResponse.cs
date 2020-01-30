using System;
using RestSharp.Deserializers;

namespace NextBusDesktop.ResponseModels
{
    public class StopLocationResponse
    {
        [DeserializeAs(Name="name")] public string Name { get; set; }
        [DeserializeAs(Name="id")] public string Id { get; set; }
        [DeserializeAs(Name="idx")] public int Index { get; set; }
    }
}
