using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
