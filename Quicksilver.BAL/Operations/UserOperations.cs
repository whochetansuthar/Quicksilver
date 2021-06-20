using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicksilver.BAL.Operations
{
    
    public class UserOperations:IUserOperations
    {
        private readonly IUserRepository _useRepository;
        public UserOperations(IUserRepository useRepository)
        {
            _useRepository = useRepository;
        }

        public List<UserDto> GetAllUsers()
        {
            return _useRepository.GetAllUsers();
        }

        public async Task DeleteUser(string id)
        {
            await _useRepository.DeleteUser(id);
        }

        public UserDto GetUserSingle(long Phone)
        {
            return _useRepository.GetUserSingle(Phone);
        }

        public UserDto GetUserSingle(string email)
        {
            return _useRepository.GetUserSingle(email);
        }

        public async Task<bool> CreateUser(long Phone, string email, string Name)
        {
            return await _useRepository.CreateUser(Phone,email,Name);
        }

        public async Task<string> GetLoggerUser(string email)
        {
            return await _useRepository.GetLoggerUser(email);
        }
    }
}
