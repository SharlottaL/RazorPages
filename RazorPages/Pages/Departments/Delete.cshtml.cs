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
    public class DeleteModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public DeleteModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;
        public string ConcurrencyErrorMessage { get; set; }

        /*public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }*/
        public async Task<IActionResult> OnGetAsync(int id, bool? concurrencyError)
        {
            Department = await _context.Departments
                                .Include(d => d.Administrator)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(d => d.DepartmentID == id);
            if (Department == null) return NotFound();

            if (concurrencyError.GetValueOrDefault())
                ConcurrencyErrorMessage = "Удавляемая запись была зменена другим пользовалетем.Удавление было прервано. Если Вы все-таки хотите удавить эту запись, удавите ее еще раз";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (await _context.Departments.AnyAsync(d => d.DepartmentID == id))
                {
                    _context.Departments.Remove(Department);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return RedirectToPage("./Delete", new { concurrencyError = true, id = id });
            }
        }

        /*public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                Department = department;
                _context.Departments.Remove(Department);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }*/
    }
}