using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    [Authorize(Roles = "Admin,Agent")]
    public class BackendController : Controller
    {

        //this is test commit
        private readonly CommonRepository commonRepository = new CommonRepository();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetDashboardStats()
        {
            return Ok(commonRepository.spGetDashboardStats());
        }
    }
}
