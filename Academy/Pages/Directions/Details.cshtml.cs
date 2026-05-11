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
    public class DetailsModel : PageModel
    {
        private readonly Academy.Data.AcademyContext _context;

        public DetailsModel(Academy.Data.AcademyContext context)
        {
            _context = context;
        }

        public Direction Direction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Directions.FirstOrDefaultAsync(m => m.direction_id == id);
            if (direction == null)
            {
                return NotFound();
            }
            else
            {
                Direction = direction;
            }
            return Page();
        }
    }
}
