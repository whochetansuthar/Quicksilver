using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.Controllers
{
    public class CourierType : Controller
    {

        private readonly CourierTypeOperations courierTypeOperations = new CourierTypeOperations();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllCouriers()
        {
            return Ok(courierTypeOperations.GetAllCourierTypes());
        }

        [HttpGet]
        public IActionResult GetCourier(int Id)
        {
            if (Id==0)
            {
                return BadRequest("Invalid Details");
            }
            return Ok(courierTypeOperations.GetCourierType(Id));
        }

        [HttpPost]
        public IActionResult CreateCourier(CourierTypeDto courierTypeDto)
        {
            if (courierTypeDto.Name == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid details");
            }
            courierTypeDto.Id = courierTypeOperations.CreateCourierType(courierTypeDto);
            return CreatedAtAction("GetCourier/" + courierTypeDto.Id, courierTypeDto);
        }

        [HttpPut]
        public IActionResult UpdateCourier(CourierTypeDto courierTypeDto)
        {
            if (courierTypeDto.Id==0 || courierTypeDto.Name == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid details");
            }
            courierTypeOperations.UpdateCourierType(courierTypeDto);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteCourierType(int Id)
        {
            if (Id==0||!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }
            courierTypeOperations.DeleteCourierType(Id);
            return Ok();
        }
    }
}
