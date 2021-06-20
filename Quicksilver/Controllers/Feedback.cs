using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using Quicksilver.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Feedback : Controller
    {
        private readonly FeedbackOperations feedbackOperations = new FeedbackOperations();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllFeedbacks()
        {
            return Ok(feedbackOperations.GetAllFeedbacks());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostRating(FeedbackDto feedbackDto)
        {
            try
            {
                feedbackOperations.PostRating(feedbackDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
