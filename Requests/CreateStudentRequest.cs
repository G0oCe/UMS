using System.ComponentModel.DataAnnotations;

namespace UMS1._0.Requests;

public class CreateStudentRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public DateTime DateOfBirth { get; set; }

	[Required]
	public DateTime EnrollmentDate { get; set; }

	[Required]
	public List<int> CourseIDs { get; set; } // List of course IDs to enroll in
}
