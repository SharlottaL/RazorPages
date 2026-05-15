using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Courses
{
    public class EditModel : DepartmentNamePageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public EditModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            Course = course;
            //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            PopulateDepartmentsDropdownList(_context, Course.DepartmentID);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
       /* public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.CourseID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }*/
       public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            Course courseToUpdate = await _context.Courses.FindAsync(id);
            if (courseToUpdate == null) return NotFound();

            bool success = await TryUpdateModelAsync<Course>
                (
                courseToUpdate,
                "course",
                c => c.Credits, c => c.DepartmentID, c => c.Title
                );
            if(success)
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateDepartmentsDropdownList(_context, courseToUpdate.DepartmentID);
            return Page();
        }
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
