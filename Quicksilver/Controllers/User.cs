using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    [Authorize(Roles = "Admin")]
    public class User : Controller
    {
        private readonly IUserOperations _userOperations;
        public User(IUserOperations userOperations)
        {
            _userOperations = userOperations;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var list = _userOperations.GetAllUsers();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !ModelState.IsValid)
            {
                return BadRequest("Invalid Details");
            }
            try
            {
                await _userOperations.DeleteUser(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetUserByPhone(long phone)
        {
            if (phone.ToString().Length!=10)
            {
                return BadRequest("Invalid Number");
            }
            var user = _userOperations.GetUserSingle(phone);
            if (user==null)
            {
                return Ok("UNF");
            }
            return Ok(user);
        }
    }
}
