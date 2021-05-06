using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using Quicksilver.DAL.DTOs;
using Microsoft.AspNetCore.Identity;
using Quicksilver.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quicksilver.DAL.Interfaces;
using Quicksilver.DAL.IdentityDbContext;

namespace Quicksilver.DAL.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserRepository> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;

        }
        void IUserRepository.DeleteUser(string Id)
        {
            throw new NotImplementedException();
        }

        List<UserDto> IUserRepository.GetAllUsers()
        {
            throw new NotImplementedException();
        }

        UserDto IUserRepository.GetUserSingle(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
