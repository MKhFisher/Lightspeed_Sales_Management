using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lightspeed_Product_Management
{
    class StoreAuthentication
    {
        public decimal rate = 0.5M;
        public decimal utilRate = 0.75M;
        public string base_uri = "https://api.merchantos.com";
        public string account { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string store { get; set; }
        public string accountID { get; set; }
    }

    [XmlRoot("stores")]
    public class Stores
    {
        [XmlElement("store")]
        public List<Store> StoreList { get; set; }
    }

    public class Store
    {
        [XmlElement("AccountID")]
        public string AccountID { get; set; }
        [XmlElement("StoreName")]
        public string StoreName { get; set; }
        [XmlElement("APIKey")]
        public string APIKey { get; set; }
        [XmlElement("Account")]
        public string Account { get; set; }
    }

    class APIManagement
    {
        public static List<StoreAuthentication> InitializeAccounts()
        {
            List<StoreAuthentication> result = new List<StoreAuthentication>();

            //result.Add(new StoreAuthentication { account = "92470", token = "d513c860b5d12baf5351210d1ef23be6230e4922", table = "IrvineOld", store = "31002", accountID = "11:11:31002:1" });

            result.Add(new StoreAuthentication { account = "93799", token = "13cf12003f6dbea7883161d84538fa72e22b75c6", name = "Fresno", store = "31001", accountID = "11:11:31001:1" });
            result.Add(new StoreAuthentication { account = "93799", token = "13cf12003f6dbea7883161d84538fa72e22b75c6", name = "Irvine", store = "31002", accountID = "11:11:31002:1" });
            result.Add(new StoreAuthentication { account = "93799", token = "13cf12003f6dbea7883161d84538fa72e22b75c6", name = "Riverside", store = "31003", accountID = "11:11:31003:1" });
            result.Add(new StoreAuthentication { account = "93799", token = "13cf12003f6dbea7883161d84538fa72e22b75c6", name = "Las Vegas", store = "31004", accountID = "11:11:31004:1" });
            result.Add(new StoreAuthentication { account = "93799", token = "13cf12003f6dbea7883161d84538fa72e22b75c6", name = "Sacramento", store = "31005", accountID = "11:11:31005:1" });
            result.Add(new StoreAuthentication { account = "93799", token = "13cf12003f6dbea7883161d84538fa72e22b75c6", name = "Carson", store = "31006", accountID = "11:11:31006:1" });

            return result;
        }

        public static List<StoreAuthentication> InitializeAccounts(string storesXMLFile)
        {
            List<StoreAuthentication> result = new List<StoreAuthentication>();

            XmlSerializer serializer = new XmlSerializer(typeof(Stores));
            Stores stores;
            using (TextReader reader = new StreamReader(storesXMLFile))
            {
                stores = (Stores)serializer.Deserialize(reader);
            }

            foreach (Store s in stores.StoreList)
            {
                result.Add(new StoreAuthentication { account = s.Account, token = s.APIKey, name = s.StoreName, store = s.AccountID.Split(':')[2], accountID = s.AccountID });
            }

            return result;
        }

        public static void GetLightspeedProductDetails(StoreAuthentication store, List<StoreAuthentication> stores)
        {
            int offset = 0;
            RootObjectItem item = null;
            Dictionary<string, ABSItem> UPCs = new Dictionary<string, ABSItem>();
            List<ABSItem> items = new List<ABSItem>();

            do
            {
                item = GetItemObject(store, "", offset);
                offset += 100;

                if (item.Item != null)
                {
                    foreach (Item entry in item.Item)
                    {
                        ABSItem temp = null;
                        if (store.name == "Irvine" && entry.ItemShops.ItemShop.Where(x => x.shopID == "6").FirstOrDefault() != null)
                        {
                            //temp = new ABSItem() { Cost = entry.defaultCost, Status = entry.archived, ProductName = entry.description, UPC = entry.upc, CustomSKU = entry.customSku, Category = entry.Category == null ? "" : entry.Category.fullPathName, Manufacturer = entry.Manufacturer == null ? "" : entry.Manufacturer.name, Size = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute2, Color = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute1, Dimension = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute3, MAP = entry.Prices.ItemPrice.Where(x => x.useType == "Default").FirstOrDefault().amount, MSRP = entry.Prices.ItemPrice.Where(x => x.useType == "MSRP").FirstOrDefault().amount, ItemID = entry.itemID, Inventory = entry.ItemShops.ItemShop.Where(x => x.shopID == "1").FirstOrDefault().qoh, SystemID = entry.systemSku };
                            temp = new ABSItem() { Cost = entry.defaultCost, Status = entry.archived, ProductName = entry.description, UPC = entry.upc, CustomSKU = entry.customSku, Category = entry.Category == null ? "" : entry.Category.fullPathName, Manufacturer = entry.Manufacturer == null ? "" : entry.Manufacturer.name, Size = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute2, Color = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute1, Dimension = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute3, MAP = entry.Prices.ItemPrice.Where(x => x.useType == "Default").FirstOrDefault().amount, MSRP = entry.Prices.ItemPrice.Where(x => x.useType == "MSRP").FirstOrDefault().amount, ItemID = entry.itemID, Inventory = entry.ItemShops.ItemShop.Where(x => x.shopID == "6").FirstOrDefault().qoh, SystemID = entry.systemSku };
                        }
                        else if (store.name == "Riverside" && entry.ItemShops.ItemShop.Where(x => x.shopID == "1").FirstOrDefault() != null)
                        {
                            temp = new ABSItem() { Cost = entry.defaultCost, Status = entry.archived, ProductName = entry.description, UPC = entry.upc, CustomSKU = entry.customSku, Category = entry.Category == null ? "" : entry.Category.fullPathName, Manufacturer = entry.Manufacturer == null ? "" : entry.Manufacturer.name, Size = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute2, Color = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute1, Dimension = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute3, MAP = entry.Prices.ItemPrice.Where(x => x.useType == "Default").FirstOrDefault().amount, MSRP = entry.Prices.ItemPrice.Where(x => x.useType == "MSRP").FirstOrDefault().amount, ItemID = entry.itemID, Inventory = entry.ItemShops.ItemShop.Where(x => x.shopID == "1").FirstOrDefault().qoh, SystemID = entry.systemSku };
                        }
                        //else if (store.table == "Fresno")
                        else if (store.name == "Fresno" && entry.ItemShops.ItemShop.Where(x => x.shopID == "3").FirstOrDefault() != null)
                        {
                            temp = new ABSItem() { Cost = entry.defaultCost, Status = entry.archived, ProductName = entry.description, UPC = entry.upc, CustomSKU = entry.customSku, Category = entry.Category == null ? "" : entry.Category.fullPathName, Manufacturer = entry.Manufacturer == null ? "" : entry.Manufacturer.name, Size = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute2, Color = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute1, Dimension = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute3, MAP = entry.Prices.ItemPrice.Where(x => x.useType == "Default").FirstOrDefault().amount, MSRP = entry.Prices.ItemPrice.Where(x => x.useType == "MSRP").FirstOrDefault().amount, ItemID = entry.itemID, Inventory = entry.ItemShops.ItemShop.Where(x => x.shopID == "3").FirstOrDefault() == null ? "0" : entry.ItemShops.ItemShop.Where(x => x.shopID == "3").FirstOrDefault().qoh, SystemID = entry.systemSku };
                        }
                        else if (store.name == "Las Vegas" && entry.ItemShops.ItemShop.Where(x => x.shopID == "5").FirstOrDefault() != null)
                        {
                            temp = new ABSItem() { Cost = entry.defaultCost, Status = entry.archived, ProductName = entry.description, UPC = entry.upc, CustomSKU = entry.customSku, Category = entry.Category == null ? "" : entry.Category.fullPathName, Manufacturer = entry.Manufacturer == null ? "" : entry.Manufacturer.name, Size = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute2, Color = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute1, Dimension = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute3, MAP = entry.Prices.ItemPrice.Where(x => x.useType == "Default").FirstOrDefault().amount, MSRP = entry.Prices.ItemPrice.Where(x => x.useType == "MSRP").FirstOrDefault().amount, ItemID = entry.itemID, Inventory = entry.ItemShops.ItemShop.Where(x => x.shopID == "5").FirstOrDefault().qoh, SystemID = entry.systemSku };
                        }
                        else if (store.name == "Sacramento" && entry.ItemShops.ItemShop.Where(x => x.shopID == "7").FirstOrDefault() != null)
                        {
                            temp = new ABSItem() { Cost = entry.defaultCost, Status = entry.archived, ProductName = entry.description, UPC = entry.upc, CustomSKU = entry.customSku, Category = entry.Category == null ? "" : entry.Category.fullPathName, Manufacturer = entry.Manufacturer == null ? "" : entry.Manufacturer.name, Size = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute2, Color = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute1, Dimension = entry.ItemAttributes == null ? "" : entry.ItemAttributes.attribute3, MAP = entry.Prices.ItemPrice.Where(x => x.useType == "Default").FirstOrDefault().amount, MSRP = entry.Prices.ItemPrice.Where(x => x.useType == "MSRP").FirstOrDefault().amount, ItemID = entry.itemID, Inventory = entry.ItemShops.ItemShop.Where(x => x.shopID == "7").FirstOrDefault().qoh, SystemID = entry.systemSku };
                        }

                        if (temp.UPC == "")
                        {
                            APIManagement.DeleteItem(store, temp.ItemID);
                        }

                        if (!UPCs.ContainsKey(entry.systemSku) && temp != null)
                        {
                            UPCs.Add(entry.systemSku, temp);
                        }
                    }
                }
            }
            while (item.Item != null);

            DataTable tableLSR = ProductManagement.CreateLSRItem();
            foreach (KeyValuePair<string, ABSItem> entry in UPCs)
            {
                tableLSR.Rows.Add(new object[] { entry.Value.Cost, entry.Value.Status, entry.Value.ProductName, entry.Value.UPC, entry.Value.CustomSKU, entry.Value.Category, entry.Value.Manufacturer, entry.Value.Size, entry.Value.Color, entry.Value.Dimension, entry.Value.MAP, entry.Value.MSRP, entry.Value.ItemID, entry.Value.Inventory, entry.Value.SystemID });
            }

            foreach (StoreAuthentication s in stores)
            {
                tableLSR.TableName = "Lightspeed_UPCs";    

                SQLManagement.TruncateUPCsTable();
                int write = SQLManagement.WriteToSQL(tableLSR);
            }
        }

        public static RootObjectSale GetSaleObject(StoreAuthentication store, string query, int offset, DateTime date)
        {
            string shopID = "";
            var client = new RestClient(store.base_uri);

            var request = new RestRequest(Method.GET);
            request.Resource = "/API/Account/" + store.account + "/Sale?oauth_token=" + store.token + "&load_relations=all&completed=true&offset=" + offset.ToString() + "&shopID=" + query + "&timeStamp=><," + date.ToString("yyyy-MM-dd") + "T00:00-08:00,2100-12-31T23:59:59-08:00";
            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                WaitForBucket(response, store);
                return GetSaleObject(store, query, offset, date);
            }
            else
            {
                var content = response.Content;
                return JsonConvert.DeserializeObject<RootObjectSale>(content);
            }
        }

        public static void SendPOSTRequest(ItemCreation item, StoreAuthentication store, string query, int offset)
        {
            var client = new RestClient(store.base_uri);

            var request = new RestRequest(Method.POST);
            request.Resource = "/API/Account/" + store.account + "/Item/" + query + "?oauth_token=" + store.token;
            string json = JsonConvert.SerializeObject(item);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }
            else if (response.StatusCode == 0)
            {
                SendPOSTRequest(item, store, query, offset);
            }
            else if (response.StatusCode.ToString() == "429")
            {
                WaitForBucket(response, store);
                SendPOSTRequest(item, store, query, offset);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                SendPOSTRequest(item, store, query, offset);
            }
        }

        public static void SendPUTRequest(ItemCreation items, StoreAuthentication store, string query)
        {
            var client = new RestClient(store.base_uri);

            var request = new RestRequest(Method.PUT);
            request.Resource = "/API/Account/" + store.account + "/Item/" + query + "/?oauth_token=" + store.token;
            string json = JsonConvert.SerializeObject(items);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }
            else if (response.StatusCode == 0)
            {
                SendPUTRequest(items, store, query);
            }
            else if (response.StatusCode.ToString() == "429")
            {
                WaitForBucket(response, store);
                SendPUTRequest(items, store, query);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.Content.Contains("You can not have more than one ItemVendorNum for each vendorID."))
                {
                    using (StreamWriter sw = new StreamWriter(@"C:\Users\MichaelF\Desktop\DuplicateItemVendorNums.txt", true))
                    {
                        sw.WriteLine("ItemID: " + query + ". UPC: " + items.upc + ".");
                        Console.WriteLine("ItemID: " + query + ". UPC: " + items.upc + ".");
                    }
                    //RootObjectItem item = GetItemObject(store, query, 0);
                    //while (item.Item[0].ItemVendorNums.ItemVendorNum[0].itemVendorNumID != item.Item[0].ItemVendorNums.ItemVendorNum[1].itemVendorNumID)
                    //{
                    //    string deletedItemVendorNumID = item.Item[0].ItemVendorNums.ItemVendorNum[1].itemVendorNumID;
                    //    //DeleteAdditionalItemVendorNums(store, item.Item[0].itemID, item.Item[0].ItemVendorNums.ItemVendorNum[1].itemVendorNumID);
                    //    items.ItemVendorNums = item.Item[0].ItemVendorNums;
                    //    items.ItemVendorNums.ItemVendorNum.RemoveAt(1);
                    //    SendPUTRequest(items, store, query);
                    //    item = GetItemObject(store, query, 0);
                    //}
                    return;
                }
                SendPUTRequest(items, store, query);
            }
        }

        private static void DeleteItem(StoreAuthentication store, string itemID)
        {
            var client = new RestClient(store.base_uri);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "/API/Account/" + store.account + "/Item/" + itemID + "?oauth_token=" + store.token;
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }
            else if (response.StatusCode.ToString() == "429")
            {
                WaitForBucket(response, store);
                DeleteItem(store, itemID);
            }
            else if (response.StatusCode == 0)
            {
                DeleteItem(store, itemID);
            }
            else
            {
                EmailManagement.SendImpersonationEmail(new string[] { "michaelf@511tactical.com" }, "[ERROR] Delete Extra ItemVendorNums", response.StatusCode.ToString(), null);
            }
        }

        public static RootObjectItem GetItemObject(StoreAuthentication store, string query, int offset)
        {
            var client = new RestClient(store.base_uri);

            var request = new RestRequest(Method.GET);
            request.Resource = "/API/Account/" + store.account + "/Item/" + query + "?oauth_token=" + store.token + "&archived=0&load_relations=[\"ItemShops\", \"Category\", \"Manufacturer\", \"ItemAttributes\", \"ItemVendorNums\"]&offset=" + offset.ToString();
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                return JsonConvert.DeserializeObject<RootObjectItem>(content);
            }
            else if (response.StatusCode == 0)
            {
                return GetItemObject(store, query, offset);
            }
            else if (response.StatusCode.ToString() == "429")
            {
                WaitForBucket(response, store);
                return GetItemObject(store, query, offset);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                return GetItemObject(store, query, offset);
            }
            else
            {
                return GetItemObject(store, query, offset);
            }
        }

        public static void UpdateManufacturerIDs(StoreAuthentication store)
        {
            int offset = 0;
            RootObjectManufacturer manufacturerIDs = null;
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            do
            {
                var client = new RestClient(store.base_uri);

                var request = new RestRequest(Method.GET);
                request.Resource = "/API/Account/" + store.account + "/Manufacturer/?oauth_token=" + store.token + "&offset=" + offset;
                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    WaitForBucket(response, store);
                    UpdateManufacturerIDs(store);
                }
                else
                {
                    var content = response.Content;
                    manufacturerIDs = JsonConvert.DeserializeObject<RootObjectManufacturer>(content);
                    if (manufacturerIDs.Manufacturer != null)
                    {
                        foreach (Manufacturer m in manufacturerIDs.Manufacturer)
                        {
                            manufacturers.Add(m);
                        }
                    }
                }



                offset += 100;
            }
            while (manufacturerIDs.Manufacturer != null);

            SQLManagement.UpsertManufacturers(manufacturers);
        }

        public static void UpdateEmployeeIDs(StoreAuthentication store)
        {
            int offset = 0;
            RootObjectEmployee employeeIDs = null;
            List<Employee> employees = new List<Employee>();

            do
            {
                var client = new RestClient(store.base_uri);

                var request = new RestRequest(Method.GET);
                request.Resource = "/API/Account/" + store.account + "/Employee/?oauth_token=" + store.token + "&offset=" + offset;
                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    WaitForBucket(response, store);
                    UpdateEmployeeIDs(store);
                }
                else
                {
                    var content = response.Content;
                    employeeIDs = JsonConvert.DeserializeObject<RootObjectEmployee>(content);
                    if (employeeIDs.Employee != null)
                    {
                        foreach (Employee e in employeeIDs.Employee)
                        {
                            employees.Add(e);
                        }
                    }
                }
                offset += 100;
            }
            while (employeeIDs.Employee != null);

            SQLManagement.UpsertEmployees(employees);
        }

        public static void UpdateCategoryIDs(StoreAuthentication store)
        {
            int offset = 0;
            RootObjectCategory categoryIDs = null;
            List<Category> categories = new List<Category>();

            do
            {
                var client = new RestClient(store.base_uri);

                var request = new RestRequest(Method.GET);
                request.Resource = "/API/Account/" + store.account + "/Category/?oauth_token=" + store.token + "&offset=" + offset;
                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    WaitForBucket(response, store);
                    UpdateCategoryIDs(store);
                }
                else
                {
                    var content = response.Content;
                    categoryIDs = JsonConvert.DeserializeObject<RootObjectCategory>(content);
                    if (categoryIDs.Category != null)
                    {
                        foreach (Category c in categoryIDs.Category)
                        {
                            categories.Add(c);
                        }
                    }
                }
                offset += 100;
            }
            while (categoryIDs.Category != null);

            SQLManagement.UpsertCategories(categories);
        }

        public static void UpdateShopIDs(StoreAuthentication store)
        {
            int offset = 0;
            RootObjectShop shopIDs = null;
            List<Shop> shops = new List<Shop>();

            do
            {
                var client = new RestClient(store.base_uri);

                var request = new RestRequest(Method.GET);
                request.Resource = "/API/Account/" + store.account + "/Shop/?oauth_token=" + store.token + "&offset=" + offset;
                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    WaitForBucket(response, store);
                    UpdateShopIDs(store);
                }
                else
                {
                    var content = response.Content;
                    shopIDs = JsonConvert.DeserializeObject<RootObjectShop>(content);
                    if (shopIDs.Shop != null)
                    {
                        foreach (Shop s in shopIDs.Shop)
                        {
                            shops.Add(s);
                        }
                    }
                }
                offset += 100;
            }
            while (shopIDs.Shop != null);

            SQLManagement.UpsertShops(shops);
        }

        private static void WaitForBucket(IRestResponse response, StoreAuthentication store)
        {
            string[] usage = response.Headers[0].Value.ToString().Split('/');

            decimal numerator = decimal.Parse(usage[0]);
            decimal denominator = decimal.Parse(usage[1]);
            decimal utilization = numerator / denominator;
            decimal threshold = store.utilRate * denominator;

            int waitTime = (int)Math.Round((numerator - threshold) / store.rate, MidpointRounding.ToEven);
            Thread.Sleep(waitTime * 1000);
        }
    }
}
