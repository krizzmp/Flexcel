using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public sealed class ListContainer
    {
        public List<RouteNumber> RouteNumberList { get; private set; } = new List<RouteNumber>();
        public List<Contractor> ContractorList { get; private set; } = new List<Contractor>();
        public List<Offer> OutputList => RouteNumberList
            .SelectMany(rn => rn.Offers)
            .Where(o => o.Win && o.IsEligible)
            .OrderBy(x => x.UserID)
            .ToList();
        
        public List<Offer> ConflictList { get; } = new List<Offer>();

        private ListContainer()
        {
        }

        public static ListContainer Instance { get; } = new ListContainer();

        public void SetLists(List<RouteNumber> routeNumberList, List<Contractor> contractorList)
        {
            this.RouteNumberList = routeNumberList;
            this.ContractorList = contractorList;
        }
    }
}