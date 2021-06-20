using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Interfaces;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.BAL.Operations
{
    public class AgentOperations:IAgentsOperations
    {
        private readonly IAgentRepository _AgentRepository;
        public AgentOperations(IAgentRepository AgentRepository)
        {
            _AgentRepository = AgentRepository;
        }
        public async Task Register(AgentDto agentDto)
        {
            await _AgentRepository.Register(agentDto);
        }
        public async Task Delete(string Id)
        {
            await _AgentRepository.DeleteAgent(Id);
        }

        public List<AgentDto> GetAllAgents()
        {
            return _AgentRepository.GetAllAgents();
        }

        public AgentDto GetAgentSingle(string id)
        {
            return _AgentRepository.GetAgentSingle(id);
        }

        public async Task UpdateAgent(AgentDto agentDto)
        {
            await _AgentRepository.UpdateAgent(agentDto);
        }
    }
}
