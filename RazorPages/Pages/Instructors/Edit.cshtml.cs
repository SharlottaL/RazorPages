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
    public class EditModel : InstructorCoursesPageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public EditModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
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

            Instructor instructor =  await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i=>i.Courses)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }
            Instructor = instructor;
            PopulateAssingnedCourseData(_context, Instructor);
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

            _context.Attach(Instructor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(Instructor.ID))
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
       public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (id == null) return NotFound();
            Instructor insrtructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (insrtructorToUpdate == null) return NotFound();

            bool success = await TryUpdateModelAsync<Instructor>
                (
                insrtructorToUpdate,
                "Instructor",
                i => i.FirstName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment
                );
            if(success)
            {
                if (String.IsNullOrWhiteSpace(insrtructorToUpdate.OfficeAssignment?.Location))
                    insrtructorToUpdate.OfficeAssignment = null;
                UpdateInstructorsCourses(selectedCourses, insrtructorToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateInstructorsCourses(selectedCourses, insrtructorToUpdate);
            PopulateAssingnedCourseData(_context, insrtructorToUpdate);
            return Page();
        }
        public void UpdateInstructorsCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if(selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }
            HashSet<string> selectedCoursesHS = new HashSet<string>(selectedCourses);
            HashSet<int> instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach(Course course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                        instructorToUpdate.Courses.Add(course);
                }
                else
                {
                    if(instructorCourses.Contains(course.CourseID))
                    {
                        Course courseToRemove = instructorToUpdate.Courses.Single(c => c.CourseID == course.CourseID);
                        instructorToUpdate.Courses.Remove(courseToRemove);
                    }
                }
            }
        }
        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
