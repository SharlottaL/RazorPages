using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Academy.Data;
using Academy.Models;

namespace Academy.Pages.Groups
{
    public class CreateModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public CreateModel(Academy.Data.AcademyContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["direction"] = new SelectList(_context.Directions, "direction_id", "direction_name");
            return Page();
        }

        [BindProperty]
        public Group Group { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Groups.Add(Group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
