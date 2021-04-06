using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
   public class OrderDto
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
