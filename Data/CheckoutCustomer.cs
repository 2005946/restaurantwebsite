using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebsite.Data
{
    public class CheckoutCustomer
    {
        [Key, StringLength(30)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int BasketID { get; set; }
    }
}
