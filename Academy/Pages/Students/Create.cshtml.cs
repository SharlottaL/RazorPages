using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Academy.Data;
using Academy.Models;

namespace Academy.Pages.Students
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
        ViewData["group"] = new SelectList(_context.Groups, "group_id", "group_name");
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
