using Stregsystem.Models;

namespace Stregsystem.Exceptions;

public class UserNotFoundException : Exception
{
	public string Username { get; }
	public UserNotFoundException(string username) : base()
	{
        Username = username;
	}

    public UserNotFoundException(string username, string? message) : base(message)
    {
        Username = username;
    }
}