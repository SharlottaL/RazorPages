using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public CreateModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            PopulateDepartmentsDropdownList(_context);
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Courses.Add(Course);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
            Course emptyCourse = new Course();
            bool success = await TryUpdateModelAsync<Course>
                (
                emptyCourse,
                "course",
                s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits
                );
                if(success)
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
            }
            PopulateDepartmentsDropdownList(_context, emptyCourse.DepartmentID);
            return Page();
        }
    }
}
