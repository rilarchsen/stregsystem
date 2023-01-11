using Stregsystem.Exceptions;
using Stregsystem.Extensions;

namespace Stregsystem.Models;

public class User : IComparable<User>
{
    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; private set; }
    public string Email { get; set; }
    public double Balance { get; set; }

    public User(int id, string firstName, string lastName, string username, string email, double balance = 0.0)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = ValidateAndReturnUsername(username);
        Email = ValidateAndReturnEmail(email);
        Balance = balance;
    }

    public int CompareTo(User? other) => Id.CompareTo(other?.Id);

    public override bool Equals(object? obj) => obj is User other && Id == other.Id && FirstName == other.FirstName && LastName == other.LastName && Email == other.Email && Balance == other.Balance;

    public override string ToString() => $"{FirstName} {LastName} ({Email})";

    public override int GetHashCode() => Id.GetHashCode();

    private string ValidateAndReturnEmail(string email)
    {
        string invalidFormatErrorMessage = "Invalid email address. Emails must be of the following format: name@domain.com";
        string invalidCharactersErrorMessage = "Invalid email address. Emails may only contain alphanumeric characters (a-z, A-Z, 0-9), periods (.), underscores (_), and hyphens (-)";
        char[] specialChars = { '.', '_', '-' };

        if (!email.Contains('@'))
            throw new UserEmailInvalidException(invalidFormatErrorMessage);

        string[] parts = email.Split('@');

        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1]))
            throw new UserEmailInvalidException(invalidFormatErrorMessage);

        if (!parts[0].IsAlphaNumericPlus(true, specialChars) || !parts[1].IsAlphaNumericPlus(true, specialChars))
            throw new UserEmailInvalidException(invalidCharactersErrorMessage);

        return email;
    }

    private string ValidateAndReturnUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new UsernameInvalidException("Invalid username. Username cannot be blank");

        if (!username.IsAlphaNumericPlus(false, '_'))
            throw new UsernameInvalidException("Invalid username. Usernames can contain lowercase alphanumeric characters (a-z, 0-9) and underscores (_)");

        return username;
    }
}
