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
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Registration Input { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userInManager;

        public RegisterModel(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm)
        {
            _signInManager = sm;
            _userInManager = um;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email};
                var result = await _userInManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent:false);
                    await _userInManager.AddToRoleAsync(user, "Member");
                    return RedirectToPage("/Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
