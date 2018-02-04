using System.Collections.Generic;

namespace Domain
{
    public sealed class ListContainer
    {
        public List<RouteNumber> RouteNumberList;
        public List<Contractor> ContractorList;
        public readonly List<Offer> OutputList;
        public readonly List<Offer> ConflictList;

        private ListContainer()
        {
            RouteNumberList = new List<RouteNumber>(); 
            ContractorList = new List<Contractor>();
            OutputList = new List<Offer>();
            ConflictList = new List<Offer>();
        }

        public static ListContainer Instance { get; } = new ListContainer();

        public void SetLists(List<RouteNumber> routeNumberList, List<Contractor> contractorList)
        {
            this.RouteNumberList = routeNumberList;
            this.ContractorList = contractorList;
        }
    }
}
