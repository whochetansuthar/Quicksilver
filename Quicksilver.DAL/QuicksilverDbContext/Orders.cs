using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.QuicksilverDbContext
{
    public partial class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public int StationId { get; set; }
        public int CargoId { get; set; }
        public long TrackingId { get; set; }
        public int TransactionId { get; set; }
        public int CourierTypeId { get; set; }
    }
}
