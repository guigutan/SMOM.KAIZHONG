using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace SIE.ESop.Uilts
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// 获取Excel页签名称
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>页签名称数组</returns>
        public static string[] ExcelSheetNames(string fileName)
        {
            IWorkbook workbook = null;
            List<string> names = new List<string>();
            if(string.IsNullOrWhiteSpace(fileName))
            {
                return names.ToArray();
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                if (fileName.IndexOf(".xlsx") > -1) // 2007版本
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls") > -1) // 2003版本
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    //
                }

                if (workbook != null)
                {
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        names.Add(workbook.GetSheetName(i));
                    }
                }
                fs.Close();
            }
                return names.ToArray();
        }

        /// <summary>
        /// 读取Excel页签数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheetName">页签名称</param>
        /// <param name="startRowNum">开始行号</param>
        /// <param name="startColNum">开始列号</param>
        /// <param name="endRowNum">结束行号</param>
        /// <param name="endColNum">结束列号</param>
        /// <returns>数据集合</returns>
        public static IList<string> ReadSheetValues(string fileName, string sheetName, int startRowNum, int startColNum, int? endRowNum, int? endColNum)
        {
            IWorkbook workbook = null;
            List<string> values = new List<string>();
            if(string.IsNullOrWhiteSpace(fileName))
            {
                return values;
            }
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (fileName.IndexOf(".xlsx") > -1) // 2007版本
            {
                workbook = new XSSFWorkbook(fs);
            }
            else if (fileName.IndexOf(".xls") > -1) // 2003版本
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                //
            }

            if (workbook != null)
            {
                var sheet = workbook.GetSheet(sheetName);
                if (sheet != null)
                {
                    for (int i = startRowNum - 1; i < (endRowNum ?? sheet.LastRowNum + 1); i++)
                    {
                        var row = sheet.GetRow(i);
                        for (int j = startColNum - 1; j < (endColNum ?? row.LastCellNum + 1); j++)
                        {
                            if (row.Cells[j].CellType == CellType.String)
                            {
                                values.Add(row.Cells[j].StringCellValue);
                            }

                            if (row.Cells[j].CellType == CellType.Numeric)
                            {
                                values.Add(row.Cells[j].NumericCellValue.ToString());
                            }
                        }
                    }
                }
            }
            fs.Close();
            return values;
        }
    }
}