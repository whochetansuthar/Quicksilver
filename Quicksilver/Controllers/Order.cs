using Microsoft.AspNetCore.Mvc;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    public class Order : Controller
    {
        OrderRepository orderRepository = new OrderRepository();
        public IActionResult Index()
        {
            return View();
        }

        //apis
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(orderRepository.GetAllOrders());
        }
    }
}
