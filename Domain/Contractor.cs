using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Contractor
    {
        public string ReferenceNumberBasicInformationPdf { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string ManagerName { get; set; }

        public int NumberOfType2PledgedVehicles { get; set; }
        public int NumberOfType3PledgedVehicles { get; set; }
        public int NumberOfType5PledgedVehicles { get; set; }
        public int NumberOfType6PledgedVehicles { get; set; }
        public int NumberOfType7PledgedVehicles { get; set; }

        public int NumberOfWonType2Offers
        {
            get => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 2);
        }

        public int NumberOfWonType3Offers
        {
            get => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 3);
        }

        public int NumberOfWonType5Offers
        {
            get => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 5);
        }

        public int NumberOfWonType6Offers
        {
            get => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 6);
        }

        public int NumberOfWonType7Offers
        {
            get => Offers.Count(o => o.Win && o.IsEligible && o.RequiredVehicleType == 7);
        }

        public List<Offer> Offers
        {
            get => ListContainer.Instance.Offers.Where(offer => offer.Contractor.UserId == UserId).ToList();
        }


        public bool MarkOvercommitedOffersIneligible()
        {
            var winningOffers = Offers.Where(offer => offer.Win && offer.IsEligible);
            var groupBy = winningOffers.GroupBy(o => o.RequiredVehicleType);
            var didMarkIneligible = false;
            foreach (IGrouping<int, Offer> grouping in groupBy)
            {
                //var offers = grouping.Where(offer => offer.IsEligible);
                for (var offers = grouping.Where(offer => offer.IsEligible);
                    offers.Count() > NumberOfPledgedVehiclesOfType(grouping.Key);
                    offers = grouping.Where(offer => offer.IsEligible))
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