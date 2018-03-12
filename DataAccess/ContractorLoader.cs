using System;
using System.IO;
using System.Linq;
using System.Text;
using Domain;

namespace DataAccess
{
    public class ContractorLoader : ILoader
    {
        private readonly Encoding _encoding;

        public ContractorLoader(Encoding encoding)
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