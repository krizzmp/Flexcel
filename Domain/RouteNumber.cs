using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class RouteNumber
    {
        public RouteNumber(string routeId, string requiredVehicleType)
        {
            RouteId = int.Parse(routeId.Trim());
            RequiredVehicleType = int.Parse(requiredVehicleType.Trim());
        }

        public List<Offer> Offers => ListContainer.Instance.Offers.Where(offer => offer.RouteID == RouteId)
            .OrderBy(x => x.OperationPrice)
            .ThenBy(x => x.RouteNumberPriority).ToList();

        public int RouteId { get; private set; }
        public int RequiredVehicleType { get; private set; }

        public void CalculateDifference()
        {
            var eligibleOffers = Offers.Where(offer => offer.IsEligible).ToList();
            int count = eligibleOffers.Count();
            const float lastOptionValue = int.MaxValue;
            if (count == 0)
            {
                throw new Exception("Der er ingen bud på garantivognsnummer " + RouteId);
            }
            else
            {
                // we group by opertion price
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

        public void FailForMultipleWinners()
        {
            IEnumerable<Offer> winningOffers = Offers.Where(offer => offer.IsEligible && offer.Win).ToList();
            if (winningOffers.Count() > 1)
            {
                var maxAll = winningOffers.MaxAll().ToList();
                foreach (Offer offer in winningOffers)
                {
                    offer.IsEligible = false;
                }

                if (maxAll.Count() == 1)
                {
                    maxAll.First().IsEligible = true;
                }
                else
                {
                    throw new ApplicationException("multiple winners");
                }
            }
        }
    }

    static class Extra
    {
        public static IEnumerable<Offer> MaxAll(this IEnumerable<Offer> enumerable)
        {
            var offers = enumerable.ToList();
            return offers.Where(x => x.RouteNumberPriority == offers.Max(arg => arg.RouteNumberPriority));
        }
    }
}