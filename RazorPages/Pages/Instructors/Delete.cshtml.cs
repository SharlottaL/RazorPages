using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Instructors
{
    public class DeleteModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public DeleteModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instructor Instructor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }
            else
            {
                Instructor = instructor;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Instructor instructor = await _context.Instructors
                .Include(i => i.Courses)
                .SingleAsync(i => i.ID == id);
            if (instructor == null) return RedirectToPage("./Index");
            List<Department> departments = await _context.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            /* var instructor = await _context.Instructors.FindAsync(id);
             if (instructor != null)
             {
                 Instructor = instructor;
                 _context.Instructors.Remove(Instructor);
                 await _context.SaveChangesAsync();
             }

             return RedirectToPage("./Index");*/
            return RedirectToPage("./Index"); 
        }
    }
}
