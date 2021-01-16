using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class CartsService : ICartsService
    {
        private IMapper _mapper;
        private ICartsRepository _cartsRepo;

        public CartsService(ICartsRepository cartsRepository
           , IMapper mapper
            )
        {
            _mapper = mapper;
            _cartsRepo = cartsRepository;
        }

        public void AddCart(CartViewModel c)
        {
            var myCart = _mapper.Map<Cart>(c);
            myCart.Product_FK = myCart.Product.Id;
            myCart.Product = null;

            _cartsRepo.AddToCart(myCart);
        }
    }
}
