using System;

namespace IRG
{
    public static class FlagsExtension
    {
        public static bool HasAnyFlag(this Enum flags, Enum flagToCheck)
        {
            return (Convert.ToInt64(flags) & Convert.ToInt64(flagToCheck)) != 0;
        }

        public static bool HasAnyFlagOrDefault(this Enum flags, Enum flagToCheck)
        {
            return Convert.ToInt64(flags) == 0 || Convert.ToInt64(flagToCheck) == 0 ||
                   (Convert.ToInt64(flags) & Convert.ToInt64(flagToCheck)) != 0;
        }
    }

}