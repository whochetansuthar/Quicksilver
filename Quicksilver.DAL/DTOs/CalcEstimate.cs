using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class CalcEstimate
    {
        public int SenderStationId { get; set; }
        public int RecipientStationId { get; set; }
        public string SenderAddress { get; set; }
        public string RecipientAddress { get; set; }
        public int CourierType { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public string CouponCode { get; set; }
    }
}