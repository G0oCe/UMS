namespace UMS1._0.Controllers;

public class StudentResponse
{
	public int ID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime DateOfBirth { get; set; }
	public DateTime EnrollmentDate { get; set; }
	public List<CourseResponse> Courses { get; set; }
}
