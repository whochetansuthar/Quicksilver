using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.BAL.Operations
{
    public class RateOperations
    {
        private readonly RateRepository rateRepository = new RateRepository();

        public List<RateDto> GetAllRates()
        {
            return rateRepository.GetAllRates();
        }

        public RateDto GetRate(int id)
        {
            return rateRepository.GetRate(id);
        }

        public int CreateRates(RateDto rateDto)
        {
            return rateRepository.CreateRate(rateDto);
        }

        public void UpdateRates(RateDto rateDto)
        {
            rateRepository.UpdateRate(rateDto);
        }

        public void DeleteRates(int id)
        {
            rateRepository.DeleteRate(id);
        }
    }
}
