using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    class OrdersService : IOrdersService
    {
        private IOrdersRepository _ordersRepo;
        private IMapper _mapper;
        private IProductsRepository _productsRepo;

        // copy from products service
        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepo = ordersRepository;
        }

        /*
 
        approach - Storing items in cart table in db
        a) User Must be Logged In
        b) In Check out method fetch list of cart items from db 

        */
        IQueryable<ProductViewModel> GetProducts(string email)
        {
            //var list = _productsRepo.GetProducts().Where(x => x.CategoryId == category)
            //           .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
            //return list;
            throw new NotImplementedException();
        }

        public void CheckOut(string email)
        {
            // get a list of products that have been added to the cart for
            // the given email (from db)

            // loop within the list of products to check qty from the stock
            // if you find a product with qty > stock - throw new Exeption("Not enough stock") OR!!!!
            // if you find a product with qty > stock - feturn false

            // 3. create an order
            Guid orderId = Guid.NewGuid();
            Order o = new Order();
            o.Id = orderId;
            // continue setting up other properties

            // Call the AddOrder from inside the IOrdersRepository (3)
            //_ordersRepo.AddOrder(o);


            // 4. loop with the list of products and create an OrderDetail for each of the products
            // start loop
            OrderDetail detail = new OrderDetail();
            detail.OrderFK = orderId;
            // productFK = prodcutId From Loop

            // continue setting other properties 

            // deduct qty from stock -- IMPORTANT

            // end loop

            // Call the AddOrderDetail from inside the IOrdersRepository (4)
            // loop then call method from _ordersRepo
            //_ordersRepo.AddOrder(o);


            throw new NotImplementedException();
        }

        IQueryable<ProductViewModel> IOrdersService.GetProducts(string email)
        {
            throw new NotImplementedException();
        }
    }
}
