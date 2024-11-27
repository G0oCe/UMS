namespace UMS1._0.Models;

public class Attendance
{
	public int ID { get; set; }
	public int StudentID { get; set; }
	public int CourseID { get; set; }
	public DateTime Date { get; set; }
	public string Status { get; set; } // Present, Absent, Late

	// Navigation Properties
	public Student Student { get; set; } // Many-to-One
	public Course Course { get; set; } // Many-to-One
}

