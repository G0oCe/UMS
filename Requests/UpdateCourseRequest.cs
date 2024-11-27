using System.ComponentModel.DataAnnotations;

namespace UMS1._0.Requests;

public class UpdateCourseRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	[Range(1, 10)]
	public int Credits { get; set; }

	public List<int> StudentIDs { get; set; } // Optional: Update enrolled students
}
