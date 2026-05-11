using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;
using RazorPages.Models.ViewModels;

namespace RazorPages.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public IndexModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }
        //public async Task OnGetAsync()
        //{
        //    //Instructor = await _context.Instructors.ToListAsync();
        //}
        // public IList<Instructor> Instructor { get;set; } = default!;
        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public async Task OnGetAsync(int? id, int? courseID)
        {
            InstructorData = new InstructorIndexData();
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .ThenInclude(c => c.Department)
                .OrderBy(i => i.LastName)
                .ToListAsync();
            if( id != null)
            {
                InstructorID = id.Value;
                Instructor instructor = InstructorData.Instructors.Where(i => i.ID == id.Value).Single();
                InstructorData.Courses = instructor.Courses;
            }
            if(courseID != null)
            {
                CourseID = courseID.Value;
                IEnumerable<Enrollment> enrollments = await _context.Enrollments
                   .Where(x => x.CourseID == courseID.Value)
                    .Include(i => i.Student)
                    .ToListAsync();
                InstructorData.Enrollments = enrollments;
                    
            }
        }

    }
}
