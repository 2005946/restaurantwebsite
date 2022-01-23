using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantWebsite.Data;

namespace RestaurantWebsite.Pages.Account
{
    public class UserModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserModel(SignInManager<ApplicationUser> sm)
        {
            _signInManager = sm;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Index");
        }
    }
}
