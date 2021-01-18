using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICartsService
    {
        void AddCart(CartViewModel cart);

        IQueryable<CartViewModel> GetCarts();

        IQueryable<CartViewModel> GetCart(Guid id);

        IQueryable<CartViewModel> GetCarts(string email);

        void RemoveFromCart(Guid id, string email);

        void EmptyCart(string email);

        void UpdateQuantity(Guid id, int quantity, string email);
    }
}
