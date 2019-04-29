using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SDK.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace Example.NPOI
{
    class Program
    {
        public class Data
        {
            public string ProductName { get; set; }
            public string No { get; set; }
            public decimal Price { get; set; }
            public int Qty { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var dir = $"{AppDomain.CurrentDomain.BaseDirectory}/temp/";

            var headers = new List<ExcelHeader>() {
                    new ExcelHeader() { Name = "商品名称",Field="ProductName", CellType = CellType.String,Alignment = HorizontalAlignment.Left },
                    new ExcelHeader() { Name = "编号",Field="No", CellType = CellType.String },
                    new ExcelHeader() { Name = "价格",Field="Price", CellType = CellType.Numeric },
                    new ExcelHeader() { Name = "库存",Field="Qty", CellType = CellType.Numeric }
                };

            // data
            //var temps = new List<Data>();
            //for (var i = 0; i < 10000; i++)
            //{
            //    temps.Add(new Data() { ProductName = $"4可口可乐250ml*12瓶/箱-{i}", No = "9900009308", Price = 5, Qty = 1 });
            //}

            //var json = JsonConvert.SerializeObject(temps);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //CreateExcel(dir, headers, json);

            var sheet = GetSheet("http://127.0.0.1:8887/eb80440cfc6e46bfaaaa87a92734dc4e.xlsx", EUploadType.Remote);

            var list = GetSheetData<IList<Data>>(sheet, headers);
            Console.WriteLine(list.Count);

            stopwatch.Stop();

            Console.WriteLine($"End.{stopwatch.ElapsedMilliseconds}");
        }

        private static void CreateExcel(string dir, IList<ExcelHeader> headers, string jsonData)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var newFile = dir + $"{Guid.NewGuid().ToN()}.xlsx";

            IWorkbook workbook = new XSSFWorkbook();

            var sheet1 = workbook.CreateSheet("Sheet1");

            // title
            var title = sheet1.CreateRow(0);

            var styleCenter = workbook.CreateCellStyle();
            styleCenter.Alignment = HorizontalAlignment.Center;

            var styleLeft = workbook.CreateCellStyle();
            styleLeft.Alignment = HorizontalAlignment.Left;

            var styleRight = workbook.CreateCellStyle();
            styleRight.Alignment = HorizontalAlignment.Right;

            var i = 0;
            foreach (var item in headers)
            {
                var cell = title.CreateCell(i, CellType.String);
                cell.CellStyle = styleCenter;
                cell.SetCellValue(item.Name);

                i++;
            }

            var j = 1;
            var datas = JsonConvert.DeserializeObject<IList<object>>(jsonData);
            foreach (var item in datas)
            {
                var obj = JObject.Parse(item.ToString());
                var row = sheet1.CreateRow(j);

                for (var k = 0; k < headers.Count; k++)
                {
                    var cellType = headers[k].CellType;
                    var algin = headers[k].Alignment;
                    var field = headers[k].Field;

                    var cell = row.CreateCell(k, cellType);

                    switch (algin)
                    {
                        case HorizontalAlignment.Left:
                            cell.CellStyle = styleLeft;
                            break;
                        case HorizontalAlignment.Right:
                            cell.CellStyle = styleRight;
                            break;
                        default:
                            cell.CellStyle = styleCenter;
                            break;
                    }

                    switch (cellType)
                    {
                        case CellType.Numeric:
                            cell.SetCellValue(double.Parse(obj[field].ToString()));
                            break;
                        default:
                            cell.SetCellValue(obj[field].ToString());
                            break;
                    }
                }

                j++;
            };

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
        private static string GetSheetJson(ISheet sheet, IList<ExcelHeader> headers, int skipHeaderRow = 1, int skipFlooterRow = 0)
        {
            var list = new List<Dictionary<string, string>>();
            var rowCount = sheet.LastRowNum - skipFlooterRow;

            for (var i = skipHeaderRow; i <= rowCount; i++)
            {
                var dic = new Dictionary<string, string>();
                var cells = sheet.GetRow(i).Cells;
                var j = 0;

                foreach (var item in headers)
                {
                    dic.Add(item.Field, cells[j].ToString());
                    j++;
                }

                list.Add(dic);
            }

            return JsonConvert.SerializeObject(list);
        }

        private static T GetSheetData<T>(ISheet sheet, IList<ExcelHeader> headers, int skipHeaderRow = 1, int skipFlooterRow = 0)
        {
            var json = GetSheetJson(sheet, headers, skipHeaderRow, skipFlooterRow);

            return JsonConvert.DeserializeObject<T>(json);
        }

        private static ISheet GetSheet(string pathOrUrl, EUploadType eUploadType)
        {
            if (pathOrUrl.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(pathOrUrl));
            }

            if (!pathOrUrl.EndsWith("xlsx") && !pathOrUrl.EndsWith("xls"))
            {
                throw new ArgumentNullException("error excel file.");
            }

            var memoryStream = new MemoryStream();

            if (eUploadType == EUploadType.Remote)
            {
                var client = new HttpClient();
                using (var stream = client.GetStreamAsync(pathOrUrl).Result)
                {
                    stream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }
            }
            else
            {
                using (var fs = new FileStream(pathOrUrl, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }
            }

            ISheet sheet = null;

            if (pathOrUrl.EndsWith("xlsx"))
            {
                var workBook = new XSSFWorkbook(memoryStream);
                sheet = workBook.GetSheetAt(0);
            }
            else
            {
                // 2003版 excel
                var workBook = new HSSFWorkbook(memoryStream);
                sheet = workBook.GetSheetAt(0);
            }

            return sheet;
        }
    }
}
