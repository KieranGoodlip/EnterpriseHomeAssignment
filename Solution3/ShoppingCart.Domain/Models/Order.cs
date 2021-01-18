using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        [Required]
        public string UserEmail { get; set; }

    }
}
