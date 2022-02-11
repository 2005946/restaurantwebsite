using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebsite.Data
{
    public class OrderHistory
    {
        [Key, Required]
        public int OrderNo { get; set; }
        [Required, StringLength(30)]
        public string Email { get; set; }
    }
}
