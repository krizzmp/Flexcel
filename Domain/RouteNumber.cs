using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class RouteNumber
    {
        public List<Offer> Offers => ListContainer.Instance.Offers.Where(offer => offer.RouteID == RouteId)
            .OrderBy(x => x.OperationPrice)
            .ThenBy(x => x.RouteNumberPriority).ToList();

        public int RouteId { get; set; }
        public int RequiredVehicleType { get; set; }

        public void CalculateDifference()
        {
            var eligibleOffers = Offers.Where(offer => offer.IsEligible).ToList();
            int count = eligibleOffers.Count();
            const float lastOptionValue = int.MaxValue;
            switch (count)
            {
                case 0:
                    throw new Exception("Der er ingen bud på garantivognsnummer " + RouteId);
                default:

                    var groupBy = eligibleOffers.GroupBy(o => o.OperationPrice).OrderBy(g => g.Key).ToList();
                    for (int i = 0; i < groupBy.Count(); i++)
                    {
                        IGrouping<float, Offer> grouping = groupBy[i];
                        foreach (Offer offer in grouping)
                        {
                            if (i == groupBy.Count() - 1) // if it is the last group
                            {
                                offer.DifferenceToNextOffer = lastOptionValue;
                            }
                            else
                            {
                                offer.DifferenceToNextOffer = groupBy[i + 1].Key - grouping.Key;
                            }
                        }
                    }

                    break;
            }
        }

        public void AssignWinner()
        {
            IEnumerable<Offer> eligibleOffers = Offers.Where(offer => offer.IsEligible).ToList();
            float min = eligibleOffers.Min(offer => offer.OperationPrice);
            IEnumerable<Offer> cheapestOffers =
                eligibleOffers.Where(offer => Math.Abs(offer.OperationPrice - min) < 0.001).ToList();
            foreach (Offer cheapestOffer in cheapestOffers)
            {
                cheapestOffer.Win = true;
            }
        }
    }
}