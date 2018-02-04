using System.Collections.Generic;
using Domain;
using DataAccess;

namespace Logic
{
    public class IOController
    {
        private List<RouteNumber> _routeNumberList;
        private List<Contractor> _contractorList;

        public IOController()
        {
            _routeNumberList = new List<RouteNumber>();
        }
       
        public void InitializeExportToPublishList(string filePath)
        {
            CSVExportToPublishList exportToPublishList = new CSVExportToPublishList(filePath);
            exportToPublishList.CreateFile(); 
        }
        public void InitializeExportToCallingList(string filePath)
        {
            CSVExportToCallList exportCallList = new CSVExportToCallList(filePath);
            exportCallList.CreateFile();
        }
        public void InitializeImport(string masterDataFilepath, string routeNumberFilepath, string routeNumbersFilepath)
        {
            CSVImport csvImport = new CSVImport();
            csvImport.ImportContractors(masterDataFilepath);
            csvImport.ImportRouteNumbers(routeNumbersFilepath);
            csvImport.ImportOffers(routeNumberFilepath);
            _contractorList = csvImport.GetListOfContractors();
            _routeNumberList = csvImport.GetListOfRouteNumbers();
            ListContainer listContainer = ListContainer.Instance;
            listContainer.SetLists(_routeNumberList, _contractorList);
        }
    }
}
