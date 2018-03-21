using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public sealed class ListContainer
    {
        public List<RouteNumber> RouteNumberList { get; set; } = new List<RouteNumber>();
        public List<Contractor> ContractorList { get; set; } = new List<Contractor>();

        public List<Offer> OutputList => Offers
            .Where(o => o.Win && o.IsEligible)
            .OrderBy(x => x.UserID)
            .ToList();

        public List<Offer> ConflictList { get; } = new List<Offer>();

        private ListContainer()
        {
            // emty private constructor
        }

        public static ListContainer Instance { get; } = new ListContainer();
        public List<Offer> Offers { get; set; }
        
    }
}