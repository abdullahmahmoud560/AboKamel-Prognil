#r "nuget: ClosedXML, 0.102.2"

using ClosedXML.Excel;
using System;

var excelPath = @"d:\Projects\Prognil\AboKamel\src\AboKamel.Api\SeedData\products.xlsx";
if (File.Exists(excelPath))
{
    Console.WriteLine("Excel file exists!");
    using var workbook = new XLWorkbook(excelPath);
    var worksheet = workbook.Worksheet(1);
    var firstRow = worksheet.Row(1);
    Console.WriteLine("Columns:");
    for (int i = 1; i <= 20; i++)
    {
        Console.WriteLine($"Cell {i}: {firstRow.Cell(i).GetValue<string>()}");
    }
    Console.WriteLine("\nFirst 5 data rows:");
    for (int r = 2; r <= 6; r++)
    {
        var row = worksheet.Row(r);
        Console.Write($"Row {r}: ");
        for (int c = 1; c <= 20; c++)
        {
            Console.Write($"[{c}:{row.Cell(c).GetValue<string>()}] ");
        }
        Console.WriteLine();
    }
}
else
{
    Console.WriteLine("Excel file NOT found!");
}
