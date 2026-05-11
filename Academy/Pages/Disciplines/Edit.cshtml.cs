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

namespace Academy.Pages.Disciplines
{
    public class EditModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public EditModel(Academy.Data.AcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Discipline Discipline { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline =  await _context.Disciplines.FirstOrDefaultAsync(m => m.discipline_id == id);
            if (discipline == null)
            {
                return NotFound();
            }
            Discipline = discipline;
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

            _context.Attach(Discipline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisciplineExists(Discipline.discipline_id))
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

        private bool DisciplineExists(int id)
        {
            return _context.Disciplines.Any(e => e.discipline_id == id);
        }
    }
}
