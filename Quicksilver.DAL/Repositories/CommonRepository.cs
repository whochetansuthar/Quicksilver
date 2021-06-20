using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using Quicksilver.DAL.DTOs;
using Razorpay.Api;
using Microsoft.Extensions.Configuration;

namespace Quicksilver.DAL.Repositories
{
    public class CommonRepository
    {
        private readonly IDbConnection db;
        public CommonRepository()
        {
            db = new SqlConnection(DbConnectionString.ConStr);
        }

        public List<OrderDetails> GetOrdersDetailsForUser(string email)
        {
            var procedure = "spGetAllOrdersForUser";
            var values = new DynamicParameters();
            values.Add("@Email",email);
            return db.Query<OrderDetails>(procedure,values,commandType:CommandType.StoredProcedure).ToList();
        }
        public TrackCourierDto GetTrackCourier(long tid)
        {
            var sql = "select OrderDate,TrackingId, recipientAddress, Status from Orders where TrackingId=@tid";
            var values = new DynamicParameters();
            values.Add("@tid",tid);
            return db.Query<TrackCourierDto>(sql,values).SingleOrDefault();
        }
        public List<StationWithcityState> GetAllStationsWithCityState()
        {
            var sql = "select Stations.Id,Stations.Name,Cities.Name as City,Cities.Id as CityId,States.Name as State,States.Id as StateId from Stations inner join Cities on Cities.Id=Stations.CityId inner join States on States.Id=Cities.StateId";
            return db.Query<StationWithcityState>(sql).ToList();
        }

        public CouponDto CanApplyCoupon(string code)
        {
            var procedure = "spCheckCoupon";
            var values = new DynamicParameters();
            values.Add("@code",code);
            return db.Query<CouponDto>(procedure,values,commandType:CommandType.StoredProcedure).SingleOrDefault();
        }

        public RateDto GetRateByCourierTypeAndWeight(double weight,int courierTypeId)
        {
            var procedure = "spGetRateByCourierTypeAndWeight";
            var values = new DynamicParameters();
            values.Add("@weight",weight);
            values.Add("@courierTypeId", courierTypeId);
            return db.Query<RateDto>(procedure, values, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public long BookCourier(int? cid,RateDto Rate,BookCourierDto bookCourierDto,string UserId,ResultEstimate Estimate,int status,int distance, string AgentId = null,bool IsByUser=false)
        {
            try
            {
                var trackingId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                var procedure = "spBookCourier";
                var values = new DynamicParameters();
                values.Add("@UserId", UserId);
                values.Add("@AgentId", AgentId);
                values.Add("@recipientAddress", bookCourierDto.rAddress);
                values.Add("@recipientStationId", bookCourierDto.rStationId);
                values.Add("@senderAddress", bookCourierDto.sAddress);
                values.Add("@senderStationId", bookCourierDto.sStationId);
                values.Add("@CargoId", 0);
                values.Add("@CourierTypeId", Rate.CourierTypeId);
                values.Add("@Status", status);
                values.Add("@rMobileNo", bookCourierDto.rMobileNo);
                values.Add("@RecipientName", bookCourierDto.RecipientName);
                values.Add("@DateNow", DateTime.Now);
                values.Add("@TrackingId", trackingId);
                values.Add("@Intialcost", Estimate.IntialCost);
                values.Add("@Discount", Estimate.Discount);
                values.Add("@Tax", Estimate.Tax);
                values.Add("@CouponId", cid);
                values.Add("@Weight", bookCourierDto.Weight);
                values.Add("@FinalCost", Estimate.GrandTotal);
                double dst = distance / 1000;
                values.Add("@Distance", dst);
                values.Add("@IsByUser", IsByUser);
                db.Execute(procedure, values, commandType: CommandType.StoredProcedure);
                return trackingId;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void ValidateTransaction(long tid)
        {
            var procedure = "ValidateTransaction";
            var values = new DynamicParameters();
            values.Add("@tid", tid);
            db.Execute(procedure, values, commandType: CommandType.StoredProcedure);
        }

        public void RevertBackOrder(long tid)
        {
            var procedure = "spRevertBackOrderCreatedByUser";
            var values = new DynamicParameters();
            values.Add("@tid",tid);
            db.Execute(procedure,values,commandType:CommandType.StoredProcedure);
        }

        private void OrderDetails(string trackingId)
        {
            var procedure = "OrderDetails";
            var values = new
            {
                @tid = trackingId
            };
            var orderDetails = db.Query<OrderDto>(procedure, values, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public OrderDetails GetOrderDetails(string tid)
        {
            var procedure = "GetOrderDetails";
            var values = new DynamicParameters();
            values.Add("@tid",tid);
            return db.Query<OrderDetails>(procedure, values, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public OrderDetails GetOrderDetailsFromTemp(string tid)
        {
            var procedure = "GetOrderDetailsFromTemp";
            var values = new DynamicParameters();
            values.Add("@tid", tid);
            return db.Query<OrderDetails>(procedure, values, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public int getNewOrdersCount(string email,bool isAdmin)
        {
            var procedure = "getNewOrdersCount";
            var values = new DynamicParameters();
            values.Add("@Email",email);
            values.Add("@IsAdmin",isAdmin);
            return db.Query<int>(procedure,values,commandType:CommandType.StoredProcedure).SingleOrDefault();
        }

        public DashboardStats spGetDashboardStats()
        {
            DashboardStats dashboardStats = new DashboardStats();
            var procedure = "spGetDashboardStats";
            var obj = db.QueryMultiple(procedure);
            dashboardStats.OrderCountByStatus = obj.Read<string>().Single().Split(',');
            dashboardStats.TotalOrders = obj.Read<int>().Single();
            dashboardStats.TotalRevenue = obj.Read<double>().Single();
            return dashboardStats;
        }

        //public void CreateOrder(double amt,string userId)
        //{
        //    try
        //    {
        //        var key = _configuration.GetValue<string>("RazorpayKeyID");
        //        var secret = _configuration.GetValue<string>("RazorpayKeySecret");
        //        int simpleAmount = Convert.ToInt32(amt*100);
        //        RazorpayClient client = new RazorpayClient(key,secret);
        //        Dictionary<string, object> options = new Dictionary<string, object>();
        //        options.Add("amount", simpleAmount); // amount in the smallest currency unit
        //        options.Add("receipt", DateTime.Now.ToString()+"_"+userId);
        //        options.Add("currency", "INR");
        //        Order order = client.Order.Create(options);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}