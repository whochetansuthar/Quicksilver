using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class AgentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long MobileNo { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string AspNetUserId { get; set; }
    }
}
