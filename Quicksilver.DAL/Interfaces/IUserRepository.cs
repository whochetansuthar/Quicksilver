using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.Interfaces
{
    public interface IUserRepository
    {
        public List<UserDto> GetAllUsers();
        public UserDto GetUserSingle(string Id);
        public void DeleteUser(string Id);
    }
}
