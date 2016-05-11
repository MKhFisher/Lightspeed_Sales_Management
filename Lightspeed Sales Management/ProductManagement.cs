using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            return new List<T> { token.ToObject<T>() };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class Attributes
    {
        public string count { get; set; }
    }

    public class RootObjectItem
    {
        public Attributes attributes { get; set; }

        [JsonProperty("Item")]
        [JsonConverter(typeof(SingleOrArrayConverter<Item>))]
        public List<Item> Item { get; set; }
    }

    public class Item
    {
        public string itemID { get; set; }
        public string systemSku { get; set; }
        public string defaultCost { get; set; }
        public string avgCost { get; set; }
        public string discountable { get; set; }
        public string tax { get; set; }
        public string archived { get; set; }
        public string itemType { get; set; }
        public string description { get; set; }
        public string modelYear { get; set; }
        public string upc { get; set; }
        public string ean { get; set; }
        public string customSku { get; set; }
        public string manufacturerSku { get; set; }
        public string createTime { get; set; }
        public string timeStamp { get; set; }
        public string categoryID { get; set; }
        public string taxClassID { get; set; }
        public string departmentID { get; set; }
        public string itemMatrixID { get; set; }
        public string manufacturerID { get; set; }
        public string seasonID { get; set; }
        public string defaultVendorID { get; set; }
        public string itemECommerceID { get; set; }
        public Category Category { get; set; }
        public ItemAttributes ItemAttributes { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public ItemShops ItemShops { get; set; }
        public ItemVendorNums ItemVendorNums { get; set; }
        public Prices Prices { get; set; }
    }

    public class Category
    {
        public string categoryID { get; set; }
        public string name { get; set; }
        public string nodeDepth { get; set; }
        public string fullPathName { get; set; }
        public string leftNode { get; set; }
        public string rightNode { get; set; }
        public string createTime { get; set; }
        public string timeStamp { get; set; }
        public string parentID { get; set; }
    }

    public class ItemAttributes
    {
        public string attribute1 { get; set; }
        public string attribute2 { get; set; }
        public string attribute3 { get; set; }
        public string itemAttributeSetID { get; set; }
    }

    public class Manufacturer
    {
        public string manufacturerID { get; set; }
        public string name { get; set; }
        public string createTime { get; set; }
        public string timeStamp { get; set; }
    }

    public class ItemShops
    {
        [JsonProperty("ItemShop")]
        [JsonConverter(typeof(SingleOrArrayConverter<ItemShop>))]
        public List<ItemShop> ItemShop { get; set; }
    }

    public class ItemShop
    {
        public string itemShopID { get; set; }
        public string qoh { get; set; }
        public string backorder { get; set; }
        public string componentQoh { get; set; }
        public string componentBackorder { get; set; }
        public string reorderPoint { get; set; }
        public string reorderLevel { get; set; }
        public string timeStamp { get; set; }
        public string itemID { get; set; }
        public string shopID { get; set; }
        public M m { get; set; }
    }

    public class M
    {
        public string layaways { get; set; }
        public string specialorders { get; set; }
        public string workorders { get; set; }
    }

    public class Prices
    {
        public List<ItemPrice> ItemPrice { get; set; }
    }

    public class ItemPrice
    {
        public string amount { get; set; }
        public string useTypeID { get; set; }
        public string useType { get; set; }
    }

    public class ABSItem
    {
        public string Cost { get; set; }
        public string Status { get; set; }
        public string ProductName { get; set; }
        public string UPC { get; set; }
        public string CustomSKU { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Dimension { get; set; }
        public string MAP { get; set; }
        public string MSRP { get; set; }
        public string ItemID { get; set; }
        public string Inventory { get; set; }
        public string SystemID { get; set; }
    }

    public class ItemCreation
    {
        public string defaultCost { get; set; }
        public string description { get; set; }
        public string upc { get; set; }
        public string customSku { get; set; }
        public string manufacturerSku { get; set; }
        public string categoryID { get; set; }
        public string manufacturerID { get; set; }
        public Prices Prices { get; set; }
        public ItemVendorNums ItemVendorNums { get; set; }
    }

    public class RootObjectManufacturer
    {
        public Attributes attributes { get; set; }
        public List<Manufacturer> Manufacturer { get; set; }
    }

    public class RootObjectEmployee
    {
        public Attributes attributes { get; set; }
        public List<Employee> Employee { get; set; }
    }

    public class RootObjectCategory
    {
        public Attributes attributes { get; set; }
        public List<Category> Category { get; set; }
    }

    public class Employee
    {
        public string employeeID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string accessPin { get; set; }
        public string lockOut { get; set; }
        public string archived { get; set; }
        public string timeStamp { get; set; }
        public string contactID { get; set; }
        public string clockInEmployeeHoursID { get; set; }
        public string employeeRoleID { get; set; }
        public string limitToShopID { get; set; }
        public string lastShopID { get; set; }
        public string lastSaleID { get; set; }
        public string lastRegisterID { get; set; }
    }

    public class ItemVendorNum
    {
        public string itemVendorNumID { get; set; }
        public string value { get; set; }
        public string timeStamp { get; set; }
        public string itemID { get; set; }
        public string vendorID { get; set; }
    }

    public class ItemVendorNums
    {
        [JsonProperty("ItemVendorNum")]
        [JsonConverter(typeof(SingleOrArrayConverter<ItemVendorNum>))]
        public List<ItemVendorNum> ItemVendorNum { get; set; }
    }

    class ProductManagement
    {
        //public static void CreateAndUpdateProducts(List<StoreAuthentication> stores)
        //{
        //    int previousYear = DateTime.Today.AddYears(-1).Year;
        //    int currentYear = DateTime.Today.Year;

        //    APIManagement.UpdateManufacturerIDs(stores[0]);
        //    APIManagement.UpdateCategoryIDs(stores[0]);
        //    APIManagement.UpdateEmployeeIDs(stores[0]);

        //    APIManagement.GetLightspeedProductDetails(stores[0], stores);
        //    SQLManagement.GetProductDetailsFromABS(stores[0].table);

        //    List<ABSItem> LSR = SQLManagement.GetLSRUPCs(stores[0].table);
        //    List<ABSItem> ABS = SQLManagement.GetABSUPCs(stores[0].table);

        //    List<ABSItem> create = ProductManagement.CalculateProductsToCreate(LSR, ABS);
        //    List<ABSItem> update = ProductManagement.CalculateProductsToUpdate(LSR, ABS, stores[0].table);

        //    ProductManagement.CreateMissingLightspeedProducts(create, stores[0]);
        //    ProductManagement.UpdateCurrentLightspeedProducts(update, stores[0]);   
        //}

        //public static void CreateMissingLightspeedProducts(List<ABSItem> data, StoreAuthentication store)
        //{
        //    int counter = 1;
        //    Manufacturer m = SQLManagement.GetManufacturer("511", store.table);
        //    Dictionary<string, Category> c = SQLManagement.GetCategory(store.table);

        //    foreach (ABSItem entry in data)
        //    {
        //        List<ItemPrice> prices = new List<ItemPrice>();
        //        string tempDescription = "";

        //        prices.Add(new ItemPrice { amount = entry.MAP, useType = "Default", useTypeID = "1" });
        //        prices.Add(new ItemPrice { amount = entry.MAP, useType = "MSRP", useTypeID = "2" });

        //        tempDescription = entry.ProductName + " " + entry.CustomSKU.Split('-')[1] + " " + entry.Color + " " + entry.Size;
        //        tempDescription += entry.Dimension == "" ? "" : " " + entry.Dimension;

        //        ItemCreation post = new ItemCreation { defaultCost = entry.Cost, description = tempDescription, upc = entry.UPC, customSku = entry.CustomSKU, categoryID = c[entry.Category.Substring(0, 3)].categoryID, manufacturerID = m.manufacturerID, Prices = new Prices { ItemPrice = prices }, manufacturerSku = entry.ItemID };

        //        APIManagement.SendPOSTRequest(post, store, "", 0);

        //        Console.WriteLine("Timestamp: {2}. Created count: {0}. Item UPC: {1}", counter, post.upc, DateTime.Today.ToString("R"));
        //        counter++;
        //    }
        //}

        //public static void UpdateCurrentLightspeedProducts(List<ABSItem> data, StoreAuthentication store)
        //{
        //    int counter = 1;
        //    Manufacturer m = SQLManagement.GetManufacturer("511", store.table);
        //    Dictionary<string, Category> c = SQLManagement.GetCategory(store.table);

        //    foreach (ABSItem entry in data)
        //    {
        //        List<ItemPrice> prices = new List<ItemPrice>();
        //        string tempDescription = "";

        //        prices.Add(new ItemPrice { amount = entry.MAP, useType = "Default", useTypeID = "1" });
        //        prices.Add(new ItemPrice { amount = entry.MSRP, useType = "MSRP", useTypeID = "2" });

        //        tempDescription = entry.ProductName + " " + entry.Color + " " + entry.Size;
        //        tempDescription += entry.Dimension == "" ? "" : " " + entry.Dimension;

        //        ItemCreation put = new ItemCreation { defaultCost = entry.Cost, description = tempDescription, upc = entry.UPC, customSku = entry.CustomSKU, manufacturerSku = entry.CustomSKU.Split('-')[0], categoryID = c[entry.Category.Substring(0, 3)].categoryID, manufacturerID = m.manufacturerID, Prices = new Prices { ItemPrice = prices } };

        //        APIManagement.SendPUTRequest(put, store, entry.ItemID);

        //        Console.WriteLine("TimeStamp: {2}. Updated count: {0}. Item UPC: {1}.", counter, put.upc, DateTime.Now.ToString("R"));
        //        counter++;
        //    }
        //}

        //public static List<ABSItem> CalculateProductsToUpdate(List<ABSItem> LSR, List<ABSItem> ABS, string table)
        //{
        //    Dictionary<string, ABSItem> UPCs = new Dictionary<string, ABSItem>();
        //    List<ABSItem> update = new List<ABSItem>();

        //    foreach (ABSItem entry in LSR)
        //    {
        //        if (!UPCs.ContainsKey(entry.UPC))
        //        {
        //            UPCs.Add(entry.UPC, entry);
        //        }
        //    }

        //    foreach (ABSItem entry in ABS)
        //    {
        //        if (UPCs.ContainsKey(entry.UPC))
        //        {
        //            bool equal = HelperFunctions.CheckForEquality(entry, (ABSItem)UPCs[entry.UPC]);
        //            if (!equal)
        //            {
        //                ABSItem itemID = UPCs[entry.UPC];
        //                entry.ItemID = itemID.ItemID;
        //                update.Add(entry);
        //            }
        //        }
        //    }

        //    return update;
        //}

        //public static List<ABSItem> CalculateProductsToCreate(List<ABSItem> LSR, List<ABSItem> ABS)
        //{
        //    HashSet<string> UPCs = new HashSet<string>();
        //    List<ABSItem> create = new List<ABSItem>();

        //    foreach (ABSItem entry in LSR)
        //    {
        //        UPCs.Add(entry.UPC);
        //    }

        //    foreach (ABSItem entry in ABS)
        //    {
        //        if (!UPCs.Contains(entry.UPC) && entry.Status != "D")
        //        {
        //            create.Add(entry);
        //        }
        //    }

        //    return create;
        //}

        public static DataTable CreateLSRItem()
        {
            DataTable result = new DataTable();
            result.TableName = "Lightspeed_UPCs";
            result.Columns.Add("Cost", typeof(string));
            result.Columns.Add("Status", typeof(string));
            result.Columns.Add("ProductName", typeof(string));
            result.Columns.Add("UPC", typeof(string));
            result.Columns.Add("CustomSKU", typeof(string));
            result.Columns.Add("Category", typeof(string));
            result.Columns.Add("Manufacturer", typeof(string));
            result.Columns.Add("Size", typeof(string));
            result.Columns.Add("Color", typeof(string));
            result.Columns.Add("Dimension", typeof(string));
            result.Columns.Add("MAP", typeof(string));
            result.Columns.Add("MSRP", typeof(string));
            result.Columns.Add("ItemID", typeof(string));
            result.Columns.Add("Inventory", typeof(string));
            result.Columns.Add("SystemID", typeof(string));

            return result;
        }

        public static DataTable CreateLSRManufacturer()
        {
            DataTable result = new DataTable();
            result.TableName = "Lightspeed_ManufacturersTemp";
            result.Columns.Add("ManufacturerID", typeof(string));
            result.Columns.Add("Name", typeof(string));

            return result;
        }

        public static DataTable CreateLSREmployee()
        {
            DataTable result = new DataTable();
            result.TableName = "Lightspeed_EmployeesTemp";
            result.Columns.Add("EmployeeID", typeof(string));
            result.Columns.Add("FirstName", typeof(string));
            result.Columns.Add("LastName", typeof(string));

            return result;
        }

        public static DataTable CreateLSRCategory()
        {
            DataTable result = new DataTable();
            result.TableName = "Lightspeed_CategoriesTemp";
            result.Columns.Add("CategoryID", typeof(string));
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("FullPathName", typeof(string));

            return result;
        }
    }
}
