using Microsoft.AspNetCore.Mvc;
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
        private readonly IAgentRepository agentRepository;

        public Agents(IAgentRepository agentRepository)
        {
            this.agentRepository = agentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        //apis
        [HttpGet]
        public IActionResult GetAllAgents(AgentDto agentDto)
        {
            return Ok();
        }
    }
}
