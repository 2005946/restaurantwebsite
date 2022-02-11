using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RestaurantWebsite.Pages.Admin
{
    [Authorize]
    public class ConfirmationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
