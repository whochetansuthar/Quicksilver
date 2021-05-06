using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.IdentityDbContext
{
    public class ApplicationUser:IdentityUser
    {
        public int? StationId { get; set; }
        public string FullName { get; set; }
    }
}