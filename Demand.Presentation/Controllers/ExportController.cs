using ClosedXML.Excel;
using Demand.Domain.ViewModels;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.IO;

public class ExportController : Controller
{
    [HttpPost]
    public IActionResult ExportToExcelAjax([FromBody] List<OfferRequestViewModel> data)
    {
        if (data == null || !data.Any())
        {
            return BadRequest("Veri bulunamadı!");
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Ürün Listesi");

            worksheet.Cell(1, 1).Value = "Ürün Kodu";
            worksheet.Cell(1, 2).Value = "Ürün Adı";
            worksheet.Cell(1, 3).Value = "Fiyat";

            var headerRange = worksheet.Range("A1:C1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var item in data.Where(x => !string.IsNullOrEmpty(x.ProductCode) || !string.IsNullOrEmpty(x.ProductName) || x.Price.IsNotNull()))
            {
                worksheet.Cell(row, 1).Value = item.ProductCode ?? "Boş";
                worksheet.Cell(row, 2).Value = item.ProductName ?? "Boş";
                worksheet.Cell(row, 3).Value = item.Price.IsNull() ? 0 : item.Price;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                var fileName = $"UrunListesi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}