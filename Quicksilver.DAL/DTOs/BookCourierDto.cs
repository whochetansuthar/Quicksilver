using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class BookCourierDto
    {
        public long Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string sAddress { get; set; }
        public string rAddress { get; set; }
        public string RecipientName { get; set; }
        public int sStationId { get; set; }
        public int rStationId { get; set; }
        public long rMobileNo { get; set; }
        public int sPIN { get; set; }
        public string rCity { get; set; }
        public string sCity { get; set; }
        public string rState { get; set; }
        public string sState { get; set; }
        public int rPIN { get; set; }
        public string CouponCode { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public int CourierType { get; set; }
    }
}