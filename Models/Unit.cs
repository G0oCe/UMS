namespace UMS1._0.Models;

public class Unit
{
	public int ID { get; set; }
	public string Title { get; set; }
	public int CourseID { get; set; }
	public string Description { get; set; }

	// Navigation Properties
	public Course Course { get; set; } // Many-to-One
}
