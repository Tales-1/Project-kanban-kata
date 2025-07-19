using System.Text;

namespace ProjectKanban.Extensions;

public static class StringExtensions
{
    public static string GetInitialsFromUsername(this string username)
    {
        if(string.IsNullOrEmpty(username))
            return string.Empty;

        // splitting and looping over names allows to account for users with multiple names
        string[] names = username.Split(" ");

        StringBuilder initialsBuilder = new ();

        foreach (string name in names)
        {
            initialsBuilder.Append(name[0]);
        }

        return initialsBuilder.ToString();
    }
}
