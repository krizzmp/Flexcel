using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Contractor
    {
        public string ReferenceNumberBasicInformationPdf { get; private set; }
        public string UserId { get; private set; }
        public string CompanyName { get; private set; }
        public string ManagerName { get; private set; }

        public int NumberOfType2PledgedVehicles { get; private set; }
        public int NumberOfType3PledgedVehicles { get; private set; }
        public int NumberOfType5PledgedVehicles { get; private set; }
        public int NumberOfType6PledgedVehicles { get; private set; }
        public int NumberOfType7PledgedVehicles { get; private set; }

        public int NumberOfWonType2Offers => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 2);
        public int NumberOfWonType3Offers => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 3);
        public int NumberOfWonType5Offers => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 5);
        public int NumberOfWonType6Offers => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 6);
        public int NumberOfWonType7Offers => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 7);

        public List<Offer> Offers =>
            ListContainer.Instance.Offers.Where(offer => offer.Contractor.UserId == UserId).ToList();

        public Contractor(string referenceNumberBasicInformationPdf, string managerName, string companyName,
            string userId, string type2Amount, string type3Amount,
            string type5Amount, string type6Amount,
            string type7Amount)
        {
            ReferenceNumberBasicInformationPdf = referenceNumberBasicInformationPdf;
            ManagerName = managerName;
            CompanyName = companyName;
            UserId = userId;
            NumberOfType2PledgedVehicles = int.Parse(type2Amount.Trim());
            NumberOfType3PledgedVehicles = int.Parse(type3Amount.Trim());
            NumberOfType5PledgedVehicles = int.Parse(type5Amount.Trim());
            NumberOfType6PledgedVehicles = int.Parse(type6Amount.Trim());
            NumberOfType7PledgedVehicles = int.Parse(type7Amount.Trim());
        }

        public bool MarkOvercommitedOffersIneligible()
        {
            var winningOffersByVehicleType = Offers.Where(offer => offer.Win && offer.IsEligible)
                .GroupBy(o => o.RequiredVehicleType);
            var didMarkIneligible = false;
            foreach (IGrouping<int, Offer> winningOffers in winningOffersByVehicleType)
            {
                var offers = winningOffers.Where(offer => offer.IsEligible);
                int numberOfPledgedVehicles = NumberOfPledgedVehiclesOfType(winningOffers.Key);
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

        private int NumberOfPledgedVehiclesOfType(int type)
        {
            switch (type)
            {
                case 2:
                    return NumberOfType2PledgedVehicles;
                case 3:
                    return NumberOfType3PledgedVehicles;
                case 5:
                    return NumberOfType5PledgedVehicles;
                case 6:
                    return NumberOfType6PledgedVehicles;
                case 7:
                    return NumberOfType7PledgedVehicles;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}