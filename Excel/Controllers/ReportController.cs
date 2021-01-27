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

namespace Excel.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Export");
        }


        [HttpGet("excel")]
        public IActionResult Export()
        {
            var excelPath = @"C:\Users\laishilin\Desktop\test.xlsx";
            using FileStream fs = new FileStream(excelPath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("Test");

            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));

            var rowIndex = 0;
            IRow row = sheet1.CreateRow(rowIndex);
            row.Height = 30 * 30;
            row.CreateCell(0).SetCellValue("this is content");
            sheet1.AutoSizeColumn(0);
            rowIndex++;

            var sheet2 = workbook.CreateSheet("Sheet2");
            var style1 = workbook.CreateCellStyle();
            style1.FillForegroundColor = HSSFColor.Blue.Index2;
            style1.FillPattern = FillPattern.SolidForeground;

            var style2 = workbook.CreateCellStyle();
            style2.FillForegroundColor = HSSFColor.Yellow.Index2;
            style2.FillPattern = FillPattern.SolidForeground;

            var cell2 = sheet2.CreateRow(0).CreateCell(0);
            cell2.CellStyle = style1;
            cell2.SetCellValue(0);

            cell2 = sheet2.CreateRow(1).CreateCell(0);
            cell2.CellStyle = style2;
            cell2.SetCellValue(1);

            workbook.Write(fs);

            return File(System.IO.File.OpenRead(@"C:\Users\laishilin\Desktop\test.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "导出数据.xlsx");
        }
    }
}
