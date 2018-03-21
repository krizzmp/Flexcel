using System.Globalization;
using System.Threading;

namespace Domain
{
    public class Offer
    {
        private readonly float _operationPrice;
        private float Hours => ListContainer.Instance.RouteNumberList.Find(r => r.RouteId == RouteID).RequiredHours;
        public string OfferReferenceNumber { get; }

        public float OperationPrice => _operationPrice * Hours;
        
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
            RouteID = Utils.ParseToIntElseZero(routeId);
            _operationPrice = Utils.ParseToFloatElseZero(operationPrice);
            UserID = userId;
            RouteNumberPriority = Utils.ParseToIntElseZero(routeNumberPriority);
            ContractorPriority = Utils.ParseToIntElseZero(contractorPriority);
        }
    }
}