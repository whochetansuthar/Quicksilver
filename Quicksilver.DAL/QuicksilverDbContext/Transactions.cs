using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.QuicksilverDbContext
{
    public partial class Transactions
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public double InitialCost { get; set; }
        public double Discount { get; set; }
        public double Cgst { get; set; }
        public double Sgst { get; set; }
        public double FinalCost { get; set; }
        public int CouponId { get; set; }
        public int OrderId { get; set; }
    }
}
