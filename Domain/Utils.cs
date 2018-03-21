using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Domain
{
    public static class Utils
    {
        public static int ParseToIntElseZero(this string str)
        {

            int.TryParse(str.Trim(), out int n);
            return n;
        }

        public static float ParseToFloatElseZero(this string toParse)
        {
            if (toParse == null)
            {
                return 0;
            }

            string currentCultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo cultureInformation = new CultureInfo(currentCultureName);
            cultureInformation.NumberFormat.NumberDecimalSeparator = ",";
            toParse = toParse.Replace(" ", "");
            float.TryParse(toParse.Replace('.', ','), NumberStyles.Float, cultureInformation, out float n);
            return n;
        }
        public static bool IsCloseToZero(this float requiredHours)
        {
            return Math.Abs(requiredHours) < 0.001;
        }
        public static IEnumerable<Offer> MaxAll(this IEnumerable<Offer> enumerable)
        {
            var offers = enumerable.ToList();
            return offers.Where(x => x.RouteNumberPriority == offers.Max(arg => arg.RouteNumberPriority));
        }
    }
}