using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using Quicksilver.DAL.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Quicksilver.DAL.Repositories
{
    public class CouponRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<CouponDto> GetAllCoupons()
        {
            var sql = "select * from Coupons";
            return db.Query<CouponDto>(sql).ToList();
        }

        public CouponDto GetCoupon(int id)
        {
            var sql = "select * from Coupons where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            return db.Query<CouponDto>(sql,values).Single();
        }

        public int CreateCoupon(CouponDto couponDto)
        {
            var procedure = "spSaveCoupon";
            var values = new DynamicParameters();
            values.Add("@Id",couponDto.Id);
            values.Add("@Code", couponDto.Code);
            values.Add("@DateCreated", couponDto.DateCreated);
            values.Add("@DateExpire", couponDto.DateExpired);
            values.Add("@Discount", couponDto.Discount);
            return db.Query<int>(procedure, values, commandType: CommandType.StoredProcedure).Single();
        }

        public void UpdateCoupon(CouponDto couponDto)
        {
            var procedure = "spSaveCoupon";
            var values = new DynamicParameters();
            values.Add("@Id", couponDto.Id);
            values.Add("@Code", couponDto.Code);
            values.Add("@DateExpire", couponDto.DateExpired);
            values.Add("@Discount", couponDto.Discount);
            db.Execute(procedure,values,commandType:CommandType.StoredProcedure);
        }

        public void DeleteCoupon(int id)
        {
            var sql = "Delete from Coupons where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            db.Execute(sql,values);
        }
    }
}
