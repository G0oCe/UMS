namespace UMS1._0.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly UniversityDbContext _dbContext;

    public ReportsController(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("high-attendance")]
    public IActionResult GetHighAttendanceStudents()
    {
        var result = _dbContext.Attendance
            .GroupBy(a => new { a.StudentID, a.CourseID })
            .Select(g => new
            {
                StudentID = g.Key.StudentID,
                CourseID = g.Key.CourseID,
                AttendancePercentage = (g.Count(a => a.Status == "Present") * 100.0) / g.Count()
            })
            .Where(x => x.AttendancePercentage >= 80)
            .ToList();

        return Ok(result);
    }

    [HttpGet("low-attendance")]
    public IActionResult GetLowAttendanceStudents()
    {
        var result = _dbContext.Attendance
            .GroupBy(a => new { a.StudentID, a.CourseID })
            .Select(g => new
            {
                StudentID = g.Key.StudentID,
                CourseID = g.Key.CourseID,
                AttendancePercentage = (g.Count(a => a.Status == "Present") * 100.0) / g.Count()
            })
            .Where(x => x.AttendancePercentage < 50)
            .ToList();

        return Ok(result);
    }

    [HttpGet("course-enrollment")]
    public IActionResult GetCourseEnrollmentStats()
    {
        var result = _dbContext.Courses
            .Select(c => new
            {
                CourseID = c.ID,
                CourseName = c.Name,
                EnrollmentCount = c.Students.Count
            })
            .OrderByDescending(c => c.EnrollmentCount)
            .ToList();

        return Ok(new
        {
            MostEnrolledCourse = result.FirstOrDefault(),
            LeastEnrolledCourse = result.LastOrDefault()
        });
    }
}
