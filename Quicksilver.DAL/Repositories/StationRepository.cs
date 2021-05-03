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
    public class StationRepository
    {
        private readonly IDbConnection db = new SqlConnection(DbConnectionString.ConStr);

        public List<StationDto> GetAllStations()
        {
            var sql = "select Stations.*,Cities.Name as CityName,Cities.StateId,States.Name as StateName from stations inner join cities on Stations.CityId=Cities.Id inner join States on States.Id=Cities.StateId";
            return db.Query<StationDto>(sql).ToList();
        }

        public StationDto GetStation(int id)
        {
            var sql = "select * from Stations where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            return db.Query<StationDto>(sql,values).Single();
        }

        public int CreateStation(StationDto stationDto)
        {
            var procedure = "spSaveStation";
            var values = new DynamicParameters();
            values.Add("@Id",stationDto.Id);
            values.Add("@Name", stationDto.Name);
            values.Add("@CityId", stationDto.CityId);
            return db.Query<int>(procedure,values,commandType:CommandType.StoredProcedure).Single();
        }

        public void UpdateStation(StationDto stationDto)
        {
            var procedure = "spSaveStation";
            var values = new DynamicParameters();
            values.Add("@Id", stationDto.Id);
            values.Add("@Name", stationDto.Name);
            values.Add("@CityId", stationDto.CityId);
            db.Execute(procedure, values, commandType: CommandType.StoredProcedure);
        }

        public void DeleteStation(int id)
        {
            var sql = "Delete from Stations where Id=@Id";
            var values = new DynamicParameters();
            values.Add("@Id",id);
            db.Execute(sql,values);
        }
    }
}
