using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    class Program
    {
        static string storesXMLFile = @"\\boomi\c$\share\Lightspeed\Stores.xml";

        static void Main(string[] args)
        {
            //string token = Authentication.Authenticate("admin_riverside@511tactical.com");

            List<StoreAuthentication> stores = APIManagement.InitializeAccounts(storesXMLFile);
            SaleManagement.GetSales(stores);
            SaleManagement.ConsolidateStores(stores);
        }
    }
}