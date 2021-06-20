using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class TrackCourierDto
    {
        public long TrackingId { get; set; }
        public int Status { get; set; }
        public string recipientAddress { get; set; }
        public double Distance { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExDeliveryDate { get; set; }
    }
}
