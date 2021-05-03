using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.QuicksilverDbContext
{
    public partial class TrackingHistory
    {
        public int Id { get; set; }
        public DateTime DateRecorded { get; set; }
        public DateTime? DateDispatched { get; set; }
        public bool CourierInward { get; set; }
        public int ArrivedStationId { get; set; }
        public int? DispatchStationId { get; set; }
        public long TrackingId { get; set; }
    }
}
