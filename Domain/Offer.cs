using System.Globalization;
using System.Threading;

namespace Domain
{
    public class Offer
    {
        public string OfferReferenceNumber { get; }
        public float OperationPrice { get; }
        public bool IsEligible { get; set; } = true;

        public int RequiredVehicleType => ListContainer.Instance.RouteNumberList.Find(r => r.RouteId == RouteID).RequiredVehicleType;

        public int RouteID { get; }
        public string UserID { get; }
        public float DifferenceToNextOffer { get; set; }
        public int RouteNumberPriority { get; }
        public int ContractorPriority { get; }
        public Contractor Contractor => ListContainer.Instance.ContractorList.Find(c => c.UserId == UserID);
        public bool Win { get; set; }

        public Offer(string offerReferenceNumber, string routeId, string operationPrice, string userId,
            string routeNumberPriority, string contractorPriority)
        {
            OfferReferenceNumber = offerReferenceNumber;
            RouteID = TryParseToIntElseZero(routeId);
            OperationPrice = TryParseToFloatElseZero(operationPrice);
            UserID = userId;
            RouteNumberPriority = TryParseToIntElseZero(routeNumberPriority);
            ContractorPriority = TryParseToIntElseZero(contractorPriority);
        }

        private int TryParseToIntElseZero(string str)
        {
            if (str == string.Empty)
            {
                return 0;
            }
            return int.Parse(str.Trim());
        }

        private float TryParseToFloatElseZero(string toParse)
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
            cultureInformation = new CultureInfo(CurrentCultureName);
            cultureInformation.NumberFormat.NumberDecimalSeparator = ",";
            return number;

        }
    }
}