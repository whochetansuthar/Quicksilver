using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.DAL.Interfaces
{
    public interface IAgentRepository
    {
        public Task Register(AgentDto agentDto);
        public Task DeleteAgent(string Id);
        public AgentDto GetAgentSingle(string Id);

        public List<AgentDto> GetAllAgents();
        public Task UpdateAgent(AgentDto agentDto);
    }
}