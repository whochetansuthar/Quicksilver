using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations.Locations;
using Quicksilver.DAL.QuicksilverDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly QuicksilverContext quicksilverDbContext;

        public LocationController(QuicksilverContext quicksilverContext)
        {
            quicksilverDbContext = quicksilverContext;
        }

        //for City
        public IActionResult Cities()
        {
            var list = quicksilverDbContext.States.ToList();
            return View(list);
        }

        //for state
        public IActionResult States()
        {

            return View();
        }
    }
}
