using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public IndexModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Department = await _context.Departments
                .Include(d => d.Administrator).ToListAsync();
        }
    }
}