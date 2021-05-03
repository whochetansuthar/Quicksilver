using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Discount { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateExpired { get; set; }
    }
}
