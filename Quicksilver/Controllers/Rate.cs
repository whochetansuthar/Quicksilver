using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    public class Rate : Controller
    {
        private readonly RateOperations rateOperations = new RateOperations();
        private readonly CourierTypeOperations courierTypeOperations = new CourierTypeOperations();
        public IActionResult Index()
        {
            var list = courierTypeOperations.GetAllCourierTypes();
            return View(list);
        }

        [HttpGet]
        public IActionResult GetAllRates()
        {
            var list = rateOperations.GetAllRates();
            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetRate(int id)
        {
            if (id==0)
            {
                return BadRequest("Invalid Details");
            }
            var list = rateOperations.GetRate(id);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateRate(RateDto rateDto)
        {
            if (rateDto.CourierTypeId == 0 || rateDto.MaxWeight == 0 || rateDto.MinWeight == 0 || rateDto.Rate == 0 || !ModelState.IsValid)
            {
                return BadRequest("Invalid Details");
            }
            rateOperations.CreateRates(rateDto);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateRate(RateDto rateDto)
        {
            if (rateDto.CourierTypeId == 0||rateDto.Id==0||rateDto.MaxWeight==0||rateDto.MinWeight==0||rateDto.Rate==0||!ModelState.IsValid)
            {
                return BadRequest("Invalid Details");
            }
            rateOperations.UpdateRates(rateDto);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteRate(int id)
        {
            if (id==0)
            {
                return BadRequest("Invalid Data");
            }
            rateOperations.DeleteRates(id);
            return Ok();
        }
    }
}
