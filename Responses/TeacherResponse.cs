namespace UMS1._0.Controllers;

public class TeacherResponse
{
	public int ID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime HireDate { get; set; }
	public List<CourseResponse> Courses { get; set; }
}
