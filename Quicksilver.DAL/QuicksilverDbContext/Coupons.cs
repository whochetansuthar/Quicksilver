using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.QuicksilverDbContext
{
    public partial class Coupons
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Discount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpired { get; set; }
    }
}
