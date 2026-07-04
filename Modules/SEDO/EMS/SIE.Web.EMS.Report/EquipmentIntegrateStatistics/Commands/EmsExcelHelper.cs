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

namespace SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Commands
{
    /// <summary>
    /// 导入导出共用方法
    /// </summary>
    public class EmsExcelHelper
    {
        #region Excel按路径导出
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="workbook">工作簿</param>
        /// <param name="cell">单元格</param>
        public void SetCellStyle(IWorkbook workbook, ICell cell)
        {
            HSSFFont headfont = (HSSFFont)workbook.CreateFont();
            headfont.FontName = "微软雅黑";
            headfont.FontHeightInPoints = 11;

            ICellStyle cellstyle = workbook.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Left;

            cell.CellStyle = cellstyle;
            cell.CellStyle.SetFont(headfont);
        }

        #endregion

        /// <summary>
        /// 导出数据至Excel (带图片导出)
        /// </summary>        
        /// <param name="dataView">数据视图</param>
        /// <param name="images">图片</param>
        public MemoryStream ExportData(JObject dataView, List<Bitmap> images)
        {
            MemoryStream ms = new MemoryStream();    //创建内存流用于写入文件
            const string dataViewSheetName = "设备综合统计报表";
            IWorkbook workbook = new HSSFWorkbook();
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;

            //合格单元格格式
            ICellStyle mergedStyle = workbook.CreateCellStyle();
            mergedStyle.VerticalAlignment = VerticalAlignment.Center;
            mergedStyle.Alignment = HorizontalAlignment.Center;
            mergedStyle.BottomBorderColor = 0;
            mergedStyle.WrapText = true;

            ISheet sheetGridData = workbook.CreateSheet(dataViewSheetName);
            workbook.SetSheetName(0, dataViewSheetName);
            SetGridData(sheetGridData, dataView, style);
            ISheet sheet1 = workbook.CreateSheet("报表");  //创建工作表 
            workbook.SetSheetOrder(dataViewSheetName, 1); //将抽样数据sheet放到最后
            workbook.SetSelectedTab(0);
            workbook.SetActiveSheet(0);

            if (sheet1 != null)
            {
                try
                {
                    for (int i = 0; i < images.Count; i++)  //可能有多张控制图片
                    {
                        var img = images[i];
                        if (img != null)
                        {
                            byte[] bytes = null;
                            sheet1.PrintSetup.Landscape = true;
                            sheet1.DisplayGridlines = false;
                            var imgRowIndex = i * 20;
                            const int chartColumn = 0;

                            using (Bitmap bitmap = new Bitmap(img)) //img 控制图图片
                            {
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    bitmap.Save(stream, ImageFormat.Jpeg);
                                    bytes = stream.GetBuffer();
                                }
                            }

                            int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG); //将图片加入EXCEL
                            IDrawing patriarch = sheet1.CreateDrawingPatriarch();  //创建绘制器
                            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, chartColumn, imgRowIndex, chartColumn, imgRowIndex);//设置坐标
                            IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);//绘制图片
                            pict.Resize();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogManager.Logger.Error(ex.Message);
                    throw new ValidationException("图片保存失败".L10N());
                }
            }

            workbook.Write(ms);  //将Excel写入流
            return ms;
        }
        /// <summary>
        /// 设置抽样数据列表
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="dataView"></param>
        /// <param name="style">样式</param>
        private void SetGridData(ISheet sheet, JObject dataView, ICellStyle style)
        {
            var header = dataView["Headers"].ToObject<List<string>>();  // 表头
            var datas = dataView["Datas"].ToObject<JArray>();  //列表数据
            IRow row = sheet.CreateRow(0);  //在工作表中添加一行
            //表头
            for (int iHeader = 0; iHeader < header.Count; iHeader++)
            {
                row.CreateCell(iHeader);
                row.Cells[iHeader].SetCellValue(header[iHeader]);
                row.Cells[iHeader].CellStyle = style;
            }
            //列表数据
            for (int i = 0; i < datas.Count; i++)
            {
                row = sheet.CreateRow(i + 1);
                var rowData = datas[i].ToObject<JObject>();
                var props = rowData.Properties().ToArray();
                for (int j = 0; j < props.Count(); j++)
                {
                    row.CreateCell(j);
                    var cell = row.Cells[j];
                    cell.SetCellValue(props[j].Value.ToString());
                    row.Cells[j].CellStyle = style;
                }
            }
        }
    }
}
