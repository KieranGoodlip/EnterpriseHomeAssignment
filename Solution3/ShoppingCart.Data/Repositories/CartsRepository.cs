using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CartsRepository : ICartsRepository
    {
        ShoppingCartDbContext _context;

        public CartsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }

        public Guid AddToCart(Cart c)
        {
            try
            {
                _context.Carts.Add(c);
                _context.SaveChanges();
                return c.Id;
            }
            catch (Exception e)
            {
                return Guid.NewGuid();
            }
        }
    }
}
