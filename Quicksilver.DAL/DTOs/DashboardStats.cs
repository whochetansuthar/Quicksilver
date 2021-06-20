using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class DashboardStats
    {
        public string[] OrderCountByStatus { get; set; }
        public int TotalOrders { get; set; }
        public double TotalRevenue { get; set; }
    }
}
