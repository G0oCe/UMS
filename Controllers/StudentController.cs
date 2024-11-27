using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UMS1._0.Models;
using UMS1._0.Requests;

namespace UMS1._0.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly UniversityDbContext _dbContext;

    public StudentController(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Post([FromBody] CreateStudentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var courses = _dbContext.Courses.Where(c => request.CourseIDs.Contains(c.ID)).ToList();
        if (courses.Count != request.CourseIDs.Count)
        {
            return BadRequest("One or more CourseIDs are invalid.");
        }

        var student = new Student
        {
            Name = request.Name,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            EnrollmentDate = request.EnrollmentDate,
            Courses = courses
        };

        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Student created successfully", StudentID = student.ID });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateStudentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var student = _dbContext.Students.Include(s => s.Courses).FirstOrDefault(s => s.ID == id);
        if (student == null)
        {
            return NotFound(new { Message = "Student not found" });
        }

        student.Name = request.Name;
        student.Email = request.Email;
        student.DateOfBirth = request.DateOfBirth;

        if (request.CourseIDs != null)
        {
            var courses = _dbContext.Courses.Where(c => request.CourseIDs.Contains(c.ID)).ToList();
            student.Courses = courses;
        }

        _dbContext.SaveChanges();

        return Ok(new { Message = "Student updated successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _dbContext.Students.RemoveRange(_dbContext.Students);
        _dbContext.SaveChanges();

        return Ok(new { Message = "All students deleted successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var student = _dbContext.Students.Include(s => s.Courses).FirstOrDefault(s => s.ID == id);

        if (student == null)
        {
            return NotFound(new { Message = "Student not found" });
        }

        // Remove relationships with courses
        student.Courses.Clear();

        _dbContext.Students.Remove(student);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Student deleted successfully" });
    }
    
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var students = _dbContext.Students.Include(s => s.Courses).ToList();

        var response = students.Select(student => new StudentResponse
        {
            ID = student.ID,
            Name = student.Name,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth,
            EnrollmentDate = student.EnrollmentDate,
            Courses = student.Courses.Select(c => new CourseResponse
            {
                ID = c.ID,
                Name = c.Name,
                Credits = c.Credits,
                TeacherID = c.TeacherID,
                TeacherName = c.Teacher?.Name
            }).ToList()
        });

        return Ok(response);
    }
}
