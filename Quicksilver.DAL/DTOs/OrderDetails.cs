using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class OrderDetails
    {
        public long PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SenderAddress { get; set; }
        public string recipientAddress { get; set; }
        public long RecipientMobileNo { get; set; }
        public string RecipientName { get; set; }
        public DateTime OrderDate { get; set; }
        public long TrackingId { get; set; }
        public double Weight { get; set; }
        public string CourierType { get; set; }
        public double InitialCost { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double FinalCost { get; set; }
        public long aPhone { get; set; }
        public string aName { get; set; }
        public string aEmail { get; set; }
        public double Distance { get; set; }
        public DateTime ExpDeliveryDate { get; set; }
        public string Status { get; set; }
        public bool IsRatingGiven { get; set; } = false;
    }
}