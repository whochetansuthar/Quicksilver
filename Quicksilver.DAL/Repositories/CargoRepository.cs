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
    public class CargoRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<CargoDto> GetAllCargoes()
        {
            var sql = "select * from cargoes";
            return db.Query<CargoDto>(sql).ToList();
        }

        public CargoDto GetCargo(int id)
        {
            var sql = "select * from cargoes where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            return db.Query<CargoDto>(sql,values).Single();
        }

        public int CreateCargo(CargoDto cargoDto)
        {
            var procedure = "Insert into Cargoes values(@CargoCompany,@CargoCompanyMobileNo);SELECT SCOPE_IDENTITY();";
            var values = new DynamicParameters();
            values.Add("@CargoCompany",cargoDto.CargoCompanyName);
            values.Add("@CargoCompanyMobileNo", cargoDto.CargoCompanyMobileNo);
            return db.Query<int>(procedure,values).Single();
        }

        public void UpdateCargo(CargoDto cargoDto)
        {
            var procedure = "update Cargoes set CargoCompany=@CargoCompany,CargoCompanyMobileNo=@CargoCompanyMobileNo where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id", cargoDto.Id);
            values.Add("@CargoCompany", cargoDto.CargoCompanyName);
            values.Add("@CargoCompanyMobileNo", cargoDto.CargoCompanyMobileNo);
            db.Execute(procedure, values);
        }

        public void DeleteCargo(int id)
        {
            var procedure = "delete from cargoes where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id", id);
            db.Execute(procedure, values);
        }
    }
}
