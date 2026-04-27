using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;
using Microsoft.Extensions.Configuration;

namespace RazorPages.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public IndexModel(RazorPages.Data.RazorPagesContosoUniversityContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurentFilter { get; set; }
        public string CurentSort { get; set; }
        public IList<Student> Student { get;set; } = default!;


        readonly IConfiguration configuration;
        public PaginatedList<Student> Students { get; set; }
        public int PageSize;
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int pageSize = 5)
        {
            CurentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";  //descending
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null) pageIndex = 1;
            else searchString = currentFilter;
            CurentFilter = searchString;


            IQueryable<Student> students = from student in _context.Students select student;

            if (!String.IsNullOrEmpty(CurentFilter))
            {
                students =
                    students.
                    Where(s => s.LastName.Contains(CurentFilter) || s.FirstName.Contains(CurentFilter));
            }

            switch (sortOrder)
            {
                case "name_desc": students = students.OrderByDescending(s => s.LastName); break;
                case "date_desc": students = students.OrderByDescending(s => s.EnrollmentDate); break;
                case "Date": students = students.OrderBy(s => s.EnrollmentDate); break;
                default: students = students.OrderBy(s => s.LastName); break;
            }
       
        PageSize = pageSize;
            Students = await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageIndex ?? 1, PageSize);
            //Student = await students.AsNoTracking().ToListAsync();
            // Student = await _context.Students.ToListAsync();
        }
    }
}
