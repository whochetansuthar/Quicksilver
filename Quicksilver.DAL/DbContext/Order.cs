using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.DbContext
{
    public partial class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public int StationId { get; set; }
        public int CargoId { get; set; }
        public double InitialCost { get; set; }
        public double Discount { get; set; }
        public double FinalCost { get; set; }
        public int CourierTypeId { get; set; }
        public int? CouponId { get; set; }
    }
}
