using System;
using System.Collections.Generic;
using System.Text;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.DTOs;

namespace Quicksilver.BAL.Operations
{
    public class OrderOperations
    {
        private readonly OrderRepository orderRepository = new OrderRepository();

        public List<GetAllOrders> GetAllOrders(string email)
        {
            return orderRepository.GetAllOrders(email);
        }

        public void DeleteOrders(int id)
        {
            orderRepository.DeleteOrders(id);
        }
    }
}