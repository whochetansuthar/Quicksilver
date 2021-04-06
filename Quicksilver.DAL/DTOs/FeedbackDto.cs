using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public DateTime DateGiven { get; set; }
        public int OrderId { get; set; }
        public double Rating { get; set; }
        public string Review { get; set; }
    }
}
