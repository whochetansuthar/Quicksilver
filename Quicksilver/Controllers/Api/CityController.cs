using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quicksilver.DAL.DTOs;
using Quicksilver.BAL.Operations;
using Quicksilver.BAL.Operations.Locations;

namespace Quicksilver.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityOperation cityOperation = new CityOperation();

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(cityOperation.GetCities());
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(cityOperation.GetCitySingle(id));
        }

        [HttpPost]
        public IActionResult CreateCity(CityDto cityDto)
        {
            if (cityDto.Name == null || cityDto.StateId == 0 || !ModelState.IsValid)
            {
                return BadRequest("Invalid City details");
            }
            cityDto.Id = cityOperation.CreateCity(cityDto);
            return CreatedAtAction("GetCity/" + cityDto.Id, cityDto);
        }

        [HttpPut]
        public IActionResult UpdateCity(CityDto cityDto)
        {
            if (cityDto.Id == 0 || cityDto.Name == null || cityDto.StateId == 0 || !ModelState.IsValid)
            {
                return BadRequest();
            }
            cityOperation.UpdateCity(cityDto);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteCities(int Id)
        {
            if (Id == 0 || !ModelState.IsValid)
            {
                return BadRequest();
            }
            cityOperation.DeleteCity(Id);
            return Ok();

        }
    }
}
