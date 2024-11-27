using System.ComponentModel.DataAnnotations.Schema;

namespace UMS1._0.Models;

public class Student
{
	public int ID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime DateOfBirth { get; set; }
	public DateTime EnrollmentDate { get; set; }

	// Navigation Properties
	public ICollection<Course>? Courses { get; set; } // Many-to-Many
	public ICollection<Attendance>? Attendance { get; set; } // One-to-Many
}
