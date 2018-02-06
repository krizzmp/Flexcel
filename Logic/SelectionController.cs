using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Logic
{
    public class SelectionController
    {
        private readonly ListContainer _listContainer = ListContainer.Instance;
        


        public void Start()
        {
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
            foreach (RouteNumber routeNumber in _listContainer.RouteNumberList)
            {
                routeNumber.CalculateDifference();
            }
        }

        private void AssignWinners()
        {
            foreach (RouteNumber routeNumber in _listContainer.RouteNumberList)
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