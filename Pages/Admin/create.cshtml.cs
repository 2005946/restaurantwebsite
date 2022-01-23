using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantWebsite.wwwroot.Data;

namespace RestaurantWebsite.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class createModel : PageModel
    {
        private AppDbContext _db;
        [BindProperty]
        public menu MenuItem { get; set; }
        public createModel(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            else
            {
                MenuItem.Active = true;
                foreach (var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    MenuItem.ImageData = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
                _db.tblMenu.Add(MenuItem);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Admin/create");
            } 
        }
    }
}
