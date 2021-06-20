using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.BAL.Operations
{
    public class FeedbackOperations
    {
        private readonly FeedbackRepository feedbackRepository = new FeedbackRepository();

        public List<FeedbackDto> GetAllFeedbacks()
        {
            return feedbackRepository.GetAllFeedback();
        }

        public void PostRating(FeedbackDto feedbackDto)
        {
            feedbackRepository.AddFeedback(feedbackDto);
        }
    }
}
