using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using SIE.Common.Employees;
using SIE.Domain;
using SIE.AbnormalInfo.Common;
using DevExpress.Charts.Native;
using NPOI.HSSF.Util;
using System.Globalization;
using SIE.MetaModel;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    /// <summary>
    /// 导入导出共用方法
    /// </summary>
    public class ExportCommandExcelHelper
    {
        /// <summary>
        /// 导出数据至Excel
        /// </summary>        
        /// <param name="indexData">数据</param>
        public virtual MemoryStream ExportData(Dictionary<string, string> indexData)
        {
            MemoryStream ms = new MemoryStream();    //创建内存流用于写入文件
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("异常任务");  //创建工作表 
            workbook.SetSelectedTab(0);
            workbook.SetActiveSheet(0);

            if (sheet1 != null)
            {
                OtherHeadingsFill(sheet1, indexData, workbook);
            }
            workbook.Write(ms);  //将Excel写入流
            return ms;
        }

        /// <summary>
        /// 表头表尾的填充
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="mergedStyle"></param>
        /// <param name="indexRowIndex"></param>
        /// <param name="mainTableInformation"></param>
        public void OtherHeadingsFill(ISheet sheet1, Dictionary<string, string> indexData, IWorkbook workbook)
        {
            IRow row = null;
            int rowCount = 0;
            (ICellStyle descriptionTitleStyle, ICellStyle bigInputFieldStyle, ICellStyle titleStyle, ICellStyle bigTitleStyle, ICellStyle mergedStyle) = CreateStyles(workbook);
            #region 初始化表格
            for (int i = 0; i < 50; i++)
            {
                row = sheet1.CreateRow(i);  //在工作表中添加一行
                for (var j = 0; j < 20; j++)
                    row.CreateCell(j);
            }
            for (int i = 2; i < 15; i++)
            {
                sheet1.SetColumnWidth(i, 15 * 256); // 设置初始列宽，可根据需要进行调整
            }
            #endregion
            #region 表头(异常信息报告单)
            row = sheet1.GetRow(0);
            row.Cells[0].SetCellValue("异常信息报告单");
            row = sheet1.GetRow(0);
            row.Cells[0].CellStyle = bigTitleStyle;
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, 9));
            rowCount = 2;
            #endregion
            rowCount += 2;
            SetTaskState(rowCount, sheet1, indexData, workbook, mergedStyle);
            SetTaskField(sheet1, row, rowCount, indexData, titleStyle, mergedStyle);
            rowCount = SetCounterplanRow(rowCount, sheet1, descriptionTitleStyle, bigInputFieldStyle, "异常描述：", indexData["ProblemDescription"]);
            rowCount = SetCounterplanRow(rowCount, sheet1, descriptionTitleStyle, bigInputFieldStyle, "临时对策：", indexData["TempMeasures"]);
            SetCounterplanRow(rowCount, sheet1, descriptionTitleStyle, bigInputFieldStyle, "长期对策：", indexData["LongMeasures"]);

            //自适应单元格宽度
            for (int i = 0; i < 15; i++)
            {
                sheet1.AutoSizeColumn(i);
            }
        }

        /// <summary>
        /// 设置任务单状态
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="sheet1"></param>
        /// <param name="indexData"></param>
        /// <param name="workbook"></param>
        /// <param name="mergedStyle"></param>
        public void SetTaskState(int rowCount, ISheet sheet1, Dictionary<string, string> indexData, IWorkbook workbook, ICellStyle mergedStyle)
        {
            IRow row = sheet1.GetRow(rowCount);
            TaskStateEnum taskState = GetTaskStateEnum(indexData["TaskState"]);
            row.Cells[0].SetCellValue(taskState.ToLabel());
            ICellStyle taskStateMergedStyle = workbook.CreateCellStyle();
            taskStateMergedStyle.CloneStyleFrom(mergedStyle);
            //根据任务状态设置颜色
            if (taskState == TaskStateEnum.ToDo)
            {
                taskStateMergedStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            }
            else if (taskState == TaskStateEnum.Doing)
            {
                taskStateMergedStyle.FillForegroundColor = IndexedColors.Blue.Index;
            }
            else if (taskState == TaskStateEnum.Done)
            {
                taskStateMergedStyle.FillForegroundColor = IndexedColors.Green.Index;
            }
            //else if (taskState == TaskStateEnum.Upgrade)
            //{
            //    taskStateMergedStyle.FillForegroundColor = IndexedColors.Yellow.Index;
            //}
            else if (taskState == TaskStateEnum.Cancel)
            {
                taskStateMergedStyle.FillForegroundColor = IndexedColors.Grey50Percent.Index;
            }
            //if (taskState != TaskStateEnum.Upgrade)
            //{
            var font = workbook.CreateFont();
            font.Color = HSSFColor.White.Index; // 设置字体颜色为白色
            taskStateMergedStyle.SetFont(font);
            //}
            taskStateMergedStyle.FillPattern = FillPattern.SolidForeground;
            //设置任务状态的每个单元格样式
            for (int i = 0; i < 2; i++)
            {
                IRow row1 = sheet1.GetRow(rowCount);
                row1.Cells[i].CellStyle = taskStateMergedStyle;
                IRow row2 = sheet1.GetRow(rowCount + 1);
                row2.Cells[i].CellStyle = taskStateMergedStyle;
                IRow row3 = sheet1.GetRow(rowCount + 2);
                row3.Cells[i].CellStyle = taskStateMergedStyle;
                IRow row4 = sheet1.GetRow(rowCount + 3);
                row4.Cells[i].CellStyle = taskStateMergedStyle;
            }
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 3, 0, 1));
        }

        /// <summary>
        /// 设置异常描述、临时对策、长期对策
        /// </summary>
        /// <returns></returns>
        public int SetCounterplanRow(int rowCount, ISheet sheet1, ICellStyle descriptionTitleStyle, ICellStyle bigInputFieldStyle, string title, string value)
        {
            rowCount = rowCount + 4;
            IRow row = sheet1.GetRow(++rowCount);
            row.Cells[0].SetCellValue(title);
            row.Cells[0].CellStyle = descriptionTitleStyle;
            row = sheet1.GetRow(++rowCount);
            row.Cells[0].SetCellValue(value);
            SetbigInputFieldStyle(sheet1, rowCount, bigInputFieldStyle);
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 3, 0, 9));
            return rowCount;
        }

        /// <summary>
        /// 设置异常描述、临时对策、长期对策内容框样式
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="rowCount"></param>
        /// <param name="bigInputFieldStyle"></param>
        public void SetbigInputFieldStyle(ISheet sheet1, int rowCount, ICellStyle bigInputFieldStyle)
        {
            for (int i = 0; i < 10; i++)
            {
                IRow row1 = sheet1.GetRow(rowCount);
                row1.Cells[i].CellStyle = bigInputFieldStyle;
                IRow row2 = sheet1.GetRow(rowCount + 1);
                row2.Cells[i].CellStyle = bigInputFieldStyle;
                IRow row3 = sheet1.GetRow(rowCount + 2);
                row3.Cells[i].CellStyle = bigInputFieldStyle;
                IRow row4 = sheet1.GetRow(rowCount + 3);
                row4.Cells[i].CellStyle = bigInputFieldStyle;
            }
        }

        /// <summary>
        /// 表格所用到的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="mergedStyle"></param>
        /// <returns></returns>
        public (ICellStyle, ICellStyle, ICellStyle, ICellStyle, ICellStyle) CreateStyles(IWorkbook workbook)
        {
            ICellStyle mergedStyle = workbook.CreateCellStyle();
            mergedStyle.VerticalAlignment = VerticalAlignment.Center;
            mergedStyle.Alignment = HorizontalAlignment.Center;
            mergedStyle.BottomBorderColor = 0;
            mergedStyle.WrapText = true;
            mergedStyle.BorderBottom = BorderStyle.Thin;
            mergedStyle.BorderLeft = BorderStyle.Thin;
            mergedStyle.BorderRight = BorderStyle.Thin;
            mergedStyle.BorderTop = BorderStyle.Thin;

            ICellStyle descriptionTitleStyle = workbook.CreateCellStyle();
            IFont descriptionTitleFont = workbook.CreateFont();
            descriptionTitleFont.FontHeightInPoints = 14;
            descriptionTitleStyle.SetFont(descriptionTitleFont);

            ICellStyle bigInputFieldStyle = workbook.CreateCellStyle();
            bigInputFieldStyle.CloneStyleFrom(mergedStyle);
            bigInputFieldStyle.Alignment = HorizontalAlignment.Left;
            bigInputFieldStyle.VerticalAlignment = VerticalAlignment.Top;

            ICellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.CloneStyleFrom(mergedStyle);
            titleStyle.Alignment = HorizontalAlignment.Right;

            ICellStyle bigTitleStyle = workbook.CreateCellStyle();
            bigTitleStyle.CloneStyleFrom(mergedStyle);
            IFont bigTitleFont = workbook.CreateFont();
            bigTitleFont.FontHeightInPoints = 20;
            bigTitleStyle.SetFont(bigTitleFont);

            return (descriptionTitleStyle, bigInputFieldStyle, titleStyle, bigTitleStyle, mergedStyle);
        }


        /// <summary>
        /// 异常信息报告单的表单内容
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="row"></param>
        /// <param name="rowCount"></param>
        /// <param name="indexData"></param>
        /// <param name="titleStyle"></param>
        /// <param name="mergedStyle"></param>
        public void SetTaskField(ISheet sheet1, IRow row, int rowCount, Dictionary<string, string> indexData, ICellStyle titleStyle, ICellStyle mergedStyle)
        {

            row = sheet1.GetRow(rowCount);
            row.Cells[2].SetCellValue("异常任务编码：");
            row.Cells[3].SetCellValue(indexData["Code"]);
            row.Cells[2].CellStyle = titleStyle;
            row.Cells[3].CellStyle = mergedStyle;
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, 2, 2));
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, 3, 3));

            row.Cells[5].SetCellValue("异常定义：");
            row.Cells[6].SetCellValue(indexData["AbnormalDefineName"]);
            row.Cells[5].CellStyle = titleStyle;
            row.Cells[6].CellStyle = mergedStyle;
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, 5, 5));
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, 6, 6));

            string dateTime = DateTime.ParseExact(indexData["CreateDate"], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            row.Cells[8].SetCellValue("发生时间：");
            row.Cells[9].SetCellValue(dateTime);
            row.Cells[8].CellStyle = titleStyle;
            row.Cells[9].CellStyle = mergedStyle;
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, 8, 8));
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount + 1, 9, 9));

            row = sheet1.GetRow(rowCount + 1);
            row.Cells[2].CellStyle = titleStyle;
            row.Cells[3].CellStyle = mergedStyle;
            row.Cells[5].CellStyle = titleStyle;
            row.Cells[6].CellStyle = mergedStyle;
            row.Cells[8].CellStyle = titleStyle;
            row.Cells[9].CellStyle = mergedStyle;

            row = sheet1.GetRow(rowCount + 2);
            row.Cells[2].SetCellValue("产线：");
            row.Cells[3].SetCellValue(indexData["LineName"]);
            row.Cells[2].CellStyle = titleStyle;
            row.Cells[3].CellStyle = mergedStyle;
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount + 2, rowCount + 3, 2, 2));
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount + 2, rowCount + 3, 3, 3));

            row.Cells[5].SetCellValue("车间：");
            row.Cells[6].SetCellValue(indexData["WorkShopName"]);
            row.Cells[5].CellStyle = titleStyle;
            row.Cells[6].CellStyle = mergedStyle;
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount + 2, rowCount + 3, 5, 5));
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount + 2, rowCount + 3, 6, 6));
            row = sheet1.GetRow(rowCount + 3);
            row.Cells[2].CellStyle = titleStyle;
            row.Cells[3].CellStyle = mergedStyle;
            row.Cells[5].CellStyle = titleStyle;
            row.Cells[6].CellStyle = mergedStyle;
        }

        /// <summary>
        /// 根据编码获取枚举
        /// </summary>
        /// <param name="taskStateEnumNumber"></param>
        /// <returns></returns>
        public TaskStateEnum GetTaskStateEnum(string taskStateEnumNumber)
        {
            return (TaskStateEnum)Enum.Parse(typeof(TaskStateEnum), taskStateEnumNumber);
        }
    }
}
