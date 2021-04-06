using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
     public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long MobileNo { get; set; }
        public string Email { get; set; }
        public string AspNetUserId { get; set; }
    }
}
