using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IOrdersRepository
    {
        Guid AddOrder(Order order);

        void AddOrderDetails(List<OrderDetail> details);
    }
}
