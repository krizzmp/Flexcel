using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Logic
{
    public class Selection
    {
        private readonly ListContainer _listContainer = ListContainer.Instance;

        public void CalculateOperationPriceDifferenceForOffers(List<RouteNumber> sortedRouteNumberList)
        {
            const int lastOptionValue = int.MaxValue;
            foreach (RouteNumber routeNumber in sortedRouteNumberList)
            {
                int numbersToCalc = routeNumber.offers.Count - 1;
                switch (routeNumber.offers.Count)
                {
                    case 0:
                        throw new Exception("Der er ingen bud på garantivognsnummer " + routeNumber.RouteID);
                    case 1:
                        routeNumber.offers[0].DifferenceToNextOffer = lastOptionValue;
                        break;
                    case 2:
                        if (Math.Abs(routeNumber.offers[0].OperationPrice - routeNumber.offers[1].OperationPrice) < 0.0001)
                        {
                            routeNumber.offers[0].DifferenceToNextOffer = lastOptionValue;
                            routeNumber.offers[1].DifferenceToNextOffer = lastOptionValue;
                        }
                        else
                        {
                            routeNumber.offers[0].DifferenceToNextOffer = routeNumber.offers[1].OperationPrice - routeNumber.offers[0].OperationPrice;
                            routeNumber.offers[1].DifferenceToNextOffer = lastOptionValue;
                        }

                        break;
                    default:
                        for (int i = 0; i < numbersToCalc; i++)
                        {
                            float difference = 0;
                            int j = i + 1;
                            if (Math.Abs(routeNumber.offers[i].OperationPrice - routeNumber.offers[numbersToCalc].OperationPrice) > 0.0001)
                            {
                                while (Math.Abs(difference) < 0.00001 && j <= numbersToCalc)
                                {
                                    difference = routeNumber.offers[j].OperationPrice - routeNumber.offers[i].OperationPrice;
                                    j++;
                                }
                            }
                            else
                            {
                                while (i < numbersToCalc)
                                {
                                    routeNumber.offers[i].DifferenceToNextOffer = lastOptionValue;
                                    i++;
                                }
                            }
                            routeNumber.offers[i].DifferenceToNextOffer = difference;
                        }
                        routeNumber.offers[numbersToCalc].DifferenceToNextOffer = lastOptionValue;
                        break;
                }
            }
        }
        public void CheckIfContractorHasWonTooManyRouteNumbers(List<Offer> offersToCheck)
        {
            List<Contractor> contractorsToCheck = GetContractorsToCheck(offersToCheck);
            foreach (Contractor contractor in contractorsToCheck)
            {
                List<Offer> offers = contractor.GetOffersWithInsufficientVehicles();
                if (offers.Count > 0)
                {
                    foreach (Offer offer in offers)
                    {
                        _listContainer.ConflictList.Add(offer);
                    }
                    throw new Exception("Denne entreprenør har vundet flere garantivognsnumre, end de har biler til.  Der kan ikke vælges imellem dem, da de har samme prisforskel ned til næste bud. Prioriter venligst buddene i den relevante fil i kolonnen Entreprenør Prioritet");
                }
            }
        }

        private List<Contractor> GetContractorsToCheck(List<Offer> offersToCheck)
        {
            List<Contractor> contractorsToCheck = new List<Contractor>();
            foreach (Offer offer in offersToCheck)
            {
                foreach (Contractor contractor in _listContainer.ContractorList)
                {
                    if (contractor.UserId.Equals(offer.UserID))
                    {
                        bool alreadyOnList = contractorsToCheck.Any(obj => obj.UserId.Equals(contractor.UserId));
                        if (!alreadyOnList)
                        {
                            contractorsToCheck.Add(contractor);
                        }
                    }
                }
            }

            return contractorsToCheck;
        }

        public List<Offer> FindWinner(RouteNumber routeNumber)
        {
            List<Offer> winningOffers = new List<Offer>();
            List<Offer> listOfOffersWithLowestPrice = new List<Offer>();
            int lengthOfOffers = routeNumber.offers.Count();
            float lowestEligibleOperationPrice = 0;
            bool cheapestNotFound = true;

            for (int i = 0; i < lengthOfOffers; i++)
            {
                if (routeNumber.offers[i].IsEligible && cheapestNotFound)
                {
                    lowestEligibleOperationPrice = routeNumber.offers[i].OperationPrice;
                    cheapestNotFound = false;
                }
            }
            foreach (Offer offer in routeNumber.offers)
            {
                if (offer.IsEligible && offer.OperationPrice == lowestEligibleOperationPrice)
                {
                    listOfOffersWithLowestPrice.Add(offer);
                }
            }

            int count = 0;
            foreach (Offer offer in listOfOffersWithLowestPrice) // Checking if offers with same price are prioritized
            {
                if (offer.RouteNumberPriority != 0)
                {
                    count++;
                }
            }
            if (count != 0) //if routenumberpriority found 

            {
                List<Offer> listOfPriotizedOffers = GetListOfPriotizedOffers(listOfOffersWithLowestPrice);
                winningOffers.Add(listOfPriotizedOffers.First());
            }
            else
            {
                foreach (Offer offer in listOfOffersWithLowestPrice)
                {
                    winningOffers.Add(offer);
                }
            }
            return winningOffers;
        }

        private static List<Offer> GetListOfPriotizedOffers(List<Offer> listOfOffersWithLowestPrice)
        {
            List<Offer> listOfPriotizedOffers = new List<Offer>();
            foreach (Offer offer in listOfOffersWithLowestPrice)
            {
                if (offer.RouteNumberPriority > 0)
                {
                    listOfPriotizedOffers.Add(offer);
                }
            }

            listOfPriotizedOffers = listOfPriotizedOffers.OrderBy(x => x.RouteNumberPriority).ToList();
            return listOfPriotizedOffers;
        }

        public List<Offer> AssignWinners(List<Offer> offersToAssign)
        {
            List<Contractor> contractorsToCheck = new List<Contractor>();
            List<Offer> ineligibleOffersAllContractors = new List<Offer>();

            foreach (Offer offer in offersToAssign)
            {
                if (offer.IsEligible)
                {
                    _listContainer.ContractorList.Find(x => x.UserId == offer.UserID).AddWonOffer(offer);
                    contractorsToCheck.Add(offer.Contractor);
                }
            }

            foreach (Contractor contractor in contractorsToCheck)
            {
                contractor.GetOffersWithInsufficientVehicles();
                List<Offer> ineligibleOffersOneContractor = contractor.ReturnIneligibleOffers();
                ineligibleOffersAllContractors.AddRange(ineligibleOffersOneContractor);
                contractor.RemoveIneligibleOffersFromWinningOffers();
            }
            
            return ineligibleOffersAllContractors;
        }
        public void CheckForMultipleWinnersForEachRouteNumber(List<Offer> winnerList)
        {
            int length = winnerList.Count;
            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (winnerList[i].RouteID == winnerList[j].RouteID)
                    {
                        foreach (Offer offer in winnerList)
                        {
                            if (offer.RouteID == winnerList[i].RouteID)
                            {
                                _listContainer.ConflictList.Add(offer);
                            }
                        }
                        throw new Exception("Dette garantivognsnummer har flere mulige vindere. Der kan ikke vælges mellem dem, da de har samme prisforskel ned til næste bud. Prioriter venligst buddene i den relevante fil i kolonnen Garantivognsnummer Prioritet.");
                    }
                }
            }
        }
    }
}
