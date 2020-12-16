using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; } // C581B912-DF44-434B-8AD4-5343FC087507
            
        [Required(ErrorMessage ="Please input a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please input a price")]
        [Range(typeof(double), "0", "9999", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]
        public double Price { get; set; }

        public string Description { get; set; }
      
        public CategoryViewModel Category { get; set; }

        public string ImageUrl { get; set; }
        //stock
        //supplier

    }
}
