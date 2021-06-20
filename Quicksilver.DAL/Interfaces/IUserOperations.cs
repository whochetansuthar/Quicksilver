using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.DAL.Interfaces
{
    public interface IUserOperations
    {
        public Task DeleteUser(string Id);
        public List<UserDto> GetAllUsers();
        public UserDto GetUserSingle(long Phone);
        public UserDto GetUserSingle(string email);
        public Task<bool> CreateUser(long Phone, string email, string Name);
        public Task<string> GetLoggerUser(string email);
    }
}