using Microsoft.EntityFrameworkCore;
using UMS1._0.Models;
using UMS1._0.Requests;

namespace UMS1._0.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly UniversityDbContext _dbContext;

    public TeacherController(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Post([FromBody] CreateTeacherRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var teacher = new Teacher
        {
            Name = request.Name,
            Email = request.Email,
            HireDate = request.HireDate
        };

        _dbContext.Teachers.Add(teacher);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Teacher created successfully", TeacherID = teacher.ID });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateTeacherRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var teacher = _dbContext.Teachers.FirstOrDefault(t => t.ID == id);
        if (teacher == null)
        {
            return NotFound(new { Message = "Teacher not found" });
        }

        teacher.Name = request.Name;
        teacher.Email = request.Email;
        teacher.HireDate = request.HireDate;

        _dbContext.SaveChanges();

        return Ok(new { Message = "Teacher updated successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var teacher = _dbContext.Teachers.Include(t => t.Courses).FirstOrDefault(t => t.ID == id);

        if (teacher == null)
        {
            return NotFound(new { Message = "Teacher not found" });
        }

        // Unassign courses from this teacher
        foreach (var course in teacher.Courses)
        {
            course.TeacherID = 0; // Or set to a default value if required
        }

        _dbContext.Teachers.Remove(teacher);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Teacher deleted successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _dbContext.Teachers.RemoveRange(_dbContext.Teachers);
        _dbContext.SaveChanges();

        return Ok(new { Message = "All teachers deleted successfully" });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var teacher = _dbContext.Teachers.Include(t => t.Courses).FirstOrDefault(t => t.ID == id);

        if (teacher == null)
        {
            return NotFound(new { Message = "Teacher not found" });
        }

        var response = new TeacherResponse
        {
            ID = teacher.ID,
            Name = teacher.Name,
            Email = teacher.Email,
            HireDate = teacher.HireDate,
            Courses = teacher.Courses.Select(c => new CourseResponse
            {
                ID = c.ID,
                Name = c.Name,
                Credits = c.Credits,
                TeacherID = c.TeacherID,
                TeacherName = teacher.Name
            }).ToList()
        };

        return Ok(response);
    }
}
