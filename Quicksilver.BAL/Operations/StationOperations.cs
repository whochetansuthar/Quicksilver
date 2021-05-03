using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;


namespace Quicksilver.BAL.Operations
{
    public class StationOperations
    {
        private readonly StationRepository stationRepository = new StationRepository();

        public List<StationDto> GetAllStations()
        {
            return stationRepository.GetAllStations();
        }

        public StationDto GetStation(int id)
        {
            return stationRepository.GetStation(id);
        }

        public int CreateStation(StationDto stationDto)
        {
            return stationRepository.CreateStation(stationDto);
        }

        public void UpdateStations(StationDto stationDto)
        {
            stationRepository.UpdateStation(stationDto);
        }

        public void DeleteStation(int id)
        {
            stationRepository.DeleteStation(id);
        }
    }
}
