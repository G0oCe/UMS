using System.ComponentModel.DataAnnotations;

namespace UMS1._0.Requests;

public class CreateAttendanceRequest
{
	[Required]
	public int StudentID { get; set; }

	[Required]
	public int CourseID { get; set; }

	[Required]
	public DateTime Date { get; set; }

	[Required]
	[RegularExpression("^(Present|Absent|Late)$", ErrorMessage = "Invalid status.")]
	public string Status { get; set; }
}
