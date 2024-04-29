using System;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml;

public class Save_Data
{
    private FileInfo excelFile;
	public Save_Data()
	{
        excelFile = new FileInfo(@"D:\Innovation_Tech_Challenge\data.xlsx"); // 設置 Excel 檔案的路徑
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage package = new ExcelPackage(excelFile);
        if (!excelFile.Exists)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            using (package)
            {
                worksheet.Cells["A1"].Value = "Vertical Rate";
                worksheet.Cells["B1"].Value = "Horizontal Rate";
                worksheet.Cells["C1"].Value = "Rotation Rate";
                worksheet.Cells["D1"].Value = "Duction Rate";
                worksheet.Cells["E1"].Value = "Flexion Rate";
                worksheet.Cells["F1"].Value = "Strength Amplitude";
                worksheet.Cells["G1"].Value = "Target Strength";
                package.Save();
            }
            
        }
    }
    public void add_item(List<object[]> items ,List<string> string_list)
    {   
        if (items.Count != string_list.Count) {
            Debug.WriteLine("Error");
            return;
        }
        for (int i = 0; i < items.Count; i++)
        {
            items[i][6] = float.Parse(string_list[i]);
        }
        ExcelPackage package = new ExcelPackage(excelFile);
        using (package)
        {   
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
            int lastrow = worksheet.Dimension.End.Row;
            // 在下一行新增資料
            worksheet.Cells[lastrow + 1, 1].LoadFromArrays(items);
            // 儲存 excel 檔案
            package.Save();
        }
    }
}
