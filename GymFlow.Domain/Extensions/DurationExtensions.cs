using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class DurationExtensions
    {
        public static string ToLocalizedHours(this double hours)
        {
            // Get the current language.
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            // We use the integer part for grammar rules.
            int value = (int)hours;

            if (culture == "ar")
            {
                // 1 hour
                if (value == 1)
                {
                    return $"{hours:0.##} " +
                        (SharedResource.ResourceManager.GetString("Hour_One")
                        ?? "ساعة");
                }

                // 2 hours
                if (value == 2)
                {
                    return $"{hours:0.##} " +
                        (SharedResource.ResourceManager.GetString("Hour_Two")
                        ?? "ساعتان");
                }

                // 3 - 10 hours
                if (value >= 3 && value <= 10)
                {
                    return $"{hours:0.##} " +
                        (SharedResource.ResourceManager.GetString("Hour_Few")
                        ?? "ساعات");
                }

                // 11+ hours
                return $"{hours:0.##} " +
                    (SharedResource.ResourceManager.GetString("Hour_Many")
                    ?? "ساعة");
            }

            // English
            if (value == 1)
            {
                return $"{hours:0.##} " +
                    (SharedResource.ResourceManager.GetString("Hour_One")
                    ?? "hour");
            }

            return $"{hours:0.##} " +
                (SharedResource.ResourceManager.GetString("Hour_Other")
                ?? "hours");

        }

    }
}
