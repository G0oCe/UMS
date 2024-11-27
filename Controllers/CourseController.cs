using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UMS1._0.Models;
using UMS1._0.Requests;

namespace UMS1._0.Controllers;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly UniversityDbContext _dbContext;

    public CourseController(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Post([FromBody] CreateCourseRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var teacher = _dbContext.Teachers.Find(request.TeacherID);
        if (teacher == null)
        {
            return BadRequest("Invalid TeacherID.");
        }

        var course = new Course
        {
            Name = request.Name,
            Credits = request.Credits,
            TeacherID = request.TeacherID,
            Teacher = teacher
        };

        _dbContext.Courses.Add(course);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Course created successfully", CourseID = course.ID });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateCourseRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var course = _dbContext.Courses.Include(c => c.Students).FirstOrDefault(c => c.ID == id);
        if (course == null)
        {
            return NotFound(new { Message = "Course not found" });
        }

        course.Name = request.Name;
        course.Credits = request.Credits;

        if (request.StudentIDs != null)
        {
            var students = _dbContext.Students.Where(s => request.StudentIDs.Contains(s.ID)).ToList();
            course.Students = students;
        }

        _dbContext.SaveChanges();

        return Ok(new { Message = "Course updated successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _dbContext.Courses.RemoveRange(_dbContext.Courses);
        _dbContext.SaveChanges();

        return Ok(new { Message = "All courses deleted successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var course = _dbContext.Courses.Include(c => c.Students).Include(c => c.Units).FirstOrDefault(c => c.ID == id);

        if (course == null)
        {
            return NotFound(new { Message = "Course not found" });
        }
        
        course.Students.Clear();
        
        _dbContext.Units.RemoveRange(course.Units);

        _dbContext.Courses.Remove(course);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Course deleted successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var course = _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Students)
            .Include(c => c.Units)
            .FirstOrDefault(c => c.ID == id);

        if (course == null)
        {
            return NotFound(new { Message = "Course not found" });
        }

        var response = new CourseResponse
        {
            ID = course.ID,
            Name = course.Name,
            Credits = course.Credits,
            TeacherID = course.TeacherID,
            TeacherName = course.Teacher?.Name,
            Units = course.Units.Select(u => new UnitResponse
            {
                ID = u.ID,
                Title = u.Title,
                Description = u.Description,
                CourseID = u.CourseID,
                CourseName = course.Name
            }).ToList()
        };

        return Ok(response);
    }
}
