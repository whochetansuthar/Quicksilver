using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public DateTime DateGiven { get; set; }
        public long TrackingId { get; set; }
        public double Rating { get; set; }
        public string Review { get; set; }
        public string UserName { get; set; }
        public long UserMobileNo { get; set; }
    }
}
