using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IControllerInterface
    {
        void InitializeExportToPublishList(string filePath);

        void InitializeExportToCallingList(string filePath);

        void InitializeImport(string masterDataFilepath, string routeNumberFilepath, string routeNumbersFilepath);
    }


}
