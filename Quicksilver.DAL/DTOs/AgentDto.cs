using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class AgentDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? StationId { get; set; }
        public string Email { get; set; }
        public string StationName { get; set; }
        public string currentPassword { get; set; }
        public string Password { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}