using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Instructors
{
    public class CreateModel : InstructorCoursesPageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;
        readonly ILogger<InstructorCoursesPageModel> _logger;

        public CreateModel(RazorPages.Data.RazorPagesContosoUniversityContext context, ILogger<InstructorCoursesPageModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Instructor instructor = new Instructor();
            instructor.Courses = new List<Course>();
            PopulateAssingnedCourseData(_context, instructor);

            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            Instructor newInstructor = new Instructor();
            if(selectedCourses.Length > 0)
            {
                newInstructor.Courses = new List<Course>();
                _context.Courses.Load();
            }

            foreach(string course in selectedCourses)
            {
                Course foundCourse = await _context.Courses.FindAsync(int.Parse(course));
                if (foundCourse != null)
                    newInstructor.Courses.Add(foundCourse);
                else _logger.LogWarning("Course {course} not found", course);
            }
            try
            {
                bool success = await TryUpdateModelAsync<Instructor>
                    (
                    newInstructor,
                    "Instructor",
                    i => i.FirstName, i => i.LastName,
                    i => i.HireDate, i => i.OfficeAssignment
                    );
                if(success)
                {
                    _context.Instructors.Add(newInstructor);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            PopulateAssingnedCourseData(_context, newInstructor);
            return Page();
        }
        // For more information, see https://aka.ms/RazorPagesCRUD.
        /*public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Instructors.Add(Instructor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }*/
    }
}
