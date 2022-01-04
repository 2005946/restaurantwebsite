using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantWebsite.wwwroot.Data;

namespace RestaurantWebsite.Pages.Admin
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public menu menu { get; set; }
        private readonly AppDbContext _db;
        public EditModel(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            menu = await _db.tblMenu.FindAsync(id);
            if (menu == null)
            {
                return RedirectToPage("/Admin/Menu");
            }
            return Page();
        }
    }
}
