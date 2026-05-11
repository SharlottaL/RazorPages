using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academy.Data;
using Academy.Models;

namespace Academy.Pages.Directions
{
    public class EditModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public EditModel(Academy.Data.AcademyContext context)
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

            var direction =  await _context.Directions.FirstOrDefaultAsync(m => m.direction_id == id);
            if (direction == null)
            {
                return NotFound();
            }
            Direction = direction;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Direction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectionExists(Direction.direction_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DirectionExists(int id)
        {
            return _context.Directions.Any(e => e.direction_id == id);
        }
    }
}
