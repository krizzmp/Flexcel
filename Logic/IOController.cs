using System.Collections.Generic;
using System.Text;
using Domain;
using DataAccess;

namespace Logic
{
    public class IoController : IControllerInterface 
    {  
        public void InitializeExportToPublishList(string filePath)
        {
            CSVExportToPublishList.CreateFile(filePath); 
        }
        public void InitializeExportToCallingList(string filePath)
        {
            CSVExportToCallList.CreateFile(filePath);
        }
        public void InitializeImport(string masterDataFilepath, string routeNumberFilepath, string routeNumbersFilepath)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            var offerLoader = new OfferLoader(encoding);
            var routeNumberLoader = new RouteNumberLoader(encoding);
            var contractorLoader = new ContractorLoader(encoding);
            contractorLoader.Load(masterDataFilepath);
            routeNumberLoader.Load(routeNumbersFilepath);
            offerLoader.Load(routeNumberFilepath);
        }
    }
}
