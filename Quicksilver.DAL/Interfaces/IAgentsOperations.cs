using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.DAL.Interfaces
{
    public interface IAgentsOperations
    {
        public Task Register(AgentDto agentDto);
        public Task Delete(string Id);
        public List<AgentDto> GetAllAgents();
        public AgentDto GetAgentSingle(string id);
        public Task UpdateAgent(AgentDto agentDto);
    }
}
