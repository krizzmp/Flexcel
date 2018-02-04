using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Contractor
    {
        public List<Offer> WinningOffers { get; private set; } = new List<Offer>();
        public string ReferenceNumberBasicInformationPdf { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string ManagerName { get; set; }
        public int NumberOfType2PledgedVehicles { get; set; }
        public int NumberOfType3PledgedVehicles { get; set; }
        public int NumberOfType5PledgedVehicles { get; set; }
        public int NumberOfType6PledgedVehicles { get; set; }
        public int NumberOfType7PledgedVehicles { get; set; }
        public int NumberOfWonType2Offers { get; private set; }
        public int NumberOfWonType3Offers { get; private set; }
        public int NumberOfWonType5Offers { get; private set; }
        public int NumberOfWonType6Offers { get; private set; }
        public int NumberOfWonType7Offers { get; private set; }

        public void AddWonOffer(Offer offer)
        {
            bool alreadyOnTheList = WinningOffers.Any(item => item.OfferReferenceNumber == offer.OfferReferenceNumber);
            if (!alreadyOnTheList)
            {
                WinningOffers.Add(offer);
            }
            else
            {
                foreach (Offer winOffer in WinningOffers)
                {
                    if (winOffer.OfferReferenceNumber == offer.OfferReferenceNumber)
                    {
                        winOffer.IsEligible = true;
                    }
                }
            }
        }

        public List<Offer> ReturnIneligibleOffers()
        {
            return WinningOffers.Where(offer => !offer.IsEligible).ToList();
        }

        public void RemoveIneligibleOffersFromWinningOffers()
        {
            WinningOffers.RemoveAll(offer => !offer.IsEligible);
        }

        public List<Offer> GetOffersWithInsufficientVehicles()
        {
            List<Offer> offersWithConflict = new List<Offer>();
            int type2 = 0;
            int type3 = 0;
            int type5 = 0;
            int type6 = 0;
            int type7 = 0;
            if (WinningOffers.Count > 0)
            {
                foreach (Offer offer in WinningOffers)
                {
                    if (offer.IsEligible)
                    {
                        if (offer.RequiredVehicleType == 2)
                        {
                            type2++;
                        }

                        if (offer.RequiredVehicleType == 3)
                        {
                            type3++;
                        }

                        if (offer.RequiredVehicleType == 5)
                        {
                            type5++;
                        }

                        if (offer.RequiredVehicleType == 6)
                        {
                            type6++;
                        }

                        if (offer.RequiredVehicleType == 7)
                        {
                            type7++;
                        }
                    }
                }
            }

            if (WinningOffers.Count > 0)
            {
                if (NumberOfType2PledgedVehicles == 0 && NumberOfType3PledgedVehicles == 0 &&
                    NumberOfType5PledgedVehicles == 0 && NumberOfType6PledgedVehicles == 0 &&
                    NumberOfType7PledgedVehicles == 0)
                {
                    //If all pledged vehicles is 0, it means they have unlimited amount of vehicles available
                }
                else
                {
                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType2PledgedVehicles, type2, 2))
                    {
                        offersWithConflict.Add(ofr);
                    }

                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType3PledgedVehicles, type3, 3))
                    {
                        offersWithConflict.Add(ofr);
                    }

                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType5PledgedVehicles, type5, 5))
                    {
                        offersWithConflict.Add(ofr);
                    }

                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType6PledgedVehicles, type6, 6))
                    {
                        offersWithConflict.Add(ofr);
                    }

                    foreach (Offer ofr in IfTooManyWonOffers(NumberOfType7PledgedVehicles, type7, 7))
                    {
                        offersWithConflict.Add(ofr);
                    }
                }
            }

            return offersWithConflict;
        }

        private List<Offer> IfTooManyWonOffers(int numberOfPledgedVehicles, int numberOfWonOffersWithThisType, int type)
        {
            List<Offer> offersToCheck = new List<Offer>();
            List<Offer> listOfOffersToReturn = new List<Offer>();
            foreach (Offer winningOffer in WinningOffers)
            {
                if (winningOffer.IsEligible && winningOffer.RequiredVehicleType == type)
                {
                    offersToCheck.Add(winningOffer);
                }
            }


            if (numberOfPledgedVehicles < numberOfWonOffersWithThisType)
            {
                if (numberOfPledgedVehicles == 0
                ) //This is done because, sometimes contractors place bids on routenumbers, they don't have the correct vehicle type for. 
                {
                    foreach (Offer offer in WinningOffers)
                    {
                        if (offer.RequiredVehicleType == type)
                        {
                            offer.IsEligible = false;
                        }
                    }
                }
                else
                {
                    listOfOffersToReturn = FindOptimalWins(offersToCheck, numberOfPledgedVehicles);
                }
            }

            return listOfOffersToReturn;
        }

        private List<Offer> FindOptimalWins(List<Offer> offersToCheck, int numberOfPledgedVehicles)
        {
            List<Offer> offersWithConflict = new List<Offer>();
            List<Offer> offersToChooseFrom = offersToCheck
                .OrderByDescending(x => x.DifferenceToNextOffer)
                .ThenBy(x => x.ContractorPriority)
                .ToList();

            foreach (Offer offer in offersToChooseFrom)
            {
                offer.IsEligible = offer.DifferenceToNextOffer >=
                                   offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer;
            }

            int eligibleOffers = offersToChooseFrom.Count(offer => offer.IsEligible);

            if (eligibleOffers > numberOfPledgedVehicles)
            {
                if (offersToChooseFrom[numberOfPledgedVehicles - 1].ContractorPriority !=
                    offersToChooseFrom[numberOfPledgedVehicles].ContractorPriority)
                {
                    int length = offersToCheck.Count;

                    for (int i = numberOfPledgedVehicles; i < length; i++)
                    {
                        if (offersToChooseFrom[i].DifferenceToNextOffer ==
                            offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer)
                        {
                            offersToChooseFrom[i].IsEligible = false;
                        }
                    }
                }
                else
                {
                    foreach (Offer offer in offersToChooseFrom)
                    {
                        if (offer.DifferenceToNextOffer ==
                            offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer && offer.IsEligible)
                        {
                            offersWithConflict.Add(offer);
                        }
                    }

                    if (offersWithConflict.Count == 1)
                    {
                        offersWithConflict.Clear();
                    }
                }
            }

            return offersWithConflict;
        }

        public void CountNumberOfWonOffersOfEachType(List<Offer> outPutList)
        {
            NumberOfWonType2Offers = 0;
            NumberOfWonType3Offers = 0;
            NumberOfWonType5Offers = 0;
            NumberOfWonType6Offers = 0;
            NumberOfWonType7Offers = 0;


            foreach (Offer offer in outPutList.Where(offer => offer.UserID == UserId))
            {
                switch (offer.RequiredVehicleType)
                {
                    case 2:
                        NumberOfWonType2Offers++;
                        break;
                    case 3:
                        NumberOfWonType3Offers++;
                        break;
                    case 5:
                        NumberOfWonType5Offers++;
                        break;
                    case 6:
                        NumberOfWonType6Offers++;
                        break;
                    case 7:
                        NumberOfWonType7Offers++;
                        break;
                }
            }
        }
    }
}