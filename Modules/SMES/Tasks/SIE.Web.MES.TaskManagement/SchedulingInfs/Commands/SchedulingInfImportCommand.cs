using Amazon.S3.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIE.Common;
using SIE.Common.ImportHelper;
using SIE.CSM.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.SchedulingInfs.Handles;
using SIE.Web.Common.Import;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellType = NPOI.SS.UserModel.CellType;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class SchedulingInfImportCommand : ImportCommandBase
    {

        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            if (importViewArgs.BehaviorName == "Download")
            {
                return DownloadTemplate();
            }
            else
            {
                return ImportData(importViewArgs);
            }
            //return base.Excute(importViewArgs, scope);
        }

        protected override object ImportData(ImportViewArgs args, string sheetName = "", bool isFirstRowColumn = true)
        {
            //new SpreadsheetHelper().LoadDocument(args.Data);
            //byte[] fileBytes = Convert.FromBase64String(args.Data);
            //using (var stream = new MemoryStream(fileBytes))
            //{
            //    IWorkbook workbook;

            //    // 根据文件类型创建工作簿
            //    if (args.Data.StartsWith("UEs")) // .xlsx 的Base64开头特征
            //        workbook = new XSSFWorkbook(stream);
            //    else
            //        workbook = new HSSFWorkbook(stream); // .xls

            //    ISheet sheet = workbook.GetSheetAt(0);
            //    IRow headerRow = sheet.GetRow(0);

            //}

            //return base.ImportData(args, sheetName, isFirstRowColumn);
            DataTable dataTable = GetDataTable(args.Data, sheetName);
            SchedulingInfImportDataHandleExt importDataHandleExt = new SchedulingInfImportDataHandleExt(args.SelectedParentId);
            string text = importDataHandleExt.ImportProcess(dataTable, GetImportHandleType(), GetImportCompleted());
            return new
            {
                ImportSuccessNum = importDataHandleExt.DrSuccess?.Length,
                ImportMsg = ((importDataHandleExt.DrSuccess == null && importDataHandleExt.DrFailed == null) ? text : "导入成功数据[{0}]条，失败数据[{1}]条".L10nFormat(importDataHandleExt.DrSuccess.Length, importDataHandleExt.DrFailed.Length)),
                FailedJson = GetFailedJsonStore(args.SelectedParentId, importDataHandleExt.DrFailed)
            };
        }


        protected override DataTable GetDataTable(string data, string sheetName)
        {
            //return base.GetDataTable(data, sheetName);
            bool isFirstRowColumn = true;
            DataTable dataTable = new DataTable();
            SpreadsheetHelper spreadsheetHelper = new SpreadsheetHelper();
            IWorkbook workbook = spreadsheetHelper.LoadDocument(data);
            ISheet sheet = (sheetName.IsNullOrEmpty() ? workbook.GetSheetAt(0) : workbook.GetSheet(sheetName));
            if (sheet == null)
            {
                return dataTable;
            }

            IRow row = sheet.GetRow(0);
            if (row == null)
            {
                throw new ValidationException("模板格式解析错误".L10N());
            }

            if (isFirstRowColumn)
            {
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    string text = row.GetCell(i).ToString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        DataColumn column = new DataColumn(text);
                        dataTable.Columns.Add(column);
                    }
                    else
                    {
                        //yyyyMMdd 为8位数，这里判断是否为日期
                        text = row.GetCell(i - 1).ToString();
                        if (text.Length == 8)
                        {
                            //必须加_1，因为不能列名
                            DataColumn column = new DataColumn(text + "_1");
                            dataTable.Columns.Add(column);
                        }
                    }
                }
            }

            for (int j = (isFirstRowColumn ? 1 : 0); j <= sheet.LastRowNum; j++)
            {
                IRow row2 = sheet.GetRow(j);
                if (row2 == null)
                {
                    continue;
                }

                DataRow dataRow = dataTable.NewRow();
                for (int k = 0; k < dataTable.Columns.Count; k++)
                {
                    if (k < row.LastCellNum)
                    {
                        ICell cell = row2.GetCell(k);
                        if (cell == null)
                        {
                            dataRow[k] = "";
                        }
                        else
                        {
                            dataRow[k] = GetCellValue(cell);
                        }
                    }
                }

                dataTable.Rows.Add(dataRow);
            }

            sheet = null;
            workbook = null;
            return dataTable;
        }

        private object GetCellValue(ICell cell)
        {
            object result = "";
            switch (cell.CellType)
            {
                case CellType.Blank:
                    result = "";
                    break;
                case CellType.Numeric:
                    {
                        short dataFormat = cell.CellStyle.DataFormat;
                        result = ((dataFormat != 14 && dataFormat != 31 && dataFormat != 57 && dataFormat != 58 && dataFormat != 20) ? ((object)cell.NumericCellValue) : cell.DateCellValue.ToString("yyyy/MM/dd"));
                        break;
                    }
                case CellType.String:
                    result = cell.StringCellValue;
                    break;
            }

            return result;
        }

        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        protected override Type GetImportHandleType()
        {
            return typeof(SchedulingInfImportHandle);
        }

        /// <summary>
        /// 直接从服务器上下载模板
        /// </summary>
        /// <returns></returns>
        public virtual object DownloadTemplate()
        {
            const string templateFileName = "MES排程中间表导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;

            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }


    }

    public class SchedulingInfImportDataHandleExt : ImportDataHandle
    {
        //
        // 摘要:
        //     父实体Id
        protected double ParentEntityId { get; set; }

        //
        // 摘要:
        //     构造函数
        //
        // 参数:
        //   parentId:
        //     父Id
        public SchedulingInfImportDataHandleExt(double parentId)
        {
            ParentEntityId = parentId;
        }

        //
        // 摘要:
        //     导入数据预处理
        //
        // 参数:
        //   importTable:
        //
        //   type:
        public override DataTable ProcessImportDataPre(DataTable importTable, Type type)
        {
            if (ParentEntityId == 0.0)
            {
                return importTable;
            }

            importTable.Columns.Add(ImportDataHandle.ParentId);
            importTable.Rows.Cast<DataRow>().ForEach(delegate (DataRow p)
            {
                p[ImportDataHandle.ParentId] = ParentEntityId;
            });
            return importTable;
        }

        protected override bool ValidColumnNames(DataTable dt, List<string> columnList)
        {
            var dtColumns = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                if (!column.ColumnName.IsNullOrEmpty())
                {
                    //将能够转换日期的去掉，只校验非日期(非动态列)那些列头
                    if (DateTime.TryParseExact(column.ColumnName, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime beginTime))
                    {
                        break;
                    } 
                    else
                        dtColumns.Add(column.ColumnName);
                }
                    
            }
            if (dtColumns.Count != columnList.Count)
                return false;
            return true;
        }

    }
}
