using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.DTOs
{
    public class CargoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CargoCompany { get; set; }
        public long CargoCompanyMobileNo { get; set; }
    }
}
