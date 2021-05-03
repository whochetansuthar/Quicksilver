using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.QuicksilverDbContext
{
    public partial class Rates
    {
        public int Id { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int CourierId { get; set; }
        public int CourierName { get; set; }
        public double Rate { get; set; }
    }
}
