namespace UMS1._0.Models;

public class Course
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Credits { get; set; }
	public int TeacherID { get; set; }

	// Navigation Properties
	public Teacher Teacher { get; set; } // Many-to-One
	public ICollection<Student> Students { get; set; } // Many-to-Many
	public ICollection<Unit> Units { get; set; } // One-to-Many
}
