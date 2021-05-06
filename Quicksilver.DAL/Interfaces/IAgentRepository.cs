using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.Interfaces
{
    public interface IAgentRepository
    {
        public void Register(AgentDto agentDto);
    }
}
