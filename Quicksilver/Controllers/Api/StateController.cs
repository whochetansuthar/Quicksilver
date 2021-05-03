using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations.Locations;
using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly StateOperations stateOperations=new StateOperations();

        [HttpGet]
        public IActionResult GetAllState()
        {
            return Ok(stateOperations.GetState());
        }

        [HttpGet("{Id}")]
        public IActionResult GetState(int? id)
        {
            if (id==null)
            {
                return BadRequest("Invalid Id");
            }
            return Ok(stateOperations.GetStateSingle(id.GetValueOrDefault()));
        }

        [HttpPost]
        public IActionResult CreateSatate(StateDto stateDto)
        {
            if (stateDto.Name==null||!ModelState.IsValid)
            {
                return BadRequest("Invalid State Name");
            }
            stateDto.Id = stateOperations.CreateState(stateDto.Name);
            if (stateDto.Id==-22)
            {
                return BadRequest("State already exists");
            }
            return CreatedAtAction("GetState?Id="+stateDto.Id,stateDto);
        }

        [HttpPut]
        public IActionResult UpdateState(StateDto stateDto)
        {
            try
            {
                if (stateDto.Id == 0 || !ModelState.IsValid)
                {
                    return BadRequest();
                }
                stateOperations.UpdateState(stateDto);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    return BadRequest("State already exist");
                }
                return BadRequest("Something went wrong");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteState(int id)
        {
            if (id==0||!ModelState.IsValid)
            {
                return BadRequest();
            }
            stateOperations.DeleteState(id);
            return Ok();
        }
    }
}