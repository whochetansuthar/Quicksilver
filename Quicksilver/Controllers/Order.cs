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
            return Ok(orderRepository.GetAllOrders(User.Identity.Name));
        }

        [HttpGet]
        public IActionResult GetOrderForDashboard()
        {
            var list = orderRepository.GetAllOrders(User.Identity.Name).Where(x=>x.Status!=5);
            return Ok(list);
        }

        [HttpDelete]
        public IActionResult DeleteOrders(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }
            try
            {
                orderRepository.DeleteOrders(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult UpdateStatus(int id,string type)
        {
            if (id==0 || string.IsNullOrEmpty(type))
            {
                return BadRequest("Invalid Inputs");
            }
            try
            {
                var status = orderRepository.UpdateStatus(id, type);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
