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

        public async void Register(AgentDto agentDto)
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


        public async void DeleteAgent(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user,roles);
            await userManager.DeleteAsync(user);
        }
    }
}