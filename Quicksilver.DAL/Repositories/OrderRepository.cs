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
    public class OrderRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<GetAllOrders> GetAllOrders(string email)
        {
            var procedure = "GetAllOrders";
            var values = new DynamicParameters();
            values.Add("@UserName", email);
            return db.Query<GetAllOrders>(procedure,values,commandType:CommandType.StoredProcedure).ToList();
        }

        public void DeleteOrders(int id)
        {
            var procedure = "DeleteOrder";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            db.Execute(procedure,values,commandType: CommandType.StoredProcedure);
        }
        public int UpdateStatus(int id,string type)
        {
            var procedure = "UpdateStatus";
            var values = new DynamicParameters();
            values.Add("@Id", id);
            values.Add("@type", type);
            return db.Query<int>(procedure, values,commandType: CommandType.StoredProcedure).SingleOrDefault();
        }
    }
}