using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Lightspeed_Product_Management
{
    class SalesLine
    {
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
        public string OrderTaker { get; set; }
        public string ReturnReasons { get; set; }
    }

    class ExcelManagement
    {
        public static void CreateDailySalesReport(string directory, StoreAuthentication store, DateTime today)
        {
            Excel.Application _Excel = new Excel.Application();
            _Excel.DisplayAlerts = false;
            _Excel.Visible = false;

            Excel.Workbook wb = _Excel.Workbooks.Add();
            Excel.Worksheet ws = wb.Worksheets.get_Item(1);

            int row = 1;

            ApplyDailySalesReportHeaders(ws);
            row++;
            SetFormat(ws, new string[] { "Text", "Text", "Text", "Text", "Int", "Dollar", "Dollar", "Dollar", "Dollar", "Text", "Text", "Text", "Text" });

            List<SalesLine> sales = SQLManagement.GetSalesFromStore(store, today);
            foreach (SalesLine sale in sales)
            {
                ws.Cells[row, 1] = sale.ManufacturerName;
                ws.Cells[row, 2] = sale.ItemNumber;
                ws.Cells[row, 3] = sale.UPC;
                ws.Cells[row, 4] = sale.ItemName;
                ws.Cells[row, 5] = sale.QtySold;
                ws.Cells[row, 6] = sale.Cost;
                ws.Cells[row, 7] = sale.PricePerItem;
                ws.Cells[row, 8] = sale.DiscountedPrice;
                ws.Cells[row, 9] = sale.Total;
                ws.Cells[row, 10] = sale.OrderDate;
                ws.Cells[row, 11] = sale.OrderNumber;
                ws.Cells[row, 12] = sale.OrderTaker;
                ws.Cells[row, 13] = sale.ReturnReasons;
                row++;
            }

            string file = directory + "F11POS" + store.store + "-" + today.ToString("yyyyMMdd") + ".csv";

            wb.SaveAs(file, Excel.XlFileFormat.xlCSV);
            wb.Close();
            ExcelQuit(_Excel, wb, ws);

            //FTPManagement.DeliverFTPFile(new Parameters { FilePath = file, Destinations = new List<Destination> { new Destination { host = "", login = "", password = "" } } });
        }

        private static void SetFormat(Excel.Worksheet ws, string[] formats)
        {
            for (int i = 0; i < formats.Length; i++)
            {
                switch(formats[i])
                {
                    case "Text":
                        ws.Cells[1, i + 1].EntireColumn.NumberFormat = "@";
                        break;
                    case "Int":
                        ws.Cells[1, i + 1].EntireColumn.NumberFormat = "0";
                        break;
                    case "Dollar":
                        ws.Cells[1, i + 1].EntireColumn.NumberFormat = "###0.00";
                        break;
                }
            }
        }

        public static void CreateProductDeltaSpreadsheet(string file, List<ABSItem> LSR)
        {
            Excel.Application _Excel = new Excel.Application();
            _Excel.DisplayAlerts = false;
            _Excel.Visible = false;

            Excel.Workbook wb = _Excel.Workbooks.Add();
            Excel.Worksheet ws = wb.Worksheets.get_Item(1);

            int row = 1;

            ApplyProductDeltaHeaders(ws);
            row++;

            foreach (ABSItem entry in LSR)
            {
                ws.Cells[row, 1] = entry.UPC;
                ws.Cells[row, 2] = entry.ProductName;
                ws.Cells[row, 3] = entry.MSRP == "0" ? entry.MAP : entry.MSRP;
                ws.Cells[row, 4] = entry.Cost;
                ws.Cells[row, 5] = entry.MSRP == "0" ? entry.MAP : entry.MSRP;
                ws.Cells[row, 6] = entry.Inventory;
                row++;
            }

            ws.Cells[1, 1].EntireColumn.NumberFormat = "0";
            wb.SaveAs(file, Excel.XlFileFormat.xlCSV);
            ExcelQuit(_Excel, wb, ws);
        }

        private static void ExcelQuit(Excel.Application _Excel, Excel.Workbook wb, Excel.Worksheet ws)
        {
            if (ws != null)
            {
                Marshal.ReleaseComObject(ws);
                ws = null;
            }

            if (wb != null)
            {
                try
                {
                    wb.Close();
                }
                catch { }

                Marshal.ReleaseComObject(wb);
                wb = null;
            }

            if (_Excel != null)
            {
                try
                {
                    _Excel.Quit();
                }
                catch { }

                Marshal.ReleaseComObject(_Excel);
                _Excel = null;
            }
        }

        private static void ApplyProductDeltaHeaders(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "UPC";
            ws.Cells[1, 2] = "Description";
            ws.Cells[1, 3] = "MSRP";
            ws.Cells[1, 4] = "Default Cost";
            ws.Cells[1, 5] = "Price";
            ws.Cells[1, 6] = "Quantity on Hand";
        }

        private static void ApplyDailySalesReportHeaders(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "ManufacturerName";
            ws.Cells[1, 2] = "ItemNumber";
            ws.Cells[1, 3] = "UPC";
            ws.Cells[1, 4] = "ItemName";
            ws.Cells[1, 5] = "QtySold";
            ws.Cells[1, 6] = "Cost";
            ws.Cells[1, 7] = "PricePerItem";
            ws.Cells[1, 8] = "DiscountedPrice";
            ws.Cells[1, 9] = "Total";
            ws.Cells[1, 10] = "OrderDate";
            ws.Cells[1, 11] = "OrderNumber";
            ws.Cells[1, 12] = "OrderTaker";
            ws.Cells[1, 13] = "ReturnReasons";
        }
    }
}
