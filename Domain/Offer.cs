namespace Domain
{
    public class Offer
    {
        public string OfferReferenceNumber { get; set; }
        public float OperationPrice { get; set; }
        public bool IsEligible { get; set; } = true;

        public int RequiredVehicleType =>
            ListContainer.Instance.RouteNumberList.Find(r => r.RouteId == RouteID).RequiredVehicleType;

        public int RouteID { get; set; }
        public string UserID { get; set; }
        public float DifferenceToNextOffer { get; set; }
        public int RouteNumberPriority { get; set; }
        public int ContractorPriority { get; set; }
        public Contractor Contractor => ListContainer.Instance.ContractorList.Find(c => c.UserId == UserID);
        public bool Win { get; set; }
    }
}