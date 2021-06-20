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
using Quicksilver.DAL.Helper;

namespace Quicksilver.DAL.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CommonHelpers commonHelpers = new CommonHelpers();

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task DeleteUser(string Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id);
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles);
                await userManager.DeleteAsync(user);
                var procedure = "spDeleteUserData";
                var values = new DynamicParameters();
                values.Add("@Id",Id);
                db.Execute(procedure,values,commandType:CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> CreateUser(long Phone,string email,string Name)
        {
            var isCreated = false;
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = email,
                    FullName = Name,
                    Email = email,
                    PhoneNumber = Phone.ToString(),
                    Password = "User@123"
                };
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                var res = await userManager.CreateAsync(user, user.Password);
                if (res.Succeeded)
                {
                    var r = await userManager.AddToRoleAsync(user, "User");
                    if (!r.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                    }
                    isCreated = true;
                }
                return isCreated;
            }
            catch (Exception)
            {
                return isCreated;
            }
        }

        public List<UserDto> GetAllUsers()
        {
            var procedure = "spGetAllCustomers";
            return db.Query<UserDto>(procedure,commandType:CommandType.StoredProcedure).ToList();
        }

        public UserDto GetUserSingle(long phone)
        {
            var sql = "spGetUserByPhone";
            var values = new DynamicParameters();
            values.Add("@Phone", phone);
            return db.Query<UserDto>(sql,values,commandType:CommandType.StoredProcedure).SingleOrDefault();
        }


        public UserDto GetUserSingle(string email)
        {
            var sql = "spGetUserByEmail";
            var values = new DynamicParameters();
            values.Add("@Email", email);
            return db.Query<UserDto>(sql, values, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public async Task<string> GetLoggerUser(string email)
        {
            var user = await userManager.FindByNameAsync(email);
            return user.Id;
        }
    }
}