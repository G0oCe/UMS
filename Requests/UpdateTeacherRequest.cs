using System.ComponentModel.DataAnnotations;

namespace UMS1._0.Requests;

public class UpdateTeacherRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public DateTime HireDate { get; set; }
}
