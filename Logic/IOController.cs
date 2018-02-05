using System.Collections.Generic;
using Domain;
using DataAccess;

namespace Logic
{
    public class IOController
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
            CSVImport csvImport = new CSVImport();
            csvImport.ImportContractors(masterDataFilepath);
            csvImport.ImportRouteNumbers(routeNumbersFilepath);
            csvImport.ImportOffers(routeNumberFilepath);
            ListContainer listContainer = ListContainer.Instance;
            listContainer.SetLists(csvImport.GetListOfRouteNumbers(), csvImport.GetListOfContractors());
        }
    }
}
