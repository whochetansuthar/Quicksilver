using Quicksilver.DAL.DTOs;
using Quicksilver.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.BAL.Operations.Locations
{
    public class StateOperations
    {
        private readonly LocationRepository locationRepository = new LocationRepository();

        public List<StateDto> GetState()
        {
            try
            {
                return locationRepository.GetAllStates();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StateDto GetStateSingle(int id)
        {
            try
            {
                return locationRepository.GetStateSingle(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int CreateState(String Name)
        {
            try
            {
                return locationRepository.CreateState(Name);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateState(StateDto stateDto)
        {
            try
            {
                locationRepository.UpdateState(stateDto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void DeleteState(int id)
        {
            try
            {
                locationRepository.DeleteState(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}