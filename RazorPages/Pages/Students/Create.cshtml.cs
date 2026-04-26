using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public CreateModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            RazorPages.Models.Student emptyStudent = new Student();
            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",
                s => s.FirstName, s => s.LastName, s => s.EnrollmentDate)
                )
            {
                _context.Students.Add(Student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Details", new {id = emptyStudent.ID});
                //return RedirectToPage($"./Details?id={emptyStudent.ID}");
            }
            return RedirectToPage("./Index");
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Students.Add(Student);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
        }
    }
}
