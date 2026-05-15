using RazorPages.Data;
using RazorPages.Models;
using RazorPages.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace RazorPages.Pages.Instructors
{
    public class InstructorCoursesPageModel:PageModel
    {
        public List<AssignedCourseData> AssignedCourseDataList;
        public void PopulateAssingnedCourseData(RazorPagesContosoUniversityContext context, Instructor instructor)
        {
            DbSet<Course> allCourses = context.Courses;
            HashSet<int> instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            AssignedCourseDataList = new List<AssignedCourseData>();
            foreach(Course course in allCourses)
            {
                AssignedCourseDataList.Add(
                    new AssignedCourseData
                    {
                        CourseID = course.CourseID,
                        Title = course.Title,
                        Assigned = instructorCourses.Contains(course.CourseID)
                    });
            }
        }
    }
}
