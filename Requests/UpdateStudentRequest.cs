using System.ComponentModel.DataAnnotations;

namespace UMS1._0.Requests;

public class UpdateStudentRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public DateTime DateOfBirth { get; set; }

	public List<int> CourseIDs { get; set; } // Optional: Update enrolled courses
}
