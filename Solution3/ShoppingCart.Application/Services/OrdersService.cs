using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class OrdersService : IOrdersService
    {
        private IOrdersRepository _ordersRepo;
        private ICartsRepository _cartsRepo;
        private IMapper _mapper;
        private IProductsRepository _productsRepo;

        // copy from products service
        public OrdersService(IOrdersRepository ordersRepository, ICartsRepository cartsRepository, IProductsRepository productsRepository, IMapper mapper)

        {
            _ordersRepo = ordersRepository;
            _cartsRepo = cartsRepository;
            _productsRepo = productsRepository;
            _mapper = mapper;
        }

        public void CheckOut(string email)
        {
            // get a list of products that have been added to the cart for
            // the given email (from db)
            var carts = _cartsRepo.GetCarts().Where(x=>x.Email == email);

            // loop within the list of products to check qty from the stock
            // if you find a product with qty > stock - throw new Exeption("Not enough stock") OR!!!!
            // if you find a product with qty > stock - feturn false
            foreach(var cart in carts)
            {
                var product = _productsRepo.GetProduct(cart.Product.Id);

                if (cart.Qty > product.Stock)
                    throw new Exception("Out Of Stock!");
            }

            // 3. create an order
            Guid orderId = Guid.NewGuid();
            Order o = new Order();
            o.Id = orderId;
            o.DatePlaced = DateTime.Now;
            o.UserEmail = email;

            // Call the AddOrder from inside the IOrdersRepository (3)
            _ordersRepo.AddOrder(o);


            // 4. loop with the list of products and create an OrderDetail for each of the products
            // start loop
            List<OrderDetail> details = new List<OrderDetail>();
            foreach (var cart in carts)
            {
                var product = _productsRepo.GetProduct(cart.Product.Id);

                OrderDetail detail = new OrderDetail();

                detail.OrderFK = orderId;
                detail.ProductFK = cart.Product.Id;
                detail.Quantity = cart.Qty;
                detail.Price = Math.Round(cart.Product.Price * cart.Qty, 2);

                details.Add(detail);

                // deduct qty from stock
                product.Stock -= cart.Qty;
                // end loop
            }
            _ordersRepo.AddOrderDetails(details);
            _cartsRepo.EmptyCart(carts);
        }
    }
}
