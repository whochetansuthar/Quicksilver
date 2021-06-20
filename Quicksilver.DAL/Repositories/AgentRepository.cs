using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Interfaces;
using Quicksilver.DAL.IdentityDbContext;
using System.Threading.Tasks;

namespace Quicksilver.DAL.Repositories
{
    public class AgentRepository:IAgentRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AgentRepository> logger;

        public AgentRepository(UserManager<ApplicationUser> user, RoleManager<IdentityRole> role, ILogger<AgentRepository> logger)
        {
            userManager = user;
            roleManager = role;
            this.logger = logger;
        }

        public async Task Register(AgentDto agentDto)
        {
            foreach (var role in Constants.Roles)
            {
                var isInRole = await roleManager.RoleExistsAsync(role);
                if (!isInRole)
                {
                    var newRole = new IdentityRole { Name = role };
                    await roleManager.CreateAsync(newRole);
                }
            }

            var user = new ApplicationUser
            {
                UserName = agentDto.Email,
                PhoneNumber = agentDto.Phone,
                StationId = agentDto.StationId,
                Email = agentDto.Email,
                FullName = agentDto.FullName
            };
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;
            var res = await userManager.CreateAsync(user, agentDto.Password);
            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Agent");
            }
            else
            {
                logger.LogError(res.Errors.ToString());
                throw new Exception("Something went wrong while creating User in Net");
            }
        }

        public async Task DeleteAgent(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user,roles);
            await userManager.DeleteAsync(user);
        }

        public List<AgentDto> GetAllAgents()
        {
            var procedure = "spGetAllAgents";
            return db.Query<AgentDto>(procedure,commandType:CommandType.StoredProcedure).ToList();
        }

        public async Task UpdateAgent(AgentDto agentDto)
        {
            var user = await userManager.FindByIdAsync(agentDto.Id);
            user.Email = agentDto.Email;
            user.PhoneNumber = agentDto.Phone;
            user.StationId = agentDto.StationId;
            user.FullName = agentDto.FullName;
            var res = await userManager.UpdateAsync(user);
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.ToString());
            }
        }

        public AgentDto GetAgentSingle(string Id)
        {

            var procedure = "spGetAllAgentSingle";
            var values = new DynamicParameters();
            values.Add("@Id",Id);
            return db.Query<AgentDto>(procedure, values, commandType: CommandType.StoredProcedure).Single();
        }

        public async void ChangePassword(AgentDto agentDto)
        {
            var user = await userManager.FindByIdAsync(agentDto.Id);
            var res = await userManager.ChangePasswordAsync(user,agentDto.currentPassword,agentDto.Password);
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.ToString());
            }
        }
    }
}