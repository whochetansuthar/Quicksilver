using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using Quicksilver.DAL.DTOs;
namespace Quicksilver.DAL.Repositories
{
    public class FeedbackRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<FeedbackDto> GetAllFeedback()
        {
            var procedure = "spGetAllFeedback";
            return db.Query<FeedbackDto>(procedure,commandType:CommandType.StoredProcedure).ToList();
        }

        public void AddFeedback(FeedbackDto feedbackDto)
        {
            var sql = "insert into Feedbacks values(@DateGiven,@TID,@Rating,@Review)";
            var values = new DynamicParameters();
            values.Add("@DateGiven",DateTime.Today);
            values.Add("@TID", feedbackDto.TrackingId);
            values.Add("@Rating", feedbackDto.Rating);
            values.Add("@Review", feedbackDto.Review);
            db.Execute(sql,values);
        }
    }
}
