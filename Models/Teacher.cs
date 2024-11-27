namespace UMS1._0.Models;

public class Teacher
{
	public int ID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime HireDate { get; set; }

	// Navigation Properties
	public ICollection<Course> Courses { get; set; } // One-to-Many
}
