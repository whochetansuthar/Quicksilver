using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.QuicksilverDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quicksilver.DAL.ViewModels;

namespace Quicksilver.Controllers
{
    public class Station : Controller
    {
        StationOperations stationOperations = new StationOperations();
        QuicksilverContext QuicksilverContext;

        public Station(QuicksilverContext context)
        {
            QuicksilverContext = context;
        }
        public IActionResult Index()
        {
            var states = QuicksilverContext.States.ToList();
            List<StateCityViewModel> list = new List<StateCityViewModel>();
            foreach (var state in states)
            {
                StateCityViewModel model = new StateCityViewModel();
                model.State = state;
                model.City = QuicksilverContext.Cities.Where(x => x.StateId == state.Id).ToList();
                list.Add(model);
            }
            return View(list);
        }


        //apis
        [HttpGet]
        public IActionResult GetStations()
        {
            return Ok(stationOperations.GetAllStations());
        }

        [HttpGet]
        public IActionResult GetStation(int id)
        {
            if (id==0)
            {
                return NotFound("Invalid Id");
            }
            return Ok(stationOperations.GetStation(id));
        }

        [HttpPost]
        public IActionResult CreateStation(StationDto stationDto)
        {
            if (stationDto.Name==null||!ModelState.IsValid||stationDto.CityId==0)
            {
                return BadRequest("Invalid Details");
            }

            stationDto.Id = stationOperations.CreateStation(stationDto);
            return CreatedAtAction(nameof(GetStation)+"/"+stationDto.Id,stationDto);
        }

        [HttpPut]
        public IActionResult UpdateStation(StationDto stationDto)
        {
            if (stationDto.Name == null || !ModelState.IsValid || stationDto.CityId == 0||stationDto.Id==0)
            {
                return BadRequest("Invalid Details");
            }
            stationOperations.UpdateStations(stationDto);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteStation(int id)
        {
            if (!ModelState.IsValid||id==0)
            {
                return BadRequest("Invalid Details");
            }
            stationOperations.DeleteStation(id);
            return Ok();
        }
    }
}