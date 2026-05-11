using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;
using RazorPages.Models.ViewModels;

namespace RazorPages.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public IndexModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get;set; } = default!;
        public IList<CourseViewModel> CourseVM { get;set; } = default!;
        public async Task OnGetAsync()
        {
            //Course = await _context.Courses
            //    .Include(c => c.Department).ToListAsync();
            Courses = await _context.Courses.Include(c => c.Department).AsNoTracking().ToListAsync();
            CourseVM = await _context.Courses
                .Select(
                p => new CourseViewModel { CourseID = p.CourseID, Title = p.Title, Credits = p.Credits, DepartmentName = p.Department.Name }
                ).ToListAsync();
        }
    }
}
