using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IProductsService
    {
        IQueryable<ProductViewModel> GetProducts();

        IQueryable<ProductViewModel> GetProducts(int category);

        ProductViewModel GetProduct(Guid id);

        void AddProduct(ProductViewModel product);

        void DeleteProduct(Guid id);

        void DisableOrEnable(Guid id);

        
    }
}
