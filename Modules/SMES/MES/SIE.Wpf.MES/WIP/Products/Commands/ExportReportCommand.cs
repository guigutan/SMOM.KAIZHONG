using DevExpress.XtraPrinting.Native;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using SIE.Utils;
using SIE.Wpf;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace SIE.WPF.MES.WIP.Products
{
    /// <summary>
    /// 导出选项
    /// </summary>
    public enum ExportOption
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [Label("当前页")]
        Current,

        /// <summary>
        /// 选中行
        /// </summary>
        [Label("选中行")]
        Selected,

        /// <summary>
        /// 查询结果
        /// </summary>
        [Label("查询结果")]
        All,
    }

    /// <summary>
    /// 导出生产通用报表
    /// </summary>
    [Command(ImageName = "OfficeExcel", Label = "导出Excel", ToolTip = "导出", GroupType = 40)]
    public class ExportReportCommand : ListViewCommand
    {
        /// <summary>
        /// 判断导出命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Data != null && base.CanExecute(view);
        }

        /// <summary>
        /// 导出命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            Export(view);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        void Export(ListLogicalView view)
        {
            ComboBox cbx = CreateOption();
            var pnl = CreateOptionPanel(cbx);
            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), pnl, w =>
            {
                w.Title = "导出选项".L10N();
                w.Width = 200;
                w.Height = 130;
            });
            if (result == 0)
            {
                var opt = (ExportOption)cbx.SelectedValue;
                var data = GetExportData(view, opt);
                CreateTableData(view, data);
            }
        }

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <param name="opt">导出选项</param>
        /// <returns>生产产品版本列表</returns>
        IList<WipProductVersion> GetExportData(ListLogicalView view, ExportOption opt)
        {
            IList<WipProductVersion> data;
            if (view.QueryView != null && opt == ExportOption.All)
            {
                var query = view.QueryView.Current as CriteriaQuery;
                data = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersions(query);
            }
            else if (opt == ExportOption.Selected)
            {
                data = view.SelectedEntities.OfType<WipProductVersion>().ToList();
            }
            else
            {
                data = view.Data.Cast<WipProductVersion>().ToList();
            }

            return data;
        }

        /// <summary>
        /// 创建表格数据
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <param name="data">待导出数据</param>
        void CreateTableData(ListLogicalView view, IList<WipProductVersion> data)
        {
            try
            {
                if (view.Data.Count == 0)
                {
                    throw new ValidationException("没有数据".L10N());
                }

                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = "生产通用报表".L10N() + DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + ".xls",
                    Filter = "excel files (*.xls)|*.xls",
                };
                bool? res = dialog.ShowDialog();
                if (res == null || !res.Value)
                    return;
                string fileName = dialog.FileName;
                if (fileName.IndexOf(":") < 0)
                    return;  //点了取消
                if (Path.GetExtension(fileName) != ".xls")
                    throw new ValidationException("扩展名不对".L10N());

                MemoryStream ms = new MemoryStream();   //创建内存流用于写入文件
                IWorkbook workbooks = new HSSFWorkbook();   //创建Excel工作簿         

                int sheetNum = 1;
                ISheet sheet = workbooks.CreateSheet("Sheet" + sheetNum); //创建工作表

                #region 设置单元格格式
                ICellStyle style1 = workbooks.CreateCellStyle();     //设置单元格格式 
                style1.FillForegroundColor = HSSFColor.Grey25Percent.Index;   //设置主表标题栏单元格的颜色
                style1.FillPattern = FillPattern.SolidForeground;

                ICellStyle style2 = workbooks.CreateCellStyle();     //设置单元格格式 
                style2.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;   //设置从表标题栏单元格的颜色
                style2.FillPattern = FillPattern.SolidForeground;

                ICellStyle style3 = workbooks.CreateCellStyle();     //设置单元格格式 
                style3.FillForegroundColor = HSSFColor.LightYellow.Index;   //设置孙表标题栏单元格的颜色
                style3.FillPattern = FillPattern.SolidForeground;
                #endregion

                if (sheet != null)
                {
                    var wipInfoList = data;
                    int row = 0;  //根据row来插入主表的属性名称       
                    //int totalCount = 0;
                    int count = 0;
                    SetTitle1Row(sheet, row, style1);   //设置主表的标题栏，每个工作单元只有一个标题
                    row++;
                    foreach (var wipInfo in wipInfoList)
                    {
                        count++;
                        sheetNum++;
                        if (IsOverLimit(wipInfo, row))
                        {
                            row = 0;
                            sheet = workbooks.CreateSheet("Sheet" + sheetNum); //创建工作表
                            SetTitle1Row(sheet, row, style1);   //设置主表的标题栏，每个工作单元只有一个标题
                            row++;
                            ExportWipData(sheet, row, wipInfoList[count - 1]);    //导出主表数据
                            row++;
                            row = ExportData(sheet, style2, style3, row, wipInfoList[count - 1], row);     //导出从表及孙表数据 
                        }
                        else
                        {
                            ExportWipData(sheet, row, wipInfo);    //导出主表数据
                            row++;
                            row = ExportData(sheet, style2, style3, row, wipInfo, row);     //导出从表及孙表数据      
                        }

                        for (int i = 0; i < sheet.GetRow(0).Cells.Count; i++)
                        {
                            sheet.AutoSizeColumn(i);    //自动调整列宽
                        }
                    }

                    workbooks.Write(ms);  //将Excel写入流
                    ms.Flush();
                    ms.Position = 0;
                    FileStream dumpFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    ms.WriteTo(dumpFile);  //将流写入
                    ms.Close();
                    ShowDialog(fileName);
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
        }

        /// <summary>
        /// 判断是否超出单个工作表行数
        /// </summary>
        /// <param name="wipInfo">待导出数据</param>
        /// <param name="rowCount">行号总数</param>
        /// <returns>超出单个工作表行数返回true，否则返回false</returns>
        private bool IsOverLimit(WipProductVersion wipInfo, int rowCount)
        {
            int itemCount = wipInfo.InspectionItemList.Count; //检验记录
            int defectCount = wipInfo.DefectList.Count;  //不良记录
            int proCount = wipInfo.ProcessList.Count; //采集记录
            int repCount = wipInfo.RepaireList.Count;   //维修记录
            int resCount = wipInfo.DefectList.Sum(p => p.ResponsibilityList.Count);
            int meaCount = wipInfo.DefectList.Sum(p => p.MeasureList.Count);
            int keyCount = wipInfo.ProcessList.Sum(p => p.KeyItemList.Count);
            int resultCount = wipInfo.ProcessList.Sum(p => p.TestResultList.Count);

            int resTitleCount = wipInfo.DefectList.Count(p => p.ResponsibilityList.Count > 0);
            int meaTitleCount = wipInfo.DefectList.Count(p => p.MeasureList.Count > 0);
            int keyTitleCount = wipInfo.ProcessList.Count(p => p.KeyItemList.Count > 0); //标题 
            int resultTitleCount = wipInfo.ProcessList.Count(p => p.TestResultList.Count > 0);
            rowCount += itemCount > 0 ? itemCount + 1 : itemCount;
            rowCount += defectCount > 0 ? defectCount + 1 : defectCount;
            rowCount += proCount > 0 ? proCount + 1 : proCount;
            rowCount += repCount > 0 ? repCount + 1 : repCount;
            rowCount += resCount > 0 ? resCount + resTitleCount : resCount;
            rowCount += meaCount > 0 ? resCount + meaTitleCount : meaCount;
            rowCount += keyCount > 0 ? keyCount + keyTitleCount : keyCount;
            rowCount += resultCount > 0 ? resultCount + resultTitleCount : resultCount;
            return rowCount >= 65535; //最大行数65535
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="style2">单元格样式2</param>
        /// <param name="style3">单元格样式3</param>
        /// <param name="row">行号</param>
        /// <param name="wipInfo">导出数据对象</param>
        /// <param name="fromrow">来源行号</param>
        /// <returns>下一行号</returns>
        private int ExportData(ISheet sheet, ICellStyle style2, ICellStyle style3, int row, WipProductVersion wipInfo, int fromrow)
        {
            #region 判断产品检验记录表是否有数据，有数据就导出
            var testInfoList = wipInfo.InspectionItemList;
            if (testInfoList.Any())
            {
                SetTitle2Row(sheet, row, style2);   //设置产品检验记录表的标题栏
                row++;
                foreach (var testInfo in testInfoList)
                {
                    ExportTestData(sheet, row, testInfo);    //导出产品检验记录表数据
                    row++;
                }
            }
            #endregion

            #region 判断生产采集记录表是否有数据，有数据就导出
            row = CheckAndExportProcessData(sheet, style2, style3, row, wipInfo);
            #endregion

            #region 判断产品维修记录表是否有数据，有数据就导出
            var repairList = wipInfo.RepaireList;
            if (repairList.Any())
            {
                SetTitle6Row(sheet, row, style2);   //设置产品维修记录表的标题栏
                row++;
                foreach (var repair in repairList)
                {
                    ExportRepairList(sheet, row, repair);     //导出产品维修记录表的数据
                    row++;
                }
            }
            #endregion

            #region 判断产品缺陷记录表是否有数据，有数据就导出
            var defectList = wipInfo.DefectList;
            if (defectList.Any())
            {
                int defectCount = 0;    //记录产品缺陷记录表导出数据条数
                SetTitle7Row(sheet, row, style2);   //设置产品缺陷记录表的标题栏
                row++;
                foreach (var defect in defectList)
                {
                    defectCount++;
                    ExportDefectData(sheet, row, defect);     //导出产品缺陷记录表数据
                    row++;
                    int initrow = row;

                    #region 判断缺陷责任表是否有数据，有数据就导出
                    var responsibilities = defect.ResponsibilityList;
                    if (responsibilities.Any())
                    {
                        SetTitle8Row(sheet, row, style3);   //设置缺陷责任表的标题栏
                        row++;
                        foreach (var responsibility in responsibilities)
                        {
                            ExportResponsibilityData(sheet, row, responsibility);     //导出缺陷责任表数据
                            row++;
                        }
                    }
                    #endregion

                    #region 判断维修措施表是否有数据，有数据就导出
                    var measureList = defect.MeasureList;

                    if (measureList.Any())
                    {
                        SetTitle9Row(sheet, row, style3);   //设置维修措施表的标题栏
                        row++;
                        foreach (var measure in measureList)
                        {
                            ExportDefectMeasureData(sheet, row, measure);   //导出维修措施表数据
                            row++;
                        }
                    }
                    #endregion

                    if (defect.ResponsibilityList.Any() || defect.MeasureList.Any())
                        sheet.GroupRow(initrow, row - 1);    //数据组合，将同一条产品缺陷记录表的从表数据折叠起来   
                }
            }
            #endregion

            if (wipInfo.InspectionItemList.Any() || wipInfo.ProcessList.Any() || wipInfo.RepaireList.Any() || wipInfo.DefectList.Any())
            {
                sheet.GroupRow(fromrow, row - 1);    //数据组合，将同一条主表数据记录下的数据分为一组   
                sheet.SetRowGroupCollapsed(fromrow, true);
            }

            sheet.RowSumsBelow = false;
            sheet.RowSumsRight = false;
            return row;
        }

        /// <summary>
        /// 判断生产采集记录表是否有数据，有数据就导出
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="style2"></param>
        /// <param name="style3"></param>
        /// <param name="row"></param>
        /// <param name="wipInfo"></param>
        /// <returns></returns>
        private int CheckAndExportProcessData(ISheet sheet, ICellStyle style2, ICellStyle style3, int row, WipProductVersion wipInfo)
        {
            var processList = wipInfo.ProcessList;
            if (processList.Any())
            {
                int processCount = 0;   //记录生产采集记录表导出的数据条数
                SetTitle3Row(sheet, row, style2);   //设置生产采集记录表的标题栏
                row++;
                foreach (var process in processList)
                {
                    processCount++;
                    ExportProcessData(sheet, row, process);     //导出生产采集记录表的数据
                    row++;
                    int startrow = row;

                    #region 判断产品生产关键件表是否有数据，有数据就导出
                    var keyItemList = process.GetLazyList(WipProductProcess.KeyItemListProperty, new EagerLoadOptions().LoadWithViewProperty()) as EntityList<WipProductProcessKeyItem>;
                    if (keyItemList.Any())
                    {
                        SetTitle4Row(sheet, row, style3);   //设置产品生产关键件表的标题栏
                        row++;
                        foreach (var keyItem in keyItemList)
                        {
                            ExportKeyItemData(sheet, row, keyItem);     //导出产品生产关键件的数据
                            row++;
                        }
                    }
                    #endregion

                    #region 判断产品测试结果表是否有数据，有数据就导出
                    var testResultList = process.TestResultList;
                    if (testResultList.Any())
                    {
                        SetTitle5Row(sheet, row, style3);   //设置产品测试结果表的标题栏
                        row++;
                        foreach (var testResult in testResultList)
                        {
                            ExportTestResult(sheet, row, testResult);    //导出产品测试结果表的数据
                            row++;
                        }
                    }
                    #endregion

                    if (process.KeyItemList.Any() || process.TestResultList.Any())
                        sheet.GroupRow(startrow, row - 1);    //数据组合，将同一条生产采集记录表下的从表数据折叠起来   
                }
            }
            return row;
        }

        /// <summary>
        /// 创建选项下拉框
        /// </summary>
        /// <returns>控件</returns>
        ComboBox CreateOption()
        {
            ComboBox cbx = new ComboBox();
            cbx.MinWidth = 200;
            cbx.ItemsSource = EnumViewModel.GetByEnumType(typeof(ExportOption));
            cbx.SelectedValuePath = "EnumValue";
            cbx.DisplayMemberPath = "TranslatedLabel";
            cbx.SelectedIndex = 0;
            return cbx;
        }

        /// <summary>
        /// 创建导出项选项框
        /// </summary>
        /// <param name="cbx">下拉选项</param>
        /// <returns>控件</returns>
        StackPanel CreateOptionPanel(ComboBox cbx)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            panel.Children.Add(new Label { Content = "数据选项" });
            panel.Children.Add(cbx);
            return panel;
        }

        /// <summary>
        /// 弹出框提示是否打开文档
        /// </summary>
        /// <param name="fileName">文件路径选择器</param>
        public void ShowDialog(string fileName)
        {
            if (CRT.MessageService.AskQuestion("是否打开文档！".L10N()))
            {
                ProcessLaunchHelper.Start(fileName);
            }
        }

        /// <summary>
        /// 初始化标题单元格
        /// </summary>
        /// <param name="cellNumber">单元格号</param>
        /// <param name="row">行</param>
        /// <param name="style">单元格样式</param>
        /// <param name="cellValue">单元格值</param>
        void InitTitleCell(int cellNumber, IRow row, ICellStyle style, string cellValue)
        {
            var cell = row.CreateCell(cellNumber);
            cell.CellStyle = style;
            cell.SetCellValue(cellValue);
        }

        /// <summary>
        /// 初始化单元格
        /// </summary>
        /// <param name="cellNumber">单元格号</param>
        /// <param name="row">行</param> 
        /// <param name="cellValue">单元格值</param>
        void InitCell(int cellNumber, IRow row, string cellValue)
        {
            var cell = row.CreateCell(cellNumber);
            cell.SetCellValue(cellValue);
        }

        #region 设置表标题
        /// <summary>
        /// 设置生产产品版本标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle1Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "条码");
            InitTitleCell(1, irow, style, "是否hold");
            InitTitleCell(2, irow, style, "工单号");
            InitTitleCell(3, irow, style, "工单类型");
            InitTitleCell(4, irow, style, "工单数量");
            InitTitleCell(5, irow, style, "工艺流程名称");
            InitTitleCell(6, irow, style, "车间");
            InitTitleCell(7, irow, style, "产品型号");
            InitTitleCell(8, irow, style, "当前工序");
            InitTitleCell(9, irow, style, "当前工位资源");
            InitTitleCell(10, irow, style, "产品等级");
            InitTitleCell(11, irow, style, "是否已完工下线");
        }

        /// <summary>
        /// 设置产品检验记录表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle2Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "产品检验记录");
            InitTitleCell(1, irow, style, "项目编码");
            InitTitleCell(2, irow, style, "项目名称");
            InitTitleCell(3, irow, style, "规范上限");
            InitTitleCell(4, irow, style, "规范下限");
            InitTitleCell(5, irow, style, "测试值");
            InitTitleCell(6, irow, style, "检验结果");
            InitTitleCell(7, irow, style, "备注");
            InitTitleCell(8, irow, style, "检验人");
        }

        /// <summary>
        /// 设置采集记录表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle3Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "生产采集记录");
            InitTitleCell(1, irow, style, "状态");
            InitTitleCell(2, irow, style, "操作时间");
            InitTitleCell(3, irow, style, "采集结果");
            InitTitleCell(4, irow, style, "工位");
            InitTitleCell(5, irow, style, "工序");
            InitTitleCell(6, irow, style, "产线");
            InitTitleCell(7, irow, style, "操作人");
            InitTitleCell(8, irow, style, "条码");
        }

        /// <summary>
        /// 设置产品生产关键件表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle4Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "产品生产关键件");
            InitTitleCell(1, irow, style, string.Empty);
            InitTitleCell(2, irow, style, "工序");
            InitTitleCell(3, irow, style, "工位");
            InitTitleCell(4, irow, style, "来源条码");
            InitTitleCell(5, irow, style, "来源类型");
            InitTitleCell(6, irow, style, "用料数");
            InitTitleCell(7, irow, style, "物料编码");
            InitTitleCell(8, irow, style, "物料名称");
            InitTitleCell(9, irow, style, "物料描述");
            InitTitleCell(10, irow, style, "单位");
            InitTitleCell(11, irow, style, "操作人");
            InitTitleCell(12, irow, style, "操作时间");
        }

        /// <summary>
        /// 设置产品测试结果表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle5Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "产品测试结果");
            InitTitleCell(1, irow, style, string.Empty);
            InitTitleCell(2, irow, style, "测试项目");
            InitTitleCell(3, irow, style, "测试结果");
            InitTitleCell(4, irow, style, "测试时间");
        }

        /// <summary>
        /// 设置产品维修记录表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle6Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "产品维修记录");
            InitTitleCell(1, irow, style, "缺陷代码");
            InitTitleCell(2, irow, style, "返修时间");
            InitTitleCell(3, irow, style, "返修人");
            InitTitleCell(4, irow, style, "工位");
            InitTitleCell(5, irow, style, "工序");
            InitTitleCell(6, irow, style, "产线");
            InitTitleCell(7, irow, style, "班次");
        }

        /// <summary>
        /// 设置产品缺陷记录表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle7Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "产品缺陷记录");
            InitTitleCell(1, irow, style, "缺陷编码");
            InitTitleCell(2, irow, style, "缺陷描述");
            InitTitleCell(3, irow, style, "备注");
            InitTitleCell(4, irow, style, "缺陷位置");
            InitTitleCell(5, irow, style, "工序");
            InitTitleCell(6, irow, style, "维修人");
            InitTitleCell(7, irow, style, "维修时间");
        }

        /// <summary>
        /// 设置缺陷责任表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle8Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "缺陷责任");
            InitTitleCell(1, irow, style, string.Empty);
            InitTitleCell(2, irow, style, "编码");
            InitTitleCell(3, irow, style, "描述");
        }

        /// <summary>
        /// 设置维修措施表的标题栏
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="style">单元格样式</param>
        private void SetTitle9Row(ISheet sheet, int row, ICellStyle style)
        {
            IRow irow = sheet.CreateRow(row);
            InitTitleCell(0, irow, style, "维修措施");
            InitTitleCell(1, irow, style, string.Empty);
            InitTitleCell(2, irow, style, "编码");
            InitTitleCell(3, irow, style, "名称");
            InitTitleCell(4, irow, style, "描述");
        }
        #endregion

        #region 数据导出 
        /// <summary>
        /// 导出生产产品版本数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="wip">生产产品版本</param>
        private void ExportWipData(ISheet sheet, int row, WipProductVersion wip)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(0, irow, wip.Sn);                                      //条码
            InitCell(1, irow, wip.IsHold.ToString());                       //是否hold
            InitCell(2, irow, wip.WorkOrder.No);                            //工单号 
            InitCell(3, irow, wip.WorkOrder.Type.ToString());               //工单类型
            InitCell(4, irow, wip.WorkOrder.PlanQty.ToString());            //工单数量
            InitCell(5, irow, wip.WorkOrder.Version?.Name);                  //工艺流程名称
            InitCell(6, irow, wip.WorkOrder.WorkShop.Name);                 //车间
            InitCell(7, irow, wip.WorkOrder.Product.Model?.Name);           //产品型号
            //InitCell(8, irow, wip.CurrentProcess?.Process?.Name);           //当前工序
            InitCell(8, irow, wip.NowProcess?.Name);           //当前工序
            InitCell(9, irow, wip.WorkOrder.Resource.Name);                 //当前工位资源
            InitCell(10, irow, wip.Grade.ToLabel());                        //产品等
            InitCell(11, irow, wip.IsFinish.ToString());                    //是否已完工下线
        }

        /// <summary>
        /// 导出产品检验记录数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="item">产品检验记录</param>
        private void ExportTestData(ISheet sheet, int row, WipProductInspectionItem item)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(2, irow, item.InspectionItem.Name);
            if (item.LimitMax.HasValue)
                InitCell(3, irow, item.LimitMax.ToString());
            if (item.LimitLow.HasValue)
                InitCell(4, irow, item.LimitLow.ToString());
            if (item.InspectionValue.HasValue)
                InitCell(5, irow, item.InspectionValue.ToString());
            InitCell(6, irow, item.Result.ToLabel());
            InitCell(7, irow, item.Remarks);
            InitCell(8, irow, item.InspectBy?.Name);
        }

        /// <summary>
        /// 导出生产采集记录表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="process">生产采集记录</param>
        private void ExportProcessData(ISheet sheet, int row, WipProductProcess process)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(1, irow, process.State.ToLabel());                            //状态
            if (process.OperateTime.HasValue)
                InitCell(2, irow, process.OperateTime.Value.ToString());           //操作时间
            InitCell(3, irow, process.Result.ToLabel());                           //采集结果
            InitCell(4, irow, process.Station.Name);                               //工位
            InitCell(5, irow, process.Process.Name);                               //工序
            InitCell(6, irow, process.Resource.Name);             //产线--资源
            InitCell(7, irow, process.OperateBy.Name);                             //操作人
            InitCell(8, irow, process.Barcode);                                    //条码
        }

        /// <summary>
        /// 导出产品生产关键件表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="keyItem">产品生产关键件</param>
        private void ExportKeyItemData(ISheet sheet, int row, WipProductProcessKeyItem keyItem)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(2, irow, keyItem.Process.Process.Name);                          //工序
            InitCell(3, irow, keyItem.Process.Station.Name);                          //工位
            InitCell(4, irow, keyItem.SourceCode);                                    //来源条码
            InitCell(5, irow, keyItem.SourceType.ToLabel());                          //来源类型
            InitCell(6, irow, keyItem.Qty.ToString());                                 //用料数
            InitCell(7, irow, keyItem.Item.Code);                                     //物料编码
            InitCell(8, irow, keyItem.Item.Name);                                     //物料名称
            InitCell(9, irow, keyItem.Item.Description);                              //物料描述
            InitCell(10, irow, keyItem.Item.Unit.Name);                                //单位
            InitCell(11, irow, keyItem.CreateByName);          //操作人
            InitCell(12, irow, keyItem.CreateDate.ToString());                         //操作时间
        }

        /// <summary>
        /// 导出产品测试结果表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="testResult">产品测试结果</param>
        private void ExportTestResult(ISheet sheet, int row, WipProductTestResult testResult)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(1, irow, testResult.Item);                           //测试项目
            InitCell(2, irow, testResult.Result?.ToString());             //测试结果
            InitCell(3, irow, testResult.CreateDate.ToString());          //测试时间
        }

        /// <summary>
        /// 导出产品维修记录表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="repair">产品维修记录</param>
        private void ExportRepairList(ISheet sheet, int row, WipProductRepair repair)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(1, irow, repair.RepairType?.ToLabel());           //维修类型
            InitCell(2, irow, repair.RepairStart.ToString());          //维修开始时间
            InitCell(3, irow, repair.RepaireTime.ToString());          //维修完成时间
            InitCell(4, irow, repair.ReparieBy.Name);                  //采集结果
            InitCell(5, irow, repair.Station.Name);                    //工位
            InitCell(6, irow, repair.Process.Name);                    //工序
            InitCell(7, irow, repair.Resource.Name);                   //产线--资源
            InitCell(8, irow, repair.Shift?.Name);                     //操作人
        }

        /// <summary>
        /// 导出产品缺陷记录表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="defect">产品缺陷记录</param>
        private void ExportDefectData(ISheet sheet, int row, WipProductDefect defect)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(1, irow, defect.Defect.Code);                  //缺陷编码
            InitCell(2, irow, defect.Defect.Description);           //缺陷描述
            InitCell(3, irow, defect.Remark);                       //备注
            InitCell(4, irow, defect.Location);                     //缺陷位置
            InitCell(5, irow, defect.Process.Name);                 //工序
            InitCell(6, irow, defect.FixedBy?.Name);                //维修人
            InitCell(7, irow, defect.FixedDate?.ToString());        //维修时间
        }

        /// <summary>
        /// 导出缺陷责任表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="responsibility">产品缺陷责任</param>
        private void ExportResponsibilityData(ISheet sheet, int row, WipDefectResponsibility responsibility)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(1, irow, responsibility.DefectResponsibility.Code);               //编码
            InitCell(2, irow, responsibility.DefectResponsibility.Description);        //描述
        }

        /// <summary>
        /// 导出维修措施表数据
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行号</param>
        /// <param name="measure">产品缺陷维修措施</param>
        private void ExportDefectMeasureData(ISheet sheet, int row, WipDefectMeasure measure)
        {
            IRow irow = sheet.CreateRow(row);
            InitCell(1, irow, measure.RepairMeasure.Code);            //编码
            InitCell(2, irow, measure.RepairMeasure.Name);            //名称
            InitCell(3, irow, measure.RepairMeasure.Description);     //描述
        }
        #endregion
    }
}
