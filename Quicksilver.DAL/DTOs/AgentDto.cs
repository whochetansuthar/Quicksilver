using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class AgentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? StationId { get; set; }
        public string Email { get; set; }
        public string StationName { get; set; }
        public string Password { get; set; }
    }
}
