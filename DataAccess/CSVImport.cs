using System;
using System.Linq;
using System.Text;
using Domain;
using System.IO;

namespace DataAccess
{
    public class CSVImport
    {
        private readonly Encoding _encoding;

        public CSVImport()
        {
            _encoding = Encoding.GetEncoding("iso-8859-1");
        }

        public void ImportOffers(string filepath)
        {
            try
            {
                var data = File.ReadAllLines(filepath, _encoding)
                    .Skip(1)
                    .Select(x => x.Split(';'))
                    .Select(x => new Offer(x[0], x[1], x[2], x[5], x[6], x[7]))
                    .Where(o => o.UserID != "" || o.OperationPrice != 0);
                ListContainer.Instance.Offers = data.ToList();
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (FormatException)
            {
                throw new FormatException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (Exception)
            {
                throw new Exception("Fejl, filerne blev ikke importeret");
            }
        }

        public void ImportRouteNumbers(string filepath)
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

        public void ImportContractors(string filepath)
        {
            try
            {
                var data = File.ReadAllLines(filepath, _encoding)
                    .Skip(1)
                    .Select(x => x.Split(';'))
                    .Select(x => new Contractor(x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7], x[8]))
                    .Where(c => c.UserId != "");
                ListContainer.Instance.ContractorList = data.ToList();
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (FormatException)
            {
                throw new FormatException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (Exception)
            {
                throw new Exception("Fejl, filerne blev ikke importeret");
            }
        }
    }
}