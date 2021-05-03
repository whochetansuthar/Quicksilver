using Microsoft.AspNetCore.Mvc;
using Quicksilver.BAL.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quicksilver.Controllers
{
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
    }
}
