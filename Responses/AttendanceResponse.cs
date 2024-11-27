namespace UMS1._0.Controllers;

public class AttendanceResponse
{
	public int ID { get; set; }
	public DateTime Date { get; set; }
	public string Status { get; set; }
	public int StudentID { get; set; }
	public string StudentName { get; set; }
	public int CourseID { get; set; }
	public string CourseName { get; set; }
}
