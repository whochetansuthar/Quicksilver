using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Dapper;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.DAL.Repositories
{
    public class RateRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<RateDto> GetAllRates()
        {
            var sql = "select Rates.*,CourierTypes.Name as CourierTypeName from Rates inner join CourierTypes on CourierTypes.Id=Rates.CourierTypeId";
            return db.Query<RateDto>(sql).ToList();
        }

        public RateDto GetRate(int id)
        {
            var sql = "select Rates.*,CourierTypes.Name as CourierTypeName from Rates inner join CourierTypes on CourierTypes.Id=Rates.CourierTypeId where Rates.Id=@Id";
            var values = new
            {
                @Id = id
            };
            return db.Query<RateDto>(sql,values).Single();
        }

        public int CreateRate(RateDto rateDto)
        {
            var sql = "Insert into Rates values(@MinWeight,@MaxWeight,@CourierTypeId,@Rate);SELECT SCOPE_IDENTITY();";
            var values = new DynamicParameters();
            values.Add("@MinWeight",rateDto.MinWeight);
            values.Add("@MaxWeight", rateDto.MaxWeight);
            values.Add("@CourierTypeId", rateDto.CourierTypeId);
            values.Add("@Rate", rateDto.Rate);
            return db.Query<int>(sql,values).Single();
        }

        public void UpdateRate(RateDto rateDto)
        {
            var sql = "update Rates set MinWeight=@MinWeight,MaxWeight=@MaxWeight,CourierTypeId=@CourierTypeId,Rate=@Rate where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@MinWeight", rateDto.MinWeight);
            values.Add("@MaxWeight", rateDto.MaxWeight);
            values.Add("@CourierTypeId", rateDto.CourierTypeId);
            values.Add("@Rate", rateDto.Rate);
            values.Add("@Id", rateDto.Id);
            db.Execute(sql, values);
        }

        public void DeleteRate(int id)
        {
            var sql = "delete from rates where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            db.Execute(sql,values); ;
        }
    }
}
