using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Academy.Data;
using Academy.Models;

namespace Academy.Pages.Disciplines
{
    public class DetailsModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public DetailsModel(Academy.Data.AcademyContext context)
        {
            _context = context;
        }

        public Discipline Discipline { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines.FirstOrDefaultAsync(m => m.discipline_id == id);
            if (discipline == null)
            {
                return NotFound();
            }
            else
            {
                Discipline = discipline;
            }
            return Page();
        }
    }
}
