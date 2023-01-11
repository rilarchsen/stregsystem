namespace Stregsystem.Extensions;

public static class StringExtensions
{
    public static bool IsAlphaNumeric(this string str, bool includeCaps = true)
    {
        return IsAlphaNumericPlus(str, includeCaps);
    }

    public static bool IsAlphaNumericPlus(this string str, bool includeCaps = true, params char[] additionalChars)
    {
        char[] charArray = str.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            if (!char.IsLetterOrDigit(charArray[i]) && !additionalChars.Contains(charArray[i]))
                return false;
            else if (includeCaps == false && char.IsLetter(charArray[i]) && char.IsUpper(charArray[i]))
                return false;
        }
        return true;
    }
}
