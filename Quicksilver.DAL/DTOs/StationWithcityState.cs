using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class StationWithcityState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
    }
}