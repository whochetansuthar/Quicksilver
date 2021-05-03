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
    public class CourierTypeRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<CourierTypeDto> GetAllCourierTypes()
        {
            return db.Query<CourierTypeDto>("select * from CourierTypes").ToList();
        }

        public CourierTypeDto GetCourierType(int id)
        {
            var sql = "select * from CourierTypes where Id = @Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            return db.Query<CourierTypeDto>(sql,values).Single();
        }

        public int CreateCourierType(CourierTypeDto courierTypeDto)
        {
            var procedure = "spSaveCourierType";
            var values = new DynamicParameters();
            values.Add("@Id",courierTypeDto.Id);
            values.Add("@Name", courierTypeDto.Name);
            return db.Query<int>(procedure,values,commandType:CommandType.StoredProcedure).Single();
        }
        public void UpdateCourierType(CourierTypeDto courierTypeDto)
        {
            var procedure = "spSaveCourierType";
            var values = new DynamicParameters();
            values.Add("@Id", courierTypeDto.Id);
            values.Add("@Name", courierTypeDto.Name);
            db.Execute(procedure, values, commandType: CommandType.StoredProcedure);
        }

        public void DeleteCourierType(int id)
        {
            var sql = "Delete from CourierTypes where Id=@Id";
            var value = new DynamicParameters();
            value.Add("@Id",id);
            db.Execute(sql,value);
        }
    }
}
