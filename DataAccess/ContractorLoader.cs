using System;
using System.Collections.Generic;
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
                    .Select((string x) => x.Split(';'))
                    .Select(x => {
                        var vehicleTypes = new Dictionary<int, int> {
                            {2, int.Parse(x[4].Trim())},
                            {3, int.Parse(x[5].Trim())},
                            {5, int.Parse(x[6].Trim())},
                            {6, int.Parse(x[7].Trim())},
                            {7, int.Parse(x[8].Trim())}
                        };

                        return new Contractor(x[0], x[1], x[2], x[3], vehicleTypes);
                    })
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