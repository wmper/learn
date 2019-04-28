using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SDK.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Example.NPOI
{
    class Program
    {
        class ExcelTitle
        {
            /// <summary>
            /// 标题名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 数据字段名
            /// </summary>
            public string Field { get; set; }
            /// <summary>
            /// 单元格类型
            /// </summary>
            public CellType CellType { get; set; } = CellType.String;
            /// <summary>
            /// 单元格对齐方式
            /// </summary>
            public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;
        }

        class Data
        {
            public string ProductName { get; set; }
            public string No { get; set; }
            public decimal Price { get; set; }
            public int Qty { get; set; }
        }

        class QuartzTask : CommonEntity
        {
            public string TaskName { get; set; }
            public string ExcelTitle { get; set; }
            public string Data { get; set; }
            public string UploadFile { get; set; }
            public string DownloadFile { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var dir = $"{AppDomain.CurrentDomain.BaseDirectory}/temp/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var newFile = dir + $"{DateTime.Now.ToString("yyyymmddHHmmss")}.xlsx";

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();

                var sheet1 = workbook.CreateSheet("Sheet1");

                // title
                var title = sheet1.CreateRow(0);

                var titles = new List<ExcelTitle>() {
                    new ExcelTitle() { Name = "商品名称",Field="ProductName", CellType = CellType.String,Alignment = HorizontalAlignment.Left },
                    new ExcelTitle() { Name = "编号",Field="No", CellType = CellType.String },
                    new ExcelTitle() { Name = "价格",Field="Price", CellType = CellType.Numeric },
                    new ExcelTitle() { Name = "库存",Field="Qty", CellType = CellType.Numeric }
                };

                var styleCenter = workbook.CreateCellStyle();
                styleCenter.Alignment = HorizontalAlignment.Center;

                var styleLeft = workbook.CreateCellStyle();
                styleLeft.Alignment = HorizontalAlignment.Left;

                var styleRight = workbook.CreateCellStyle();
                styleRight.Alignment = HorizontalAlignment.Right;

                var i = 0;
                titles.ForEach(item =>
                {
                    var cell = title.CreateCell(i, CellType.String);
                    cell.CellStyle = styleCenter;
                    cell.SetCellValue(item.Name);

                    i++;
                });

                // data
                var temp = new List<Data>() {
                    new Data(){ ProductName="4可口可乐250ml*12瓶/箱",No="9900009308",Price=5,Qty=1},
                    new Data(){ ProductName="4可口可乐250ml*12瓶/箱",No="9900009308",Price=6,Qty=2},
                    new Data(){ ProductName="4可口可乐250ml*12瓶/箱",No="9900009308",Price=7,Qty=3},
                    new Data(){ ProductName="4可口可乐250ml*12瓶/箱",No="9900009308",Price=888888.89m,Qty=444444444}
                };

                var json = JsonConvert.SerializeObject(temp);


                var j = 1;
                var datas = JsonConvert.DeserializeObject<List<object>>(json);
                datas.ForEach(item =>
                {
                    var obj = JObject.Parse(item.ToString());
                    var row = sheet1.CreateRow(j);

                    for (var k = 0; k < titles.Count; k++)
                    {
                        var cellType = titles[k].CellType;
                        var algin = titles[k].Alignment;
                        var field = titles[k].Field;

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
                });

                workbook.Write(fs);
            }

            Console.WriteLine("End.");
        }
    }
}
