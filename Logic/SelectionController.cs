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
            PrioritizeMultipleWinners();
            //FailIfRouteHasZeroWinners();
            // zero bids on routenumber is already handled
        }

        private void PrioritizeMultipleWinners()
        {
            foreach (RouteNumber routeNumber in _listContainer.RouteNumberList)
            {
                routeNumber.FailForMultipleWinners();
            }
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
            return _listContainer.ContractorList.Any(contractor => contractor.MarkOvercommitedOffersIneligible());
        }
    }
}