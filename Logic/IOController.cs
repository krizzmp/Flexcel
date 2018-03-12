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
            csvImport.LoadConctractors(masterDataFilepath);
            csvImport.LoadRouteNumbers(routeNumbersFilepath);
            csvImport.LoadOffers(routeNumberFilepath);
        }
    }
}
