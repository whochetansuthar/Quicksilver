using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class GetAllOrders
    {
        public string aPhone { get; set; }
        public string aName { get; set; }
        public string aStation { get; set; }
        public string uMobileNo { get; set; }
        public string uName { get; set; }
        public string RecpientStation { get; set; }
        public string SenderStation { get; set; }
        public string CourierType { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public DateTime OrderDate { get; set; }
        public string recipientAddress { get; set; }
        public int recipientStationId { get; set; }
        public string SenderAddress { get; set; }
        public int SenderStationId { get; set; }
        public int CargoId { get; set; }
        public long TrackingId { get; set; }
        public int TransactionId { get; set; }
        public int CourierTypeId { get; set; }
        public int Status { get; set; }
        public long RecipientMobileNo { get; set; }
        public double Weight { get; set; }
        public string RecipientName { get; set; }
        public double Distance { get; set; }
        public DateTime DateCreated { get; set; }
        public double InitialCost { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double FinalCost { get; set; }
        public int CouponId { get; set; }
    }

}