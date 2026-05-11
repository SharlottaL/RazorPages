using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Academy.Data;
using Academy.Models;

namespace Academy.Pages.Directions
{
    public class DeleteModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public DeleteModel(Academy.Data.AcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Direction Direction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Directions.FirstOrDefaultAsync(m => m.direction_id == id);

            if (direction == null)
            {
                return NotFound();
            }
            else
            {
                Direction = direction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Directions.FindAsync(id);
            if (direction != null)
            {
                Direction = direction;
                _context.Directions.Remove(Direction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
