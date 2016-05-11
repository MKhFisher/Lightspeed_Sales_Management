using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    public class RootObjectSale
    {
        public Attributes attributes { get; set; }

        [JsonProperty("Sale")]
        [JsonConverter(typeof(SingleOrArrayConverter<Sale>))]
        public List<Sale> Sale { get; set; }
    }

    public class Sale
    {
        public string saleID { get; set; }
        public string timeStamp { get; set; }
        public string discountPercent { get; set; }
        public string completed { get; set; }
        public string archived { get; set; }
        public string voided { get; set; }
        public string enablePromotions { get; set; }
        public string isTaxInclusive { get; set; }
        public string referenceNumber { get; set; }
        public string referenceNumberSource { get; set; }
        public string tax1Rate { get; set; }
        public string tax2Rate { get; set; }
        public string change { get; set; }
        public string receiptPreference { get; set; }
        public string displayableSubtotal { get; set; }
        public string ticketNumber { get; set; }
        public string calcDiscount { get; set; }
        public string calcTotal { get; set; }
        public string calcSubtotal { get; set; }
        public string calcTaxable { get; set; }
        public string calcNonTaxable { get; set; }
        public string calcAvgCost { get; set; }
        public string calcFIFOCost { get; set; }
        public string calcTax1 { get; set; }
        public string calcTax2 { get; set; }
        public string calcPayments { get; set; }
        public string total { get; set; }
        public string totalDue { get; set; }
        public string displayableTotal { get; set; }
        public string balance { get; set; }
        public string customerID { get; set; }
        public string discountID { get; set; }
        public string employeeID { get; set; }
        public string quoteID { get; set; }
        public string registerID { get; set; }
        public string shipToID { get; set; }
        public string shopID { get; set; }
        public string taxCategoryID { get; set; }
        public SaleLines SaleLines { get; set; }
        public string taxTotal { get; set; }
    }

    public class SaleLines
    {
        [JsonProperty("SaleLine")]
        [JsonConverter(typeof(SingleOrArrayConverter<SaleLine>))]
        public List<SaleLine> SaleLine { get; set; }
    }

    public class SaleLine
    {
        public string saleLineID { get; set; }
        public string createTime { get; set; }
        public string timeStamp { get; set; }
        public string unitQuantity { get; set; }
        public string unitPrice { get; set; }
        public string normalUnitPrice { get; set; }
        public string discountAmount { get; set; }
        public string discountPercent { get; set; }
        public string avgCost { get; set; }
        public string fifoCost { get; set; }
        public string tax { get; set; }
        public string tax1Rate { get; set; }
        public string tax2Rate { get; set; }
        public string isLayaway { get; set; }
        public string isWorkorder { get; set; }
        public string isSpecialOrder { get; set; }
        public string displayableSubtotal { get; set; }
        public string displayableUnitPrice { get; set; }
        public string calcLineDiscount { get; set; }
        public string calcTransactionDiscount { get; set; }
        public string calcTotal { get; set; }
        public string calcSubtotal { get; set; }
        public string calcTax1 { get; set; }
        public string calcTax2 { get; set; }
        public string taxClassID { get; set; }
        public string customerID { get; set; }
        public string discountID { get; set; }
        public string employeeID { get; set; }
        public string itemID { get; set; }
        public string noteID { get; set; }
        public string parentSaleLineID { get; set; }
        public string shopID { get; set; }
        public string saleID { get; set; }
        public TaxClass TaxClass { get; set; }
        public Discount Discount { get; set; }
        public Item Item { get; set; }
    }

    public class TaxClass
    {
        public string taxClassID { get; set; }
        public string name { get; set; }
        public string timeStamp { get; set; }
    }

    public class Discount
    {
        public string discountID { get; set; }
        public string name { get; set; }
        public string discountAmount { get; set; }
        public string discountPercent { get; set; }
        public string requireCustomer { get; set; }
        public string archived { get; set; }
        public string timeStamp { get; set; }
    }

    public class RootObjectShop
    {
        public Attributes attributes { get; set; }
        [JsonProperty("Shop")]
        [JsonConverter(typeof(SingleOrArrayConverter<Shop>))]
        public List<Shop> Shop { get; set; }
    }

public class Shop
    {
        public string shopID { get; set; }
        public string name { get; set; }
        public string serviceRate { get; set; }
        public string timeZone { get; set; }
        public string taxLabor { get; set; }
        public string labelTitle { get; set; }
        public string labelMsrp { get; set; }
        public string archived { get; set; }
        public string timeStamp { get; set; }
        public string contactID { get; set; }
        public string taxCategoryID { get; set; }
        public string receiptSetupID { get; set; }
        public string ccGatewayID { get; set; }
        public string priceLevelID { get; set; }
        public string companyRegistrationNumber { get; set; }
        public string vatNumber { get; set; }
    }

    public class ABSSale
    {
        public string AccountID { get; set; }
        public string Store { get; set; }
        public string SalesLineID { get; set; }
        public string ManufacturerName { get; set; }
        public string ItemNumber { get; set; }
        public string UPC { get; set; }
        public string ItemName { get; set; }
        public string QtySold { get; set; }
        public string Cost { get; set; }
        public string PricePerItem { get; set; }
        public string DiscountedPrice { get; set; }
        public string Total { get; set; }
        public string OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public string OrderTakerID { get; set; }
        public string ReturnReasons { get; set; }
    }

    class SaleManagement
    {
        public static void GetSales(List<StoreAuthentication> stores)
        {
            DateTime today = DateTime.Today;

            APIManagement.UpdateManufacturerIDs(stores[0]);
            APIManagement.UpdateCategoryIDs(stores[0]);
            APIManagement.UpdateEmployeeIDs(stores[0]);

            GetSalesFromLightspeed(stores);
            //CreateSalesFiles(stores, today);
        }

        public static void ConsolidateStores(List<StoreAuthentication> stores)
        {
            SQLManagement.PrepareQlikTable();
            SQLManagement.AddStoreToConsolidation();
            SQLManagement.FormatConsolidatedTable();
        }

        private static void CreateSalesFiles(List<StoreAuthentication> stores, DateTime today)
        {
            string year = today.ToString("yyyy");
            string month = today.ToString("MMMM");
            foreach (StoreAuthentication store in stores)
            {
                Directory.CreateDirectory(@"C:\Share\Retail\" + store.store + @"\" + year + @"\" + month + @"\");
                ExcelManagement.CreateDailySalesReport(@"C:\Share\Retail\" + store.store + @"\" + year + @"\" + month + @"\", store, today);
            }
        }

        private static void GetSalesFromLightspeed(List<StoreAuthentication> stores)
        {
            APIManagement.UpdateShopIDs(stores[0]);
            Dictionary<string, Shop> shops = SQLManagement.GetShopIDs();

            foreach (StoreAuthentication store in stores)
            {
                string shopID = "";
                try
                {
                    shopID = shops[store.name].shopID;
                }
                catch
                {
                    if (store.name == "Irvine Old")
                    {
                        shopID = "1";
                    }
                    else
                    {
                        shopID = "0";
                    }
                }

                int offset = 0;
                RootObjectSale sales = null;
                Dictionary<string, ABSSale> saleIDs = new Dictionary<string, ABSSale>();
                int unique = 0;

                DateTime lastDate = SQLManagement.GetLastSalesDate(store);
                if (lastDate.Year != 2000)
                {
                    SQLManagement.DeleteLastDate(store, lastDate);
                }

                do
                {
                    sales = APIManagement.GetSaleObject(store, shopID, offset, lastDate);
                    offset += 100;

                    if (sales.Sale != null)
                    {
                        foreach (Sale entry in sales.Sale)
                        {
                            if (entry.SaleLines != null)
                            {
                                foreach (SaleLine entrySL in entry.SaleLines.SaleLine)
                                {
                                    ABSSale temp = null;
                                    string[] timeStamp = entry.timeStamp.Split(' ');
                                    string[] date = timeStamp[0].Split('/');
                                    string month = date[0];
                                    string day = date[1];
                                    string year = date[2];

                                    if (!saleIDs.ContainsKey(entrySL.saleLineID) && entrySL.Item != null)
                                    {
                                        if (entrySL.Item.upc != "")
                                        {
                                            temp = new ABSSale() { AccountID = store.accountID, Store = store.name, SalesLineID = entrySL.saleLineID, ManufacturerName = entrySL.Item.manufacturerID, ItemNumber = entrySL.Item.customSku, UPC = entrySL.Item.upc, ItemName = entrySL.Item.description, QtySold = entrySL.unitQuantity, Cost = entrySL.Item.defaultCost, PricePerItem = entrySL.displayableUnitPrice, DiscountedPrice = Math.Round(decimal.Parse(entrySL.displayableSubtotal) / decimal.Parse(entrySL.unitQuantity), 2).ToString(), Total = entrySL.displayableSubtotal, OrderDate = year + month + day, OrderNumber = entry.saleID, OrderTakerID = entry.employeeID, ReturnReasons = "" };
                                        }
                                        else
                                        {
                                            temp = temp = new ABSSale() { AccountID = store.accountID, Store = store.name, SalesLineID = entrySL.saleLineID, ManufacturerName = entrySL.Item.manufacturerID, ItemNumber = entrySL.Item.customSku, UPC = "Misc" + unique.ToString(), ItemName = entrySL.Item.description, QtySold = entrySL.unitQuantity, Cost = entrySL.Item.defaultCost, PricePerItem = entrySL.displayableUnitPrice, DiscountedPrice = Math.Round(decimal.Parse(entrySL.displayableSubtotal) / decimal.Parse(entrySL.unitQuantity), 2).ToString(), Total = entrySL.displayableSubtotal, OrderDate = year + month + day, OrderNumber = entry.saleID, OrderTakerID = entry.employeeID, ReturnReasons = "" };
                                            unique++;
                                        }
                                    }
                                    else if (!saleIDs.ContainsKey(entrySL.saleLineID) && entrySL.Item == null)
                                    {
                                        temp = new ABSSale() { AccountID = store.accountID, Store = store.name, SalesLineID = entrySL.saleLineID, ManufacturerName = "Misc", ItemNumber = "Misc", UPC = "Misc" + unique.ToString(), ItemName = "Misc", QtySold = entrySL.unitQuantity, Cost = "0.00", PricePerItem = entrySL.displayableUnitPrice, DiscountedPrice = Math.Round(decimal.Parse(entrySL.displayableSubtotal) / decimal.Parse(entrySL.unitQuantity), 2).ToString(), Total = entrySL.displayableSubtotal, OrderDate = year + month + day, OrderNumber = entry.saleID, OrderTakerID = entry.employeeID, ReturnReasons = "" };
                                        unique++;
                                    }
                                    saleIDs.Add(entrySL.saleLineID, temp);
                                }
                            }
                        }
                    }
                }
                while (sales.Sale != null);

                DataTable tableDB = CreateLSRSale();
                foreach (KeyValuePair<string, ABSSale> entry in saleIDs)
                {
                    tableDB.Rows.Add(new object[] { entry.Value.AccountID, entry.Value.Store, entry.Value.SalesLineID, entry.Value.ManufacturerName, entry.Value.ItemNumber, entry.Value.UPC, entry.Value.ItemName, entry.Value.QtySold, entry.Value.Cost, entry.Value.PricePerItem, entry.Value.DiscountedPrice, entry.Value.Total, entry.Value.OrderDate, entry.Value.OrderNumber, entry.Value.OrderTakerID, entry.Value.ReturnReasons });
                }

                SQLManagement.UpsertSales(tableDB);
            }
        }

        private static DataTable CreateLSRSale()
        {
            DataTable result = new DataTable();
            result.TableName = "Lightspeed_SalesTemp";
            result.Columns.Add("AccountID");
            result.Columns.Add("Store");
            result.Columns.Add("SalesLineID");
            result.Columns.Add("ManufacturerName");
            result.Columns.Add("ItemNumber");
            result.Columns.Add("UPC");
            result.Columns.Add("ItemName");
            result.Columns.Add("QtySold");
            result.Columns.Add("Cost");
            result.Columns.Add("PricePerItem");
            result.Columns.Add("DiscountedPrice");
            result.Columns.Add("Total");
            result.Columns.Add("OrderDate");
            result.Columns.Add("OrderNumber");
            result.Columns.Add("OrderTakerID");
            result.Columns.Add("ReturnReasons");

            return result;
        }

        public static DataTable CreateLSRShop()
        {
            DataTable result = new DataTable();
            result.TableName = "Lightspeed_ShopsTemp";
            result.Columns.Add("ShopID", typeof(string));
            result.Columns.Add("AccountID", typeof(string));

            return result;
        }
    }
}
