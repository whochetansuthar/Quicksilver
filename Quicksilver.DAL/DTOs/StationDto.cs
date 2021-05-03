using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class StationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}
