using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Domain;
using System.IO;
using System.Globalization;

namespace DataAccess
{
    public class CSVImport
    {
        private readonly Encoding _encoding;
        public readonly List<Contractor> ListOfContractors;
        public readonly List<RouteNumber> ListOfRouteNumbers;
        public readonly List<Offer> ListOfOffers;

        public CSVImport()
        {
            ListOfContractors = new List<Contractor>();
            ListOfRouteNumbers = new List<RouteNumber>();
            ListOfOffers = new List<Offer>();
            _encoding = Encoding.GetEncoding("iso-8859-1");
        }

        public int TryParseToIntElseZero(string toParse)
        {
            toParse = toParse.Replace(" ", "");
            bool tryParse = Int32.TryParse(toParse, out int number);
            return number;
        }

        public float TryParseToFloatElseZero(string toParse)
        {
            string CurrentCultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo cultureInformation = new CultureInfo(CurrentCultureName);
            if (cultureInformation.NumberFormat.NumberDecimalSeparator != ",")
                // Forcing use of decimal separator for numerical values
            {
                cultureInformation.NumberFormat.NumberDecimalSeparator = ",";
                Thread.CurrentThread.CurrentCulture = cultureInformation;
            }

            float number;
            toParse = toParse.Replace(" ", "");
            bool tryParse = float.TryParse(toParse.Replace('.', ','), out number);
            return number;
        }

        public void ImportOffers(string filepath)
        {
            try
            {
                var data = File.ReadAllLines(filepath, _encoding)
                    .Skip(1)
                    .Select(x => x.Split(';'))
                    .Select(x => new Offer
                    {
                        OfferReferenceNumber = x[0],
                        RouteID = TryParseToIntElseZero(x[1]),
                        OperationPrice = TryParseToFloatElseZero(x[2]),
                        UserID = x[5],
                        RouteNumberPriority = TryParseToIntElseZero(x[6]),
                        ContractorPriority = TryParseToIntElseZero(x[7]),
                    }).Where(o => o.UserID != "" || o.OperationPrice != 0);
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
                    .Select(x => new RouteNumber
                        {
                            RouteId = TryParseToIntElseZero(x[0]),
                            RequiredVehicleType = TryParseToIntElseZero(x[1]),
                        }
                    )
                    .Where(rn => rn.RouteId != 0 && rn.RequiredVehicleType != 0);
                ListOfRouteNumbers.AddRange(data);

                //foreach (var r in data)
                //{
                //    bool doesAlreadyContain = ListOfRouteNumbers.Any(obj => obj.RouteId == r.RouteId);

                //    if (!doesAlreadyContain && r.RouteId != 0 && r.RequiredVehicleType != 0)
                //    {
                //        ListOfRouteNumbers.Add(r);
                //    }
                //}
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
                ListOfContractors.AddRange(data);
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

        public List<Contractor> GetListOfContractors()
        {
            return ListOfContractors;
        }

        public List<RouteNumber> GetListOfRouteNumbers()
        {
            return ListOfRouteNumbers;
        }
    }
}