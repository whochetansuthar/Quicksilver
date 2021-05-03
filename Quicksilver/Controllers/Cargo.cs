using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    public class Cargo : Controller
    {
        private readonly CargoOperations cargoOperations = new CargoOperations();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllCargoes()
        {
            return Ok(cargoOperations.GetAllCargoes());
        }

        [HttpGet]
        public IActionResult GetCargo(int id)
        {
            if (id==0)
            {
                return BadRequest("Invalid Data");
            }
            return Ok(cargoOperations.GetCargo(id));
        }

        [HttpPost]
        public IActionResult CreateCargo(CargoDto cargoDto)
        {
            if (cargoDto.CargoCompanyName == null || cargoDto.CargoCompanyMobileNo == 0)
            {
                return BadRequest("Invalid Data");
            }
            cargoOperations.CreateCargoes(cargoDto);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateCargoes(CargoDto cargoDto)
        {
            if (cargoDto.Id==0||cargoDto.CargoCompanyName ==null||cargoDto.CargoCompanyMobileNo==0)
            {
                return BadRequest("Invalid Data");
            }
            cargoOperations.UpdateCargoes(cargoDto);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteCargoes(int id)
        {
            if (id==0)
            {
                return BadRequest("Invalid Data");
            }
            cargoOperations.DeleteCargoes(id);
            return Ok();
        }
    }
}
