﻿using System;

namespace NextBusDesktop.Models
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
