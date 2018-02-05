using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Logic
{
    public class SelectionController
    {
        private List<RouteNumber> _sortedRouteNumberList;
        private readonly ListContainer _listContainer = ListContainer.Instance;

        private void SortRouteNumberList(List<RouteNumber> routeNumberList)
        {
            _sortedRouteNumberList = routeNumberList.OrderBy(x => x.RouteId).ToList();
            foreach (RouteNumber routeNumber in _sortedRouteNumberList)
            {
                routeNumber.Offers = routeNumber.Offers.OrderBy(x => x.OperationPrice)
                    .ThenBy(x => x.RouteNumberPriority).ToList();
            }
        }


        public void Start()
        {
            SortRouteNumberList(_listContainer.RouteNumberList);
            bool t = true;
            do
            {
                CalculateDifference();
                AssignWinners();
                t = MarkOvercommitedOffersIneligible();
            } while (t);

            // when no more OvercommitedOffers
            //FailIfRouteHasMultipleWinners();
            //FailIfRouteHasZeroWinners();
        }

        private void CalculateDifference()
        {
            foreach (RouteNumber routeNumber in _sortedRouteNumberList)
            {
                routeNumber.CalculateDifference();
            }
        }

        private void AssignWinners()
        {
            foreach (RouteNumber routeNumber in _sortedRouteNumberList)
            {
                routeNumber.AssignWinner();
            }
        }

        private bool MarkOvercommitedOffersIneligible()
        {
            //foreach (Contractor contractor in _listContainer.ContractorList)
            //{
            //    contractor.MarkOvercommitedOffersIneligible();
            //}

            return _listContainer.ContractorList.Any(contractor => contractor.MarkOvercommitedOffersIneligible());
        }
    }
}