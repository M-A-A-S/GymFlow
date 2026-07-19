using GymFlow.Domain.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class DayOfWeekExtensions
    {
        public static string ToLocalizedString(this DayOfWeek day, string language)
        {
            return language.ToLower() switch
            {
                "ar" => day switch
                {
                    DayOfWeek.Sunday => "الأحد",
                    DayOfWeek.Monday => "الاثنين",
                    DayOfWeek.Tuesday => "الثلاثاء",
                    DayOfWeek.Wednesday => "الأربعاء",
                    DayOfWeek.Thursday => "الخميس",
                    DayOfWeek.Friday => "الجمعة",
                    DayOfWeek.Saturday => "السبت",
                    _ => day.ToString()
                },

                _ => day.ToString() // English
            };
        }
    
        public static string ToLocalizedString(this DayOfWeek day)
        {
            return SharedResource.ResourceManager.GetString(day.ToString())
                ?? day.ToString();
        }
    }
}
