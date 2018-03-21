using System;
using System.IO;
using System.Linq;
using System.Text;
using Domain;

namespace DataAccess
{
    public class RouteNumberLoader : ILoader
    {
        private readonly Encoding _encoding;

        public RouteNumberLoader(Encoding encoding)
        {
            this._encoding = encoding;
        }

        public void Load(string filepath)
        {
            try
            {
                var routeNumbers = File.ReadAllLines(filepath, _encoding)
                    .Skip(1)
                    .Select(x => x.Split(';'))
                    .Select(RouteNumberFactory)
                    .Where(rn => rn.RouteId != 0 && rn.RequiredVehicleType != 0);
                ListContainer.Instance.RouteNumberList = routeNumbers.ToList();
            }


            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (FormatException)
            {
                throw new FormatException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
        }

        private RouteNumber RouteNumberFactory(string[] x)
        {
            const int weekdaysInWeek = 5;
            const int weeksInYear = 52;
            const int weekendDaysInWeek = 2;
            const int holydaysInYear = 6;

            string routeId = x.ElementAtOrDefault(0);
            string requiredVehicleType = x.ElementAtOrDefault(1);

            float hoursInWeekday = x.ElementAtOrDefault(2).ParseToFloatElseZero();
            float hoursInWeekend = x.ElementAtOrDefault(3).ParseToFloatElseZero();
            float hoursInHolyday = x.ElementAtOrDefault(4).ParseToFloatElseZero();
            
            float requiredHours = hoursInWeekday * weekdaysInWeek * weeksInYear + hoursInWeekend * weekendDaysInWeek * weeksInYear + hoursInHolyday * holydaysInYear;
            if (requiredHours.IsCloseToZero())
            {
                requiredHours = 1;
            }

            return new RouteNumber(routeId, requiredVehicleType, requiredHours);
        }
    }
}