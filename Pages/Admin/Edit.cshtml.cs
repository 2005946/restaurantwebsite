using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebsite.wwwroot.Data;

namespace RestaurantWebsite.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public menu menu { get; set; }
        private readonly AppDbContext _db;
        public EditModel(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int mealID)
        {
            menu = await _db.tblMenu.FindAsync(mealID);
            if (menu == null)
            {
                return RedirectToPage("/Admin/Menu");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSave()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.Attach(menu).State = EntityState.Modified;
            try
            {
                foreach (var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    menu.ImageData = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new Exception($"Item {menu.mealID} could not be updated", e);
            }
            return RedirectToPage("/Admin/Menu");
        }
    }
}
