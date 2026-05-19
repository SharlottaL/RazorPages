using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RazorPages.Data;
using RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPages.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly RazorPages.Data.RazorPagesContosoUniversityContext _context;

        public EditModel(RazorPages.Data.RazorPagesContosoUniversityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;
        public SelectList InstructorNameSL { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Department = await _context.Departments
                                .Include(d => d.Administrator)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var department =  await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (Department == null)
            {
                return NotFound();
            }
            //this.Department = Department;
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FirstName");
            //ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        /*public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(Department.DepartmentID))
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
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid) return Page();

            Department department = await _context.Departments
                                            .Include(d => d.Administrator)
                                            .FirstOrDefaultAsync(m => m.DepartmentID == id);
            //TODO:
            if (department == null) return HandleDeletedDepartment();
            _context.Entry(department)
                    .Property(d => d.ConcurrencyToken)
                    .OriginalValue = this.Department.ConcurrencyToken;

            bool success = await TryUpdateModelAsync<Department>
                (
                department,
                "Department",
                d => d.Name, d => d.StartDate, d => d.Budget, d => d.InstructorID
                );
            if (success)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    EntityEntry exceptionEntry = ex.Entries.Single();
                    Department clientValues = exceptionEntry.Entity as Department;
                    PropertyValues databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Невозможно сохранить, Department был удален");
                        return Page();
                    }
                    Department dbValues = databaseEntry.ToObject() as Department;
                    //TODO:
                    await SetDbErrorMessage(dbValues, clientValues, _context);
                    this.Department.ConcurrencyToken = dbValues.ConcurrencyToken;
                    ModelState.Remove($"{nameof(Department)}.{nameof(Department.ConcurrencyToken)}");
                }
            }
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return Page();
        }
        IActionResult HandleDeletedDepartment()
        {
            ModelState.AddModelError
                (
                    string.Empty,
                    "HandleDeletedDepartment: Департамент был удален другим пользователем"
                );
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);
            return Page();
        }
        async Task SetDbErrorMessage(Department dbValues, Department clientValues, RazorPagesContosoUniversityContext context)
        {
            if (dbValues.Name != clientValues.Name) ModelState.AddModelError("Department.Name", $"Current value:{dbValues.Name}");
            if (dbValues.Budget != clientValues.Budget) ModelState.AddModelError("Department.Budget", $"Current value:{dbValues.Budget:c}");
            if (dbValues.StartDate != clientValues.StartDate) ModelState.AddModelError("Department.StartDate", $"Current value:{dbValues.StartDate:d}");

            if (dbValues.InstructorID != clientValues.InstructorID)
            {
                Instructor dbInstructor = await _context
                                                .Instructors
                                                .FindAsync(dbValues.InstructorID);
                ModelState.AddModelError("Department.InstructorID", $"Current value:{dbInstructor.FullName}");
            }
            ModelState.AddModelError(string.Empty, "Error");
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}