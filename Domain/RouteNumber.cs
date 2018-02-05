using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class RouteNumber
    {
        public List<Offer> Offers { get; set; } = new List<Offer>();
        public int RouteId { get; set; }
        public int RequiredVehicleType { get; set; }
        
        public void CalculateDifference()
        {
            var eligibleOffers = Offers.Where(offer => offer.IsEligible);
            int count = eligibleOffers.Count();
            const float lastOptionValue = int.MaxValue;
            switch (count)
            {
                case 0:
                    throw new Exception("Der er ingen bud på garantivognsnummer " + RouteId);
                case 1:
                    Offers[0].DifferenceToNextOffer = lastOptionValue;
                    break;
                case 2:
                    if (Offers[0].OperationPrice == Offers[1].OperationPrice)
                    {
                        Offers[0].DifferenceToNextOffer = lastOptionValue;
                        Offers[1].DifferenceToNextOffer = lastOptionValue;
                    }
                    else
                    {
                        Offers[0].DifferenceToNextOffer = Offers[1].OperationPrice - Offers[0].OperationPrice;
                        Offers[1].DifferenceToNextOffer = lastOptionValue;
                    }

                    break;
                default:
                    var numbersToCalc = count - 1;
                    for (int i = 0; i < numbersToCalc; i++)
                    {
                        float difference = 0;
                        int j = i + 1;
                        if (Offers[i].OperationPrice == Offers[numbersToCalc].OperationPrice)
                        {
                            while (difference == 0 && j <= numbersToCalc)
                            {
                                difference = Offers[j].OperationPrice - Offers[i].OperationPrice;
                                j++;
                            }
                        }
                        else
                        {
                            while (i < numbersToCalc)
                            {
                                Offers[i].DifferenceToNextOffer = lastOptionValue;
                                i++;
                            }
                        }
                        Offers[i].DifferenceToNextOffer = difference;
                    }
                    Offers[numbersToCalc].DifferenceToNextOffer = lastOptionValue;
                    break;
            }
        }
        public void AssignWinner()
        {
            IEnumerable<Offer> eligibleOffers = Offers.Where(offer => offer.IsEligible).ToList();
            float min = eligibleOffers.Min(offer => offer.OperationPrice);
            IEnumerable<Offer> cheapestOffers = eligibleOffers.Where(offer => Math.Abs(offer.OperationPrice - min) < 0.001).ToList();
            foreach (Offer cheapestOffer in cheapestOffers)
            {
                cheapestOffer.Win = true;
            }
        }
    }
}
