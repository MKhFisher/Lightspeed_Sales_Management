using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    class SQLManagement
    {
        static SqlConnection boomi = new SqlConnection(@"Server=RECON\SQL2012;Database=BOOMI;User ID=sa;Password=#$Ql511#;MultipleActiveResultSets=true");
        static SqlConnection ecomm = new SqlConnection(@"Server=RECON\SQL2012;Database=ecomm;User ID=sa;Password=#$Ql511#;MultipleActiveResultSets=true");

        public static List<SalesLine> GetSalesFromStore(StoreAuthentication store, DateTime today)
        {
            List<SalesLine> result = new List<SalesLine>();
            OpenConnection(boomi);

            SqlCommand get = new SqlCommand("SELECT * FROM Lightspeed_Sales WHERE OrderDate = @orderDate AND AccountID = @accountID", boomi);
            get.CommandTimeout = 0;
            get.Parameters.AddWithValue("@orderDate", today.AddDays(-1).ToString("yyyyMMdd"));
            get.Parameters.AddWithValue("@accountID", store.accountID);
            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new SalesLine { ManufacturerName = dr["ManufacturerName"] == DBNull.Value ? "" : dr["ManufacturerName"].ToString(), ItemNumber = dr["ItemNumber"] == DBNull.Value ? "" : dr["ItemNumber"].ToString(), UPC = dr["UPC"] == DBNull.Value ? "" : dr["UPC"].ToString(), ItemName = dr["ItemName"] == DBNull.Value ? "" : dr["ItemName"].ToString(), QtySold = dr["QtySold"] == DBNull.Value ? "" : dr["QtySold"].ToString(), Cost = dr["Cost"] == DBNull.Value ? "" : dr["Cost"].ToString(), PricePerItem = dr["PricePerItem"] == DBNull.Value ? "" : dr["PricePerItem"].ToString(), DiscountedPrice = dr["DiscountedPrice"] == DBNull.Value ? "" : dr["DiscountedPrice"].ToString(), Total = dr["Total"] == DBNull.Value ? "" : dr["Total"].ToString(), OrderDate = dr["OrderDate"] == DBNull.Value ? "" : dr["OrderDate"].ToString(), OrderNumber = dr["OrderNumber"] == DBNull.Value ? "" : dr["OrderNumber"].ToString(), OrderTaker = dr["OrderTakerID"] == DBNull.Value ? "" : dr["OrderTakerID"].ToString(), ReturnReasons = dr["ReturnReasons"] == DBNull.Value ? "" : dr["ReturnReasons"].ToString() });
                }
            }

            return result;
        }

        public static void UpdateManufacturerName()
        {
            OpenConnection(boomi);

            SqlCommand updateJoin = new SqlCommand("UPDATE S set S.ManufacturerName = M.Name FROM Lightspeed_Sales S LEFT JOIN Lightspeed_Manufacturers_Fresno M ON S.ManufacturerName = M.ManufacturerID", boomi);
            updateJoin.CommandTimeout = 0;
            try
            {
                updateJoin.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                EmailManagement.SendEmail(new string[] { "michaelf@511tactical.com" }, "[ERROR] Couldn't update manufacturer name in sales table", e.Message.ToString(), null);
            }            

            CloseConnection(boomi);
        }

        public static DateTime GetLastSalesDate(StoreAuthentication store)
        {
            OpenConnection(boomi);
            DateTime result = new DateTime();
            string date = "00000000";

            SqlCommand get = new SqlCommand("SELECT TOP 1 OrderDate FROM Lightspeed_Sales WHERE AccountID = @accountID ORDER BY OrderDate DESC", boomi);
            get.CommandTimeout = 0;
            get.Parameters.AddWithValue("@accountID", store.accountID);
            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    date = dr["OrderDate"] == DBNull.Value ? "00000000" : dr["OrderDate"].ToString();
                }
            }

            if (date != "00000000")
            {
                result = new DateTime(Int32.Parse(date.Substring(0, 4)), Int32.Parse(date.Substring(4, 2)), Int32.Parse(date.Substring(6, 2)));
            }
            else
            {
                result = new DateTime(2000, 1, 1);
            }
            
            return result;
        }

        public static void DeleteLastDate(StoreAuthentication store, DateTime date)
        {
            OpenConnection(boomi);

            SqlCommand delete = new SqlCommand("DELETE FROM Lightspeed_Sales WHERE OrderDate >= @date AND AccountID = @accountID", boomi);
            delete.Parameters.AddWithValue("@date", date.ToString("yyyyMMdd"));
            delete.Parameters.AddWithValue("@accountID", store.accountID);
            delete.CommandTimeout = 0;

            try
            {
                delete.ExecuteNonQuery();
            }
            catch (Exception e) { }
        }

        public static void UpdateEmployeeName()
        {
            OpenConnection(boomi);

            SqlCommand updateJoin = new SqlCommand("UPDATE S set S.OrderTakerID = E.FirstName + ' ' + E.LastName FROM Lightspeed_Sales S LEFT JOIN Lightspeed_Employees E ON S.OrderTakerID = E.EmployeeID", boomi);
            updateJoin.CommandTimeout = 0;
            try
            {
                updateJoin.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                EmailManagement.SendEmail(new string[] { "michaelf@511tactical.com" }, "[ERROR] Couldn't update employee name in sales table", e.Message.ToString(), null);
            }

            CloseConnection(boomi);
        }

        public static List<ABSItem> GetLSRUPCs(string table)
        {
            OpenConnection(boomi);
            List<ABSItem> result = new List<ABSItem>();

            SqlCommand get = new SqlCommand("SELECT * FROM Lightspeed_UPCs_" + table, boomi);
            get.CommandTimeout = 0;
            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new ABSItem { Cost = dr["Cost"] == DBNull.Value ? "" : dr["Cost"].ToString(), Status = dr["Status"] == DBNull.Value ? "" : dr["Status"].ToString(), ProductName = dr["ProductName"] == DBNull.Value ? "" : dr["ProductName"].ToString(), UPC = dr["UPC"] == DBNull.Value ? "" : dr["UPC"].ToString(), CustomSKU = dr["CustomSKU"] == DBNull.Value ? "" : dr["CustomSKU"].ToString(), Category = dr["Category"] == DBNull.Value ? "" : dr["Category"].ToString(), Manufacturer = dr["Manufacturer"] == DBNull.Value ? "" : dr["Manufacturer"].ToString(), Size = dr["Size"] == DBNull.Value ? "" : dr["Size"].ToString(), Color = dr["Color"] == DBNull.Value ? "" : dr["Color"].ToString(), Dimension = dr["Dimension"] == DBNull.Value ? "" : dr["Dimension"].ToString(), MAP = dr["MAP"] == DBNull.Value ? "" : dr["MAP"].ToString(), MSRP = dr["MSRP"] == DBNull.Value ? "" : dr["MSRP"].ToString(), ItemID = dr["ItemID"] == DBNull.Value ? "" : dr["ItemID"].ToString(), Inventory = dr["Inventory"] == DBNull.Value ? "" : dr["Inventory"].ToString(), SystemID = dr["SystemID"] == DBNull.Value ? "" : dr["SystemID"].ToString() });
                }
            }

            CloseConnection(boomi);
            return result;
        }

        public static void GetProductDetailsFromABS(string name)
        {
            OpenConnection(boomi);

            TryDrop("Lightspeed_ABSProducts");
            SqlCommand getItems = null;

            if (name == "Irvine")
            {
                getItems = new SqlCommand("SELECT * INTO Lightspeed_ABSProducts_" + name + " FROM (SELECT LANDED_PRICE Cost, BK_STAT Status, SUBSTRING(LTRIM(RTRIM(MSTSHOVELB.DESCRIPTION)), 1, 25) ProductName, LTRIM(RTRIM(UPC)) UPC, LTRIM(RTRIM(MSTSHOVELB.STYLE)) + '-' + LTRIM(RTRIM(CLR)) + '-' + LTRIM(RTRIM(MSTSHOVELB.SIZE)) + CASE WHEN MSTSHOVELB.DIM <> '' THEN '-' + LTRIM(RTRIM(MSTSHOVELB.DIM)) ELSE '' END CustomSKU, SUBSTRING(CATEGORY, 1, CHARINDEX(':', CATEGORY) - 1) + ':' + LTRIM(RTRIM(SUBSTRING(CATEGORY, CHARINDEX(':', CATEGORY) + 1, 25))) + '>' + LTRIM(RTRIM(SUBSTRING(SUB_CATEGORY, CHARINDEX(':', SUB_CATEGORY) + 1, 25))) Category, '511' Manufacturer, LTRIM(RTRIM(MSTSHOVELB.size)) Size, LTRIM(RTRIM(SUBSTRING(MSTSHOVELB.COLOR, 5, 25))) Color, LTRIM(RTRIM(MSTSHOVELB.dim)) Dimension, MST_MSRP MAP, MST_MSRP MSRP FROM F11CUST.I511.F11CUST.MSTSHOVELB MSTSHOVELB WHERE CO = '11' AND DIV = '11' AND SEASON_CD = 'P' AND SEASON_YR = '10' AND CATEGORY NOT IN ('DON: 5.11 FUND RAISING', 'WRT: WEAR TEST', 'SMP: SAMPLES', 'PRO: PROMOS', 'POS: POS') AND BK_STAT <> 'D' AND UPC <> '') A WHERE Cost > 0.01 AND (MAP > 0.01 OR MSRP > 0.01)", boomi);
            }
            else
            {
                getItems = new SqlCommand("SELECT * INTO Lightspeed_ABSProducts_" + name + " FROM (SELECT LANDED_PRICE Cost, BK_STAT Status, SUBSTRING(LTRIM(RTRIM(MSTSHOVELB.DESCRIPTION)), 1, 25) ProductName, LTRIM(RTRIM(UPC)) UPC, LTRIM(RTRIM(MSTSHOVELB.STYLE)) + '-' + LTRIM(RTRIM(CLR)) + '-' + LTRIM(RTRIM(MSTSHOVELB.SIZE)) + CASE WHEN MSTSHOVELB.DIM <> '' THEN '-' + LTRIM(RTRIM(MSTSHOVELB.DIM)) ELSE '' END CustomSKU, SUBSTRING(CATEGORY, 1, CHARINDEX(':', CATEGORY) - 1) + ':' + LTRIM(RTRIM(SUBSTRING(CATEGORY, CHARINDEX(':', CATEGORY) + 1, 25))) + '>' + LTRIM(RTRIM(SUBSTRING(SUB_CATEGORY, CHARINDEX(':', SUB_CATEGORY) + 1, 25))) Category, '511' Manufacturer, LTRIM(RTRIM(MSTSHOVELB.size)) Size, LTRIM(RTRIM(SUBSTRING(MSTSHOVELB.COLOR, 5, 25))) Color, LTRIM(RTRIM(MSTSHOVELB.dim)) Dimension, MST_MAP MAP, MST_MAP MSRP FROM F11CUST.I511.F11CUST.MSTSHOVELB MSTSHOVELB WHERE CO = '11' AND DIV = '11' AND SEASON_CD = 'P' AND SEASON_YR = '10' AND CATEGORY NOT IN ('DON: 5.11 FUND RAISING', 'WRT: WEAR TEST', 'SMP: SAMPLES', 'PRO: PROMOS', 'POS: POS') AND BK_STAT <> 'D' AND UPC <> '') A WHERE Cost > 0.01 AND (MAP > 0.01 OR MSRP > 0.01)", boomi);
            }
            getItems.CommandTimeout = 0;
            try
            {
                getItems.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //Insert error.
            }

            CloseConnection(boomi);
        }

        public static Dictionary<string, Category> GetCategory(string table)
        {
            OpenConnection(boomi);

            Dictionary<string, Category> result = new Dictionary<string, Category>();
            SqlCommand get = new SqlCommand("SELECT * FROM Lightspeed_Categories_" + table, boomi);
            get.CommandTimeout = 0;

            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    if (dr["FullPathName"] == DBNull.Value ? "".Contains(":") : dr["FullPathName"].ToString().Contains(":") && !result.ContainsKey(dr["FullPathName"] == DBNull.Value ? "" : dr["FullPathName"].ToString().Substring(0, 3)))
                    {
                        result.Add(dr["FullPathName"] == DBNull.Value ? "" : dr["FullPathName"].ToString().Substring(0, 3), new Category { categoryID = dr["CategoryID"] == DBNull.Value ? "" : dr["CategoryID"].ToString(), fullPathName = dr["FullPathName"] == DBNull.Value ? "" : dr["FullPathName"].ToString() });
                    }
                }
            }

            return result;
        }

        public static List<ABSItem> GetABSUPCs(string table)
        {
            OpenConnection(boomi);
            List<ABSItem> result = new List<ABSItem>();

            SqlCommand get = new SqlCommand("SELECT * FROM Lightspeed_ABSProducts_" + table, boomi);
            get.CommandTimeout = 0;
            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(new ABSItem { Cost = dr["Cost"] == DBNull.Value ? "" : dr["Cost"].ToString(), Status = dr["Status"] == DBNull.Value ? "" : dr["Status"].ToString(), ProductName = dr["ProductName"] == DBNull.Value ? "" : dr["ProductName"].ToString(), UPC = dr["UPC"] == DBNull.Value ? "" : dr["UPC"].ToString(), CustomSKU = dr["CustomSKU"] == DBNull.Value ? "" : dr["CustomSKU"].ToString(), Category = dr["Category"] == DBNull.Value ? "" : dr["Category"].ToString(), Manufacturer = dr["Manufacturer"] == DBNull.Value ? "" : dr["Manufacturer"].ToString(), Size = dr["Size"] == DBNull.Value ? "" : dr["Size"].ToString(), Color = dr["Color"] == DBNull.Value ? "" : dr["Color"].ToString(), Dimension = dr["Dimension"] == DBNull.Value ? "" : dr["Dimension"].ToString(), MAP = dr["MAP"] == DBNull.Value ? "" : dr["MAP"].ToString(), MSRP = dr["MSRP"] == DBNull.Value ? "" : dr["MSRP"].ToString(), ItemID = dr["CustomSKU"] == DBNull.Value ? "" : dr["CustomSKU"].ToString().Split('-')[0] });
                }
            }

            CloseConnection(boomi);
            return result;
        }

        public static Manufacturer GetManufacturer(string name, string table)
        {
            OpenConnection(boomi);
            Manufacturer result = new Manufacturer();

            SqlCommand read = new SqlCommand("SELECT * FROM Lightspeed_Manufacturers_" + table + " WHERE Name = @name", boomi);
            read.Parameters.AddWithValue("@name", name);
            using (SqlDataReader dr = read.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.manufacturerID = dr["ManufacturerID"] == DBNull.Value ? "" : dr["ManufacturerID"].ToString();
                    result.name = dr["Name"] == DBNull.Value ? "" : dr["Name"].ToString();
                }
            }

            return result;
        }

        public static Dictionary<string, Shop> GetShopIDs()
        {
            OpenConnection(boomi);
            Dictionary<string, Shop> result = new Dictionary<string, Shop>();

            SqlCommand get = new SqlCommand("SELECT * FROM Lightspeed_Shops", boomi);
            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    try
                    {
                        result.Add(dr["AccountID"] == DBNull.Value ? "" : dr["AccountID"].ToString(), new Shop { shopID = dr["ShopID"] == DBNull.Value ? "" : dr["ShopID"].ToString(), name = dr["AccountID"] == DBNull.Value ? "" : dr["AccountID"].ToString() });
                    }
                    catch (Exception e)
                    {
                        //Insert error: Names are split on - character. Store name must have this character to be added to look up dictionary.
                    }
                }
            }

            return result;
        }

        public static void UpsertManufacturers(List<Manufacturer> manufacturers)
        {
            OpenConnection(boomi);
            TryTruncate("Lightspeed_ManufacturersTemp");

            DataTable manufacturersLSR = ProductManagement.CreateLSRManufacturer();
            foreach (Manufacturer m in manufacturers)
            {
                manufacturersLSR.Rows.Add(m.manufacturerID, m.name);
            }
            WriteToSQL(manufacturersLSR);

            MergeTables("Lightspeed_Manufacturers", "Lightspeed_ManufacturersTemp", new string[] { "ManufacturerID" }, new string[] { "Name" }, new string[] { "ManufacturerID", "Name" });

            TryTruncate("Lightspeed_ManufacturersTemp");
            CloseConnection(boomi);
        }

        public static void UpsertEmployees(List<Employee> employees)
        {
            OpenConnection(boomi);
            TryTruncate("Lightspeed_EmployeesTemp");

            DataTable employeesLSR = ProductManagement.CreateLSREmployee();
            foreach (Employee e in employees)
            {
                employeesLSR.Rows.Add(e.employeeID, e.firstName, e.lastName);
            }
            WriteToSQL(employeesLSR);

            MergeTables("Lightspeed_Employees", "Lightspeed_EmployeesTemp", new string[] { "EmployeeID" }, new string[] { "FirstName", "LastName" }, new string[] { "EmployeeID", "FirstName", "LastName" });

            TryTruncate("Lightspeed_EmployeesTemp");
            CloseConnection(boomi);
        }

        public static void UpsertCategories(List<Category> categories)
        {
            OpenConnection(boomi);
            TryTruncate("Lightspeed_CategoriesTemp");

            DataTable categoriesLSR = ProductManagement.CreateLSRCategory();
            foreach (Category c in categories)
            {
                categoriesLSR.Rows.Add(c.categoryID, c.name, c.fullPathName);
            }
            WriteToSQL(categoriesLSR);

            MergeTables("Lightspeed_Categories", "Lightspeed_CategoriesTemp", new string[] { "CategoryID" }, new string[] { "Name", "FullPathName" }, new string[] { "CategoryID", "Name", "FullPathName" });

            TryTruncate("Lightspeed_CategoriesTemp");
            CloseConnection(boomi);
        }

        public static void UpsertShops(List<Shop> shops)
        {
            OpenConnection(boomi);
            TryTruncate("Lightspeed_ShopsTemp");

            DataTable shopsLSR = SaleManagement.CreateLSRShop();
            foreach (Shop s in shops)
            {
                shopsLSR.Rows.Add(s.shopID, s.name.Split('-')[1].Trim());
            }
            WriteToSQL(shopsLSR);

            MergeTables("Lightspeed_Shops", "Lightspeed_ShopsTemp", new string[] { "ShopID" }, new string[] { "AccountID" }, new string[] { "ShopID, AccountID" });

            TryTruncate("Lightspeed_ShopsTemp");
            CloseConnection(boomi);
        }

        public static void UpsertSales(DataTable sales)
        {
            OpenConnection(boomi);
            TryTruncate("Lightspeed_SalesTemp");

            WriteToSQL(sales);

            SQLManagement.UpdateManufacturerName();
            SQLManagement.UpdateEmployeeName();

            MergeTables("Lightspeed_Sales", "Lightspeed_SalesTemp", new string[] { "SalesLineID", "AccountID" }, new string[] { "AccountID", "Store", "ManufacturerName", "ItemNumber", "UPC", "ItemName", "QtySold", "Cost", "PricePerItem", "DiscountedPrice", "Total", "OrderDate", "OrderNumber", "OrderTakerID", "ReturnReasons" }, new string[] { "AccountID", "Store", "SalesLineID", "ManufacturerName", "ItemNumber", "UPC", "ItemName", "QtySold", "Cost", "PricePerItem", "DiscountedPrice", "Total", "OrderDate", "OrderNumber", "OrderTakerID", "ReturnReasons" });

            TryTruncate("Lightspeed_SalesTemp");
            CloseConnection(boomi);
        }

        public static void PrepareQlikTable()
        {
            OpenConnection(boomi);
            TryDrop("QV_Lightspeed_Sales");
            CloseConnection(boomi);
        }

        public static void AddStoreToConsolidation()
        {
            OpenConnection(boomi);

            SqlCommand insert = new SqlCommand("SELECT * INTO QV_Lightspeed_Sales FROM Lightspeed_Sales", boomi);
            insert.CommandTimeout = 0;

            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception e) { }

            CloseConnection(boomi);
        }

        public static void AddIrvineOldToConsolidation()
        {
            OpenConnection(boomi);

            SqlCommand insert = new SqlCommand("INSERT INTO QV_Lightspeed_Sales SELECT * FROM Lightspeed_Sales_IrvineOld", boomi);
            insert.CommandTimeout = 0;
            
            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception e) { }

            CloseConnection(boomi);
        }

        public static void FormatConsolidatedTable()
        {
            OpenConnection(boomi);

            SqlCommand formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD Style varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD ProductID varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD Color varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD Size varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD Dimension varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD SizeDimension varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            formatTable = new SqlCommand("ALTER TABLE QV_Lightspeed_Sales ADD SizeChart varchar(50)", boomi);
            formatTable.ExecuteNonQuery();

            SqlCommand style = new SqlCommand("UPDATE Q SET Q.ProductID = LTRIM(RTRIM(S.STYLE)) + ': ' + LTRIM(RTRIM(S.DESCRIPTION)), Q.Style = LTRIM(RTRIM(S.STYLE)), Q.Color = LTRIM(RTRIM(S.COLORNAME)), Q.Size = LTRIM(RTRIM(S.SIZE)), Q.Dimension = LTRIM(RTRIM(S.DIM)), Q.SizeDimension = LTRIM(RTRIM(S.SIZE)) + ':' + LTRIM(RTRIM(S.DIM)), Q.SizeChart = S.SCALE_CD FROM QV_Lightspeed_Sales Q LEFT JOIN LINKEDABS.I511.F11CUST.MSTSHOVELB S ON Q.UPC = S.UPC LEFT JOIN (SELECT * FROM LINKEDABS.I511.ABS400F.MSTABLE WHERE TNO = '22' AND TKEY <> '') REF22 ON S.SCALE_CD = REF22.TKEY AND CO = TCO AND DIV = TDIV WHERE CODIV = '11:11' AND SEASON_CD = 'P' AND SEASON_YR = '10' AND S.UPC <> ''", boomi);
            style.CommandTimeout = 0;
            style.ExecuteNonQuery();

            CloseConnection(boomi);
        }

        public static void MergeTables(string table, string tempTable, string[] mergeOn, string[] update, string[] insertFields)
        {
            OpenConnection(boomi);

            string mergeString = "";
            foreach (string s in mergeOn)
            {
                mergeString += "Destination." + s + " = Source." + s + " AND ";
            }
            mergeString = mergeString.Trim();
            mergeString = mergeString.Substring(0, mergeString.Length - 4);

            string updateString = "";
            foreach (string s in update)
            {
                updateString += "Destination." + s + " = Source." + s + ", ";
            }
            updateString = updateString.Trim();
            updateString = updateString.Substring(0, updateString.Length - 1);

            string insertFieldHeaders = "";
            foreach (string s in insertFields)
            {
                insertFieldHeaders += s + ", ";
            }
            insertFieldHeaders = insertFieldHeaders.Substring(0, insertFieldHeaders.Length - 2);

            string insertFieldValues = "";
            foreach (string s in insertFields)
            {
                insertFieldValues += "Source." + s + ", ";
            }
            insertFieldValues = insertFieldValues.Substring(0, insertFieldValues.Length - 2);

            SqlCommand merge = new SqlCommand("MERGE " + table + " Destination USING " + tempTable + " Source ON " + mergeString + " WHEN MATCHED THEN UPDATE SET " + updateString + " WHEN NOT MATCHED BY TARGET THEN INSERT(" + insertFieldHeaders + ") VALUES (" + insertFieldValues + ");", boomi);
            try
            {
                merge.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                CloseConnection(boomi);
                EmailManagement.SendEmail(new string[] { "michaelf@511tactical.com" }, "[Error] Upsert Lightspeed Manufacturers", e.Message.ToString(), null);
            }

            CloseConnection(boomi);
        }

        public static int WriteToSQL(DataTable table)
        {
            OpenConnection(boomi);
            CreateSQLTableFromDataTable(table);
            CloseConnection(boomi);

            return 1;
        }

        private static void CreateSQLTableFromDataTable(DataTable table)
        {
            // checking whether the table selected from the dataset exists in the database or not
            string exists = null;
            string exists2 = "";

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM sysobjects where name = '" + table.TableName + "'", boomi);
                exists = cmd.ExecuteScalar().ToString();
            }
            catch (Exception exce)
            {
                exists = null;
            }

            if (table.TableName.Contains("Temp"))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM sysobjects where name = '" + table.TableName.Replace("Temp", "") + "'", boomi);
                    exists2 = cmd.ExecuteScalar().ToString();
                }
                catch (Exception exce)
                {
                    exists2 = null;
                }
            }

            // if does not exist
            if (exists == null)
            {
                // selecting each column of the datatable to create a table in the database
                foreach (DataColumn dc in table.Columns)
                {
                    if (exists == null)
                    {
                        SqlCommand createtable = new SqlCommand("CREATE TABLE " + table.TableName + " (" + dc.ColumnName + " varchar(MAX))", boomi);
                        createtable.ExecuteNonQuery();
                        exists = table.TableName;
                    }
                    else
                    {
                        SqlCommand addcolumn = new SqlCommand("ALTER TABLE " + table.TableName + " ADD " + dc.ColumnName + " varchar(MAX)", boomi);
                        addcolumn.ExecuteNonQuery();
                    }
                }

                // copying the data from datatable to database table
                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(boomi))
                {
                    bulkcopy.DestinationTableName = table.TableName;
                    bulkcopy.WriteToServer(table);
                }
            }
            // if table exists, just copy the data to the destination table in the database
            else
            {
                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(boomi))
                {
                    bulkcopy.DestinationTableName = table.TableName;
                    bulkcopy.WriteToServer(table);
                }
            }

            if (exists2 == null)
            {
                // selecting each column of the datatable to create a table in the database
                foreach (DataColumn dc in table.Columns)
                {
                    if (exists2 == null)
                    {
                        SqlCommand createtable = new SqlCommand("CREATE TABLE " + table.TableName.Replace("Temp", "") + " (" + dc.ColumnName + " varchar(MAX))", boomi);
                        createtable.ExecuteNonQuery();
                        exists2 = table.TableName.Replace("Temp", "");
                    }
                    else
                    {
                        SqlCommand addcolumn = new SqlCommand("ALTER TABLE " + table.TableName.Replace("Temp", "") + " ADD " + dc.ColumnName + " varchar(MAX)", boomi);
                        addcolumn.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void TruncateUPCsTable()
        {
            OpenConnection(boomi);

            SqlCommand truncate = new SqlCommand("TRUNCATE TABLE Lightspeed_UPCs", boomi);
            try
            {
                truncate.ExecuteNonQuery();
            }
            catch { }

            CloseConnection(boomi);
        }

        public static string RetrievePassword(string username)
        {
            OpenConnection(ecomm);
            string result = "";

            SqlCommand get = new SqlCommand("SELECT * FROM users WHERE username = @username", ecomm);
            get.Parameters.AddWithValue("@username", username);

            using (SqlDataReader dr = get.ExecuteReader())
            {
                while (dr.Read())
                {
                    result += dr.GetString(4);
                }
            }

            CloseConnection(ecomm);
            return result;
        }

        private static void OpenConnection(SqlConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch { }
        }

        private static void CloseConnection(SqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch { }
        }

        private static void TryDrop(string table)
        {
            OpenConnection(boomi);

            SqlCommand drop = new SqlCommand("DROP TABLE " + table, boomi);
            try
            {
                drop.ExecuteNonQuery();
            }
            catch (Exception e) { }
        }

        private static void TryTruncate(string table)
        {
            OpenConnection(boomi);

            SqlCommand truncate = new SqlCommand("TRUNCATE TABLE " + table, boomi);
            try
            {
                truncate.ExecuteNonQuery();
            }
            catch (Exception e) { }
            finally
            {
                CloseConnection(boomi);
            }
        }
    }
}
