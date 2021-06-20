using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class ResultEstimate
    {
        public double IntialCost { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public double TaxAmount { get; set; }
        public double DiscountAmt { get; set; }
    }
}
