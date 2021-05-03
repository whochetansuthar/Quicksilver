using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.BAL.Operations.Locations
{
    public class CityOperation
    {
        private readonly LocationRepository locationRepository = new LocationRepository();

        public List<CityDto> GetCities()
        {
            try
            {
                return locationRepository.GetAllCities();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public CityDto GetCitySingle(int id)
        {
            try
            {
                return locationRepository.GetCitySingle(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int CreateCity(CityDto cityDto)
        {
            try
            {
                return locationRepository.CreateCity(cityDto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateCity(CityDto cityDto)
        {
            try
            {
                locationRepository.UpdateCity(cityDto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void DeleteCity(int id)
        {
            try
            {
                locationRepository.DeleteCity(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
