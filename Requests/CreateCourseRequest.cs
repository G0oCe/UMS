using System.ComponentModel.DataAnnotations;

namespace UMS1._0.Requests;

public class CreateCourseRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	[Range(1, 10)]
	public int Credits { get; set; }

	[Required]
	public int TeacherID { get; set; } // Teacher responsible for the course
}
