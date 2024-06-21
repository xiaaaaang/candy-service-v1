using System.Text.RegularExpressions;

namespace Candy.Service.API.Commons;

public static class ValueChecker
{
    public static bool IsTelegramBotCommand(this string value, out string command)
    {
        if (!value.StartsWith("/"))
        {
            command = string.Empty;
            return false;
        }

        command = Regex.Match(value, "/[a-z0-9]*(?![a-z0-9])").Value;
        return true;
    }
}