using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Academy.Data;
using Academy.Models;

namespace Academy.Pages.Directions
{
    public class IndexModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public IndexModel(Academy.Data.AcademyContext context)
        {
            _context = context;
        }

        public IList<Direction> Direction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Direction = await _context.Directions.ToListAsync();
        }
    }
}
