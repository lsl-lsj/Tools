using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using Dapper;
using System.Text;
using EBook.Domain;

namespace Excel.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReportController : Controller
    {
        private readonly IDbContext _db;
        public ReportController(IDbContext db)
        {
            _db = db;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View("Export");
        }


        [HttpGet("excel")]
        public async Task<IActionResult> Export()
        {
            var excelPath = @"C:\Users\laishilin\Desktop\test.xlsx";
            using FileStream fs = new FileStream(excelPath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            using var conn = _db.GetConnection();
            var result = await conn.QueryAsync<EbookInfo>("SELECT * FROM ebook_info WHERE type = @Type", new { Type = BookType.Web });

            // var models = result.ListMapTo<EbookInfo, BookModel>();

            // ISheet sheet1 = workbook.CreateSheet("Test");

            // sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));

            // var rowIndex = 0;
            // IRow row = sheet1.CreateRow(rowIndex);
            // row.Height = 30 * 30;
            // row.CreateCell(0).SetCellValue("this is content");
            // sheet1.AutoSizeColumn(0);
            // rowIndex++;

            var sheet = workbook.CreateSheet("book");

            string[] titles = { "id", "number","name","type","author","publish","publish_date","created_on","created_by",
                                "price","score","url","discount","is_discount","modify_by","modified_on","download_times",
                                "description","is_deleted","actul_price" };

            ICell cell;
            IRow row;
            row = sheet.CreateRow(0);
            // bg-color:grey_25_percent;text-align:center;font-weight:bold;border-type:thin;
            var cellStyle = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.IsBold = true;
            // font.FontName = "宋体";
            // font.FontHeightInPoints = 12;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            // cellStyle.FillForegroundColor = HSSFColor.Grey50Percent.Index;
            // cellStyle.FillPattern = FillPattern.SolidForeground;
            // cellStyle.BottomBorderColor = HSSFColor.Black.Index;
            // cellStyle.LeftBorderColor = HSSFColor.Black.Index;
            // cellStyle.RightBorderColor = HSSFColor.Black.Index;
            // cellStyle.TopBorderColor = HSSFColor.Black.Index;
            // cellStyle.BorderBottom = BorderStyle.Double;
            // cellStyle.BorderRight = BorderStyle.Double;
            // cellStyle.BorderLeft = BorderStyle.Double;
            // cellStyle.BorderTop = BorderStyle.Double;
            cellStyle.SetFont(font);

            for (int i = 0; i < titles.Length; i++)
            {
                cell = row.CreateCell(i);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(titles[i]);
            }

            int indexRow = 1;
            foreach (var item in result)
            {
                row = sheet.CreateRow(indexRow);
                var props = item.GetType().GetProperties();
                int indexCol = 0;
                foreach (var item2 in props)
                {
                    cell = row.CreateCell(indexCol);
                    var o = item2.GetValue(item);
                    if (o == null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    else
                    {
                        cell.SetCellValue(o.ToString());
                    }
                    indexCol++;
                }
                indexRow++;
            }


            //设置自适应宽度
            for (int columnNum = 0; columnNum < titles.Length; columnNum++)
            {
                sheet.AutoSizeColumn(columnNum);
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                var num = sheet.LastRowNum;
                for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow = sheet.GetRow(rowNum);
                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, columnWidth * 240);
            }

            workbook.Write(fs);
            return Ok();

            // return File(System.IO.File.OpenRead(@"C:\Users\laishilin\Desktop\test.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "导出数据.xlsx");
        }
    }
}
