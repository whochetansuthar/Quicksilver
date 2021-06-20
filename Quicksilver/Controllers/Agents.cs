using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Interfaces;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    public class Agents : Controller
    {
        private readonly IAgentsOperations _agentRepository;
        private readonly CommonRepository commonRepository;
        private readonly CourierTypeOperations courierTypeOperations;

        public Agents(IAgentsOperations agentRepository/*, CommonRepository cr*/)
        {
            _agentRepository = agentRepository;
            commonRepository = new CommonRepository();
            courierTypeOperations = new CourierTypeOperations();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var list = commonRepository.GetAllStationsWithCityState();
            return View(list);
        }

        //apis
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllAgents()
        {
            try
            {
                var list = _agentRepository.GetAllAgents();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAgentSingle(string id)
        {
            if (string.IsNullOrEmpty(id)||!ModelState.IsValid)
            {
                return BadRequest("Invalid Details");
            }
            try
            {
                var agent = _agentRepository.GetAgentSingle(id);
                return Ok(agent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAgent(AgentDto agentDto)
        {
            if (string.IsNullOrEmpty(agentDto.FullName)|| string.IsNullOrEmpty(agentDto.Email) || string.IsNullOrEmpty(agentDto.Phone) || agentDto.StationId==0||!ModelState.IsValid)
            {
                return BadRequest("Invalid Details");
            }
            try
            {
               await _agentRepository.Register(agentDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAgent(AgentDto agentDto)
        {
            if (string.IsNullOrEmpty(agentDto.FullName) || string.IsNullOrEmpty(agentDto.Email) || string.IsNullOrEmpty(agentDto.Phone) || agentDto.StationId == 0 || !ModelState.IsValid || string.IsNullOrEmpty(agentDto.Id))
            {
                return BadRequest("Invalid Details");
            }
            try
            {
                await _agentRepository.UpdateAgent(agentDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAgent(string id)
        {
            if (string.IsNullOrEmpty(id)||!ModelState.IsValid)
            {
                return BadRequest("Invalid Details");
            }
            try
            {
                await _agentRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Agent")]
        public IActionResult PlaceOrder(OrderDto orderDto)
        {
            var list = courierTypeOperations.GetAllCourierTypes();
            return View(list);
        }

        [HttpPost]
        public IActionResult GetAllStationWithCityState()
        {
            var list = commonRepository.GetAllStationsWithCityState();
            return Ok(list);
        }
    }
}
