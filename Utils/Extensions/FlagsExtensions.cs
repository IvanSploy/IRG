using System;

public static class FlagsExtension
{
    public static bool HasAnyFlag(this Enum flags, Enum flagToCheck, bool strict = false)
    {
        return (!strict && (Convert.ToInt32(flags) == 0 || Convert.ToInt32(flagToCheck) == 0)) ||
               (Convert.ToInt64(flags) & Convert.ToInt64(flagToCheck)) != 0;
    }
}
