using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.BAL.Operations
{
    public class CourierTypeOperations
    {
        private readonly CourierTypeRepository courierTypeRepository = new CourierTypeRepository();

        public List<CourierTypeDto> GetAllCourierTypes()
        {
            return courierTypeRepository.GetAllCourierTypes();
        }

        public CourierTypeDto GetCourierType(int id)
        {
            return courierTypeRepository.GetCourierType(id);
        }

        public int CreateCourierType(CourierTypeDto courierTypeDto)
        {
            return courierTypeRepository.CreateCourierType(courierTypeDto);
        }

        public void UpdateCourierType(CourierTypeDto courierTypeDto)
        {
             courierTypeRepository.UpdateCourierType(courierTypeDto);
        }

        public void DeleteCourierType(int id)
        {
             courierTypeRepository.DeleteCourierType(id);
        }
    }
}
