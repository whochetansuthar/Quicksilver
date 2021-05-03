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

        public List<OrderDto> GetAllOrders()
        {
            var procedure = "GetAllOrders";
            return db.Query<OrderDto>(procedure,commandType:CommandType.StoredProcedure).ToList();
        }
    }
}
