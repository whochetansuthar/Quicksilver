using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
     public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long MobileNo { get; set; }
        public string Email { get; set; }
        public int TotalOrders { get; set; }
        public DateTime LastOrderDate { get; set; }
        public double AvgGivenRating { get; set; }
        public string AspNetUserId { get; set; }
    }
}
