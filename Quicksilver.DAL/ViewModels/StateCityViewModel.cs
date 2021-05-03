using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.QuicksilverDbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.ViewModels
{
    public class StateCityViewModel
    {
        public States State { get; set; }
        public List<Cities> City { get; set; }
    }
}
