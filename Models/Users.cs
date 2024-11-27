namespace UMS1._0.Models;

public class User
{
	public int ID { get; set; }
	public string Username { get; set; }
	public string PasswordHash { get; set; } // Store hashed passwords
	public string Role { get; set; } // Admin or Teacher
}
