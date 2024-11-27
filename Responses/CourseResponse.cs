namespace UMS1._0.Controllers;

public class CourseResponse
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Credits { get; set; }
	public int TeacherID { get; set; }
	public string TeacherName { get; set; }
	public List<UnitResponse> Units { get; set; }
}
