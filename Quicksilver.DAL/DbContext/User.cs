﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.DbContext
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long MobileNo { get; set; }
        public string Email { get; set; }
        public string AspNetUserId { get; set; }
    }
}