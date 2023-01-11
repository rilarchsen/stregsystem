namespace Stregsystem.Exceptions;

public class UserEmailInvalidException : Exception
{
	public UserEmailInvalidException() : base() { }

    public UserEmailInvalidException(string? message) : base(message) { }
}
