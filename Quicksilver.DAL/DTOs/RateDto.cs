using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class RateDto
    {
        public int Id { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int CourierTypeId { get; set; }
        public string CourierTypeName { get; set; }
        public double Rate { get; set; }
    }
}
