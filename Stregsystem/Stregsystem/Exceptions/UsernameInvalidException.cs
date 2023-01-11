namespace Stregsystem.Exceptions;

public class UsernameInvalidException : Exception
{
	public UsernameInvalidException() : base() { }

    public UsernameInvalidException(string? message) : base(message) { }
}
