using System;
using System.IO;
using System.Linq;
using System.Text;
using Domain;

namespace DataAccess
{
    public class RouteNumberLoader: ILoader
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
                var data = File.ReadAllLines(filepath, _encoding)
                    .Skip(1)
                    .Select(x => x.Split(';'))
                    .Select(x => new RouteNumber(x[0], x[1]))
                    .Where(rn => rn.RouteId != 0 && rn.RequiredVehicleType != 0);
                ListContainer.Instance.RouteNumberList = data.ToList();
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
    }
}