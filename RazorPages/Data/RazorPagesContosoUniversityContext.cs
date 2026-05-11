using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;

namespace RazorPages.Data
{
    public class RazorPagesContosoUniversityContext : DbContext
    {
        public RazorPagesContosoUniversityContext (DbContextOptions<RazorPagesContosoUniversityContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPages.Models.Student> Students { get; set; } = default!;
        public DbSet<RazorPages.Models.Enrollment> Enrollments { get; set; } = default!;
        public DbSet<RazorPages.Models.Course> Courses { get; set; } = default!;

        public DbSet<Department> Departments { get; set; } = default!;
        public DbSet<Instructor> Instructors { get; set; } = default!;
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("Courses")
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Instructor>().ToTable("Instructors");

        }
    }
}
