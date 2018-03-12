using System;
using System.IO;
using System.Linq;
using System.Text;
using Domain;

namespace DataAccess
{
    public class OfferLoader
    {
        private readonly Encoding _encoding;

        public OfferLoader(Encoding encoding)
        {
            _encoding = encoding;
        }

        public void Load(string filepath)
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
    }
}