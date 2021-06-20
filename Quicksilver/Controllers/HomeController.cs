using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.IdentityDbContext;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly CourierTypeOperations courierTypeOperations = new CourierTypeOperations();
        private readonly CommonRepository commonRepository = new CommonRepository();
        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Services()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Agent,User")]
        public IActionResult MyOrders()
        {
            var orders = commonRepository.GetOrdersDetailsForUser(User.Identity.Name ?? "");
            return View(orders);
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }
        [Authorize(Roles ="Admin,Agent,User")]
        public IActionResult BookCourier()
        {
            var list = courierTypeOperations.GetAllCourierTypes();
            return View(list);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
