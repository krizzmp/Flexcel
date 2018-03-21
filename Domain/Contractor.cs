using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Contractor
    {
        public Dictionary<int, int> VehicleTypes { get; private set; }
        public string ReferenceNumberBasicInformationPdf { get; private set; }
        public string UserId { get; private set; }
        public string CompanyName { get; private set; }
        public string ManagerName { get; private set; }

        public List<Offer> Offers =>
            ListContainer.Instance.Offers.Where(offer => offer.Contractor.UserId == UserId).ToList();

        public Contractor(
            string referenceNumberBasicInformationPdf,
            string managerName,
            string companyName,
            string userId,
            Dictionary<int, int> vehicleTypes)
        {
            VehicleTypes = vehicleTypes;
            ReferenceNumberBasicInformationPdf = referenceNumberBasicInformationPdf;
            ManagerName = managerName;
            CompanyName = companyName;
            UserId = userId;
        }

        public bool MarkOvercommitedOffersIneligible()
        {
            var winningOffersByVehicleType = Offers.Where(offer => offer.Win && offer.IsEligible)
                .GroupBy(o => o.RequiredVehicleType);
            var didMarkIneligible = false;
            foreach (IGrouping<int, Offer> winningOffers in winningOffersByVehicleType)
            {
                var offers = winningOffers.Where(offer => offer.IsEligible);
                int numberOfPledgedVehicles = VehicleTypes[winningOffers.Key];
                while (offers.Count() > numberOfPledgedVehicles)
                {
                    if (MarkOfferWithSmallestDifferenceToNextOfferIneligible(offers))
                    {
                        didMarkIneligible = true;
                    }
                }
            }

            return didMarkIneligible;
        }

        public int NumberOfWonOffersOfType(int i) =>
            Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == i);

        private bool MarkOfferWithSmallestDifferenceToNextOfferIneligible(IEnumerable<Offer> offers)
        {
            Offer first = offers.OrderBy(offer => offer.DifferenceToNextOffer).FirstOrDefault();
            if (first != null)
            {
                first.IsEligible = false;
                return true;
            }

            return false;
        }
    }
}