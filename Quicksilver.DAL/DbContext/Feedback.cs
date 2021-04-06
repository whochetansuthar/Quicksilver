using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.DbContext
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public DateTime DateGiven { get; set; }
        public int OrderId { get; set; }
        public double Rating { get; set; }
        public string Review { get; set; }
    }
}
