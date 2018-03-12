using System.Text;

namespace DataAccess
{
    public class CSVImport
    {
        
        private readonly ContractorLoader _contractorLoader;
        private readonly OfferLoader _offerLoader;
        private readonly RouteNumberLoader _routeNumberLoader;

        public CSVImport()
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            _offerLoader = new OfferLoader(encoding);
            _routeNumberLoader = new RouteNumberLoader(encoding);
            _contractorLoader = new ContractorLoader(encoding);
        }


        public void LoadOffers(string filepath)
        {
            _offerLoader.Load(filepath);
        }

        public void LoadRouteNumbers(string filepath)
        {
            _routeNumberLoader.Load(filepath);
        }

        public void LoadConctractors(string filepath)
        {
            _contractorLoader.Load(filepath);
        }
    }
}