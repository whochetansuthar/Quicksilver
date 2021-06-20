using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.DAL.Interfaces
{
    public interface IUserRepository
    {
        public List<UserDto> GetAllUsers();
        public UserDto GetUserSingle(long phone);
        public UserDto GetUserSingle(string email);
        public Task DeleteUser(string Id);
        public Task<bool> CreateUser(long Phone, string email, string Name);
        public Task<string> GetLoggerUser(string email);
    }
}
