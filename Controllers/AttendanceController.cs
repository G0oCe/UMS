using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UMS1._0.Models;
using UMS1._0.Requests;

namespace UMS1._0.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly UniversityDbContext _dbContext;

    public AttendanceController(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize(Roles = "Teacher, Admin")]
    [HttpPost]
    public IActionResult Post([FromBody] CreateAttendanceRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var student = _dbContext.Students.FirstOrDefault(s => s.ID == request.StudentID);
        var course = _dbContext.Courses.FirstOrDefault(c => c.ID == request.CourseID);

        if (student == null || course == null)
        {
            return BadRequest("Invalid StudentID or CourseID.");
        }

        var attendance = new Attendance
        {
            StudentID = request.StudentID,
            CourseID = request.CourseID,
            Date = request.Date,
            Status = request.Status
        };

        _dbContext.Attendance.Add(attendance);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Attendance recorded successfully", AttendanceID = attendance.ID });
    }
    
    [Authorize(Roles = "Teacher, Admin")]
    [HttpPut]
    public IActionResult Put([FromBody] CreateAttendanceRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var attendance = _dbContext.Attendance.FirstOrDefault(a => a.StudentID == request.StudentID);
        if (attendance == null)
        {
            return NotFound(new { Message = "Attendance record not found" });
        }

        attendance.StudentID = request.StudentID;
        attendance.CourseID = request.CourseID;
        attendance.Date = request.Date;
        attendance.Status = request.Status;

        _dbContext.SaveChanges();

        return Ok(new { Message = "Attendance record updated successfully" });
    }
    
    [Authorize(Roles = "Teacher, Admin")]
    [HttpGet("student/{id}")]
    public IActionResult GetByStudent(int id)
    {
        var attendanceRecords = _dbContext.Attendance
            .Include(a => a.Course)
            .Include(a => a.Student)
            .Where(a => a.StudentID == id)
            .ToList();

        var response = attendanceRecords.Select(a => new AttendanceResponse
        {
            ID = a.ID,
            Date = a.Date,
            Status = a.Status,
            StudentID = a.StudentID,
            StudentName = a.Student.Name,
            CourseID = a.CourseID,
            CourseName = a.Course.Name
        });

        return Ok(response);
    }
    
    [Authorize(Roles = "Teacher, Admin")]
    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _dbContext.Attendance.RemoveRange(_dbContext.Attendance);
        _dbContext.SaveChanges();

        return Ok(new { Message = "All attendance records deleted successfully" });
    }
    
    [Authorize(Roles = "Teacher, Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var attendance = _dbContext.Attendance.FirstOrDefault(a => a.ID == id);

        if (attendance == null)
        {
            return NotFound(new { Message = "Attendance record not found" });
        }

        _dbContext.Attendance.Remove(attendance);
        _dbContext.SaveChanges();

        return Ok(new { Message = "Attendance record deleted successfully" });
    }
    
    [Authorize(Roles = "Teacher, Admin")]
    [HttpGet("course/{id}")]
    public IActionResult GetByCourse(int id)
    {
        var attendanceRecords = _dbContext.Attendance
            .Include(a => a.Course)
            .Include(a => a.Student)
            .Where(a => a.CourseID == id)
            .ToList();

        var response = attendanceRecords.Select(a => new AttendanceResponse
        {
            ID = a.ID,
            Date = a.Date,
            Status = a.Status,
            StudentID = a.StudentID,
            StudentName = a.Student.Name,
            CourseID = a.CourseID,
            CourseName = a.Course.Name
        });

        return Ok(response);
    }
}
