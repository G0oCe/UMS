namespace UMS1._0;

using Microsoft.AspNetCore.Identity;

public class HashingService
{
	private readonly PasswordHasher<string> _passwordHasher;

	// Constructor Injection
	public HashingService()
	{
		_passwordHasher = new PasswordHasher<string>();
	}

	// Hashes the password
	public string HashPassword(string username, string password)
	{
		return _passwordHasher.HashPassword(username, password);
	}

	// Verifies the password
	public bool VerifyPassword(string username, string inputPassword, string storedPasswordHash)
	{
		var result = _passwordHasher.VerifyHashedPassword(username, storedPasswordHash, inputPassword);
		return result == PasswordVerificationResult.Success;
	}
}