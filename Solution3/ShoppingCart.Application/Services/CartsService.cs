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

        public void EmptyCart(string email)
        {
            var cToDelete = _cartsRepo.GetCarts().Where(x => x.Email == email);

            if (cToDelete != null)
            {
                _cartsRepo.EmptyCart(cToDelete);
            }
        }

        public IQueryable<CartViewModel> GetCart(Guid id)
        {
            var cart = _cartsRepo.GetCart(id).ProjectTo<CartViewModel>(_mapper.ConfigurationProvider);
            return cart;
        }

        public IQueryable<CartViewModel> GetCarts()
        {
            var products = _cartsRepo.GetCarts().ProjectTo<CartViewModel>(_mapper.ConfigurationProvider);

            return products;
        }

        public IQueryable<CartViewModel> GetCarts(string email)
        {
            var list = _cartsRepo.GetCarts().Where(x => x.Email == email)
                       .ProjectTo<CartViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public void RemoveFromCart(Guid id, string email)
        {
            var cToDelete = _cartsRepo.GetCarts().Where(x => x.Email == email && x.Product_FK == id);

            if (cToDelete != null)
            {
                _cartsRepo.DeleteCarts(cToDelete);
            }
        }

        public void UpdateQuantity(Guid id, int quantity, string email)
        {
            _cartsRepo.UpdateQuantity(id, quantity, email);
        }
    }
}
