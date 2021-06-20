using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class CourierBookedMessageDto
    {
        public long TrackingId { get; set; }
        public double TotalCost { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string  OrderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }    
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}