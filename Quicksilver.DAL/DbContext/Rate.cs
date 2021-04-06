using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.DbContext
{
    public partial class Rate
    {
        public int Id { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int CourierId { get; set; }
        public double Rate1 { get; set; }
    }
}
