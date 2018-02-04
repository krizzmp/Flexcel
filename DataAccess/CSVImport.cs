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
                        OperationPrice = TryParseToFloatElseZero((x[2])),
                        UserID = x[5],
                        CreateRouteNumberPriority = x[6],
                        CreateContractorPriority = x[7],
                    });
                foreach (var o in data)
                {
                    if (o.UserID != "" || o.OperationPrice != 0)
                    {
                        o.RouteNumberPriority = TryParseToIntElseZero(o.CreateRouteNumberPriority);
                        o.ContractorPriority = TryParseToIntElseZero(o.CreateContractorPriority);
                        Contractor contractor = ListOfContractors.Find(x => x.UserId == o.UserID);
                        try
                        {
                            o.RequiredVehicleType = (ListOfRouteNumbers.Find(r => r.RouteID == o.RouteID))
                                .RequiredVehicleType;
                            Offer newOffer = new Offer(o.OfferReferenceNumber, o.OperationPrice, o.RouteID, o.UserID,
                                o.RouteNumberPriority, o.ContractorPriority, contractor, o.RequiredVehicleType);
                            ListOfOffers.Add(newOffer);
                        }
                        catch
                        {
                            // Help for debugging purpose only.
                            string failure = o.RouteID.ToString();
                        }
                    }
                }

                foreach (RouteNumber routeNumber in ListOfRouteNumbers)
                {
                    foreach (Offer offer in ListOfOffers)
                    {
                        if (offer.RouteID == routeNumber.RouteID)
                        {
                            routeNumber.offers.Add(offer);
                        }
                    }
                }
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

        public void ImportRouteNumbers(string routeNumbersFilepath)
        {
            try
            {
                string filepath = routeNumbersFilepath;
                var data = File.ReadAllLines(filepath, _encoding)
                    .Skip(1)
                    .Select(x => x.Split(';'))
                    .Select(x => new RouteNumber
                        {
                            RouteID = TryParseToIntElseZero(x[0]),
                            RequiredVehicleType = TryParseToIntElseZero(x[1]),
                        }
                    );
                foreach (var r in data)
                {
                    bool doesAlreadyContain = ListOfRouteNumbers.Any(obj => obj.RouteID == r.RouteID);

                    if (!doesAlreadyContain && r.RouteID != 0 && r.RequiredVehicleType != 0)
                    {
                        ListOfRouteNumbers.Add(r);
                    }
                }
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
                    .Select(x => new Contractor
                        {
                            ReferenceNumberBasicInformationPdf = x[0],
                            ManagerName = x[1],
                            CompanyName = x[2],
                            UserId = x[3],
                            NumberOfType2PledgedVehicles = TryParseToIntElseZero(x[4]),
                            NumberOfType3PledgedVehicles = TryParseToIntElseZero(x[5]),
                            NumberOfType5PledgedVehicles = TryParseToIntElseZero(x[6]),
                            NumberOfType6PledgedVehicles = TryParseToIntElseZero(x[7]),
                            NumberOfType7PledgedVehicles = TryParseToIntElseZero(x[8])
                        }
                    ).Where(c => c.UserId != "");
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