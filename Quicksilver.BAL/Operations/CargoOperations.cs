using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.BAL.Operations
{
    public class CargoOperations
    {
        private readonly CargoRepository cargoRepository = new CargoRepository();

        public List<CargoDto> GetAllCargoes()
        {
            return cargoRepository.GetAllCargoes();
        }

        public CargoDto GetCargo(int id)
        {
            return cargoRepository.GetCargo(id);
        }

        public int CreateCargoes(CargoDto cargoDto)
        {
            return cargoRepository.CreateCargo(cargoDto);
        }

        public void UpdateCargoes(CargoDto cargoDto)
        {
            cargoRepository.UpdateCargo(cargoDto);
        }

        public void DeleteCargoes(int id)
        {
            cargoRepository.DeleteCargo(id);
        }
    }
}
