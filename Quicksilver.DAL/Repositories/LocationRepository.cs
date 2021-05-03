using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Quicksilver.DAL.DTOs;
using Dapper;

namespace Quicksilver.DAL.Repositories
{
    public class LocationRepository
    {
        private IDbConnection db = new SqlConnection(DbConnectionString.ConStr);
        public List<StateDto> GetAllStates()
        {
            List<StateDto> list;
                var sql = "select * from States";
                list = db.Query<StateDto>(sql).ToList();
            return list;
        }

        public StateDto GetStateSingle(int id)
        {
                var sql = "select * from States where Id=@Id";
                var values = new DynamicParameters();
                values.Add("@Id",id);
                var state = db.Query<StateDto>(sql,values).Single();
                return state;
        }

        public int CreateState(string Name)
        {
            if (GetStateByName(Name)>=0)
            {
                return -22;
            }
                var sql = "Insert into States values(@Name);SELECT SCOPE_IDENTITY();";
                var values = new DynamicParameters();
                values.Add("@Name", Name);
                var id = db.Query<int>(sql, values).Single();
                return id;
        }

        private int GetStateByName(string Name)
        {
            var sql = "select count(*) from States where LOWER(Name)=@Name";
            var values = new DynamicParameters();
            values.Add("@Name", Name.ToLower());
            var id = db.Query<int>(sql, values).Single();
            return id;
        }

        public void UpdateState(StateDto stateDto)
        {
                var sql = "update States set Name=@Name where Id=@Id";
                var values = new DynamicParameters();
                values.Add("@Id", stateDto.Id);
                values.Add("@Name", stateDto.Name);
                db.Execute(sql, values);
        }

        public void DeleteState(int id)
        {
                var sql = "delete from States where Id=@Id";
                var values = new DynamicParameters();
                values.Add("@Id", id);
                db.Execute(sql, values);
        }

        public List<CityDto> GetAllCities()
        {
            List<CityDto> list;
                var procedure = "spGetAllCities";
                list = db.Query<CityDto>(procedure,commandType:CommandType.StoredProcedure).ToList();
            return list;
        }

        public CityDto GetCitySingle(int id)
        {
            var procedure = "spGetCitySingle";
            var values = new DynamicParameters();
            values.Add("@Id", id);
            var city = db.Query<CityDto>(procedure, values, commandType: CommandType.StoredProcedure).Single();
            return city;
        }

        public int CreateCity(CityDto cityDto)
        {
            var sql = "Insert into Cities values(@Name,@Cid);SELECT SCOPE_IDENTITY();";
            var values = new DynamicParameters();
            values.Add("@Name", cityDto.Name);
            values.Add("@Cid", cityDto.StateId);
            var id = db.Query<int>(sql, values).Single();
            return id;
        }

        public void UpdateCity(CityDto cityDto)
        {
            var sql = "update Cities set Name=@Name,StateId=@Sid where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id", cityDto.Id);
            values.Add("@Sid", cityDto.StateId);
            values.Add("@Name", cityDto.Name);
            db.Execute(sql, values);
        }

        public void DeleteCity(int id)
        {
            var sql = "delete from Cities where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id", id);
            db.Execute(sql, values);
        }
    }
}
