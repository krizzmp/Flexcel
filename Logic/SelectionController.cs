using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Logic
{
    public class SelectionController
    {
        private List<RouteNumber> _routeNumberList;
        private readonly Selection _selection;
        private List<RouteNumber> _sortedRouteNumberList;
        private readonly ListContainer _listContainer = ListContainer.Instance;

        public SelectionController()
        {
            _routeNumberList = new List<RouteNumber>();
            _selection = new Selection();
        }

        private void SortRouteNumberList(List<RouteNumber> routeNumberList)
        {
            _sortedRouteNumberList = routeNumberList.OrderBy(x => x.RouteID).ToList();
            foreach (RouteNumber routeNumber in _sortedRouteNumberList)
            {
                routeNumber.offers = routeNumber.offers.OrderBy(x => x.OperationPrice)
                    .ThenBy(x => x.RouteNumberPriority).ToList();
            }
        }

        public void SelectWinners()
        {
            _routeNumberList = _listContainer.RouteNumberList;
            SortRouteNumberList(_routeNumberList);
            List<Offer> offersToAssign = new List<Offer>();

            _selection.CalculateOperationPriceDifferenceForOffers(_sortedRouteNumberList);
            
            foreach (RouteNumber routeNumber in _sortedRouteNumberList)
            {
                List<Offer> toAddToAssign = _selection.FindWinner(routeNumber);
                foreach (Offer offer in toAddToAssign)
                {
                    offersToAssign.Add(offer);
                }
            }

            List<Offer> offersThatAreIneligible = _selection.AssignWinners(offersToAssign);

            bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatAreIneligible);
            if (allRouteNumberHaveWinner)
            {
                _selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList());
                _selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                List<Offer> winningOffers = CreateWinnerList();
                foreach (Offer offer in winningOffers)
                {
                    _listContainer.OutputList.Add(offer);
                }
            }
            else
            {
                ContinueUntilAllRouteNumbersHaveWinner(offersThatAreIneligible);
            }
        }

        private void ContinueUntilAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            List<Offer> offersToAssign =
                GetOffersToAssign(offersThatAreIneligible);

            var offersThatHaveBeenMarkedIneligible =  _selection.AssignWinners(offersToAssign);
            bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
            if (allRouteNumberHaveWinner)
            {
                _selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList());
                _selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                foreach (Offer offer in CreateWinnerList())
                {
                    _listContainer.OutputList.Add(offer);
                }
            } // Sidste punkt
            else
            {
                ContinueUntilAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
            }
        }

        private List<Offer> GetOffersToAssign(List<Offer> offersThatHaveBeenMarkedIneligible)
        {
            List<Offer> offersToAssign = new List<Offer>();

            foreach (Offer offer in offersThatHaveBeenMarkedIneligible)
            {
                foreach (RouteNumber routeNumber in _sortedRouteNumberList)
                {
                    if (routeNumber.RouteID == offer.RouteID)
                    {
                        
                        List<Offer> offersToAssignToContractor = _selection.FindWinner(routeNumber);
                        offersToAssign.AddRange(offersToAssignToContractor);
                    }
                }
            }

            return offersToAssign;
        }

        private List<Offer> CreateWinnerList()
        {
            return _listContainer.ContractorList.SelectMany(c => c.WinningOffers).ToList();
        }

        private bool DoAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            return offersThatAreIneligible.Count == 0;
        }
    }
}