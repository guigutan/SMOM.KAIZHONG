using DevExpress.Xpf.Charts;
using DevExpress.Xpf.PivotGrid;
using SIE.Domain;
using SIE.MES.DashBoard.WorkOrderReachs;
using SIE.Wpf.MES.DashBoard.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 直通率报表基础控件
    /// Interaction logic for DirectRateBasePivotGridControl.xaml
    /// </summary>
    public partial class WorkOrderReachPivotGridControl : UserControl
    {
        private const string FINISHED_ONTIME_NUMBER = "准时完工数";
        private const string ORDER_FINISHED_NUMBER = "工单完工数";

        /// <summary>
        /// 柏拉图第二纵坐标最小值
        /// </summary>
        double _secondaryAxisYMinValue;

        /// <summary>
        /// 柏拉图第二纵坐标最大值
        /// </summary>
        double _secondaryAxisYMaxValue = 1;

        /// <summary>
        /// 报表控件的ViewModel
        /// </summary>
        private WoReachReportViewModel _reportViewModel { get; set; }

        /// <summary>
        /// 报表控件的ViewModel
        /// </summary>
        public WoReachReportViewModel ReportViewModel
        {
            get { return _reportViewModel; }
            set { _reportViewModel = value; }
        }

        /// <summary>
        /// 工单总数柱状图
        /// </summary>
        private List<SeriesPoint> lsBarPoint = new List<SeriesPoint>();

        /// <summary>
        /// 准时完工数柱状图
        /// </summary>
        private List<SeriesPoint> lsBar2Point = new List<SeriesPoint>();

        /// <summary>
        /// 工单完工数柱状图
        /// </summary>
        private List<SeriesPoint> lsBar3Point = new List<SeriesPoint>();

        /// <summary>
        /// 达成率折线图
        /// </summary>
        private List<SeriesPoint> lsLinePoint = new List<SeriesPoint>();

        /// <summary>
        /// 完工率折线图
        /// </summary>
        private List<SeriesPoint> lsLine2Point = new List<SeriesPoint>();

        /// <summary>
        /// 自定义求和字典（Key:行属性+":"+时间，Value:百分值）
        /// </summary>
        Dictionary<string, double> dics = new Dictionary<string, double>();

        /// <summary>
        /// 行头
        /// </summary>
        private PivotGridField rowfield;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderReachPivotGridControl()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                _reportViewModel = this.DataContext as WoReachReportViewModel;
                _reportViewModel.LayoutFileName = "工单准时达成率报表";
                if (pivotGrid.Fields.Count <= 0)
                {
                    pivotGrid.Fields.AddRange(new ReportHelper().InitPivotGridFields(_reportViewModel.WorkOrderReachList.EntityType));
                    rowfield = pivotGrid.Fields.FirstOrDefault(p => p.FieldName == "RowName");
                    rowfield.SortByFieldName = "RowOrder";
                    ReportHelper.RestoreLayoutFromXml(pivotGrid, _reportViewModel.LayoutFileName);
                }

                SetDic();
                BindingChartByChartDisplayMode();
            };
            pivotGrid.Loaded += PivotGrid_Loaded;
        }

        /// <summary>
        /// 报表加载事件
        /// </summary>
        /// <param name="sender">s</param>
        /// <param name="e">e</param>
        private void PivotGrid_Loaded(object sender, RoutedEventArgs e)
        {
            pivotGrid.ShowRowHeaders = false;
        }

        /// <summary>
        /// 字典存储数据
        /// </summary>
        private void SetDic()
        {
            if (ReportViewModel.WorkOrderReachList != null)
            {
                dics.Clear();
                var list = ReportViewModel.WorkOrderReachList;
                var datas = list.OrderBy(p => p.Week).GroupBy(p => p.Week);
                for (int i = 0; i < 3; i++)
                {
                    if (i == 1)
                    {
                        datas = list.OrderBy(p => p.Month).GroupBy(p => p.Month);
                    }
                    else if (i == 2)
                    {
                        datas = list.OrderBy(p => p.Year).GroupBy(p => p.Year);
                    }
                    else
                    {
                        //
                    }

                    ComputeDic(datas);
                }

                var plandatas = list.OrderBy(p => p.Week).GroupBy(p => p.PlanDate.ToString());
                ComputeDic(plandatas);
            }
        }

        /// <summary>
        /// 计算字典存储数据
        /// </summary>
        /// <param name="datas"></param>
        private void ComputeDic(IEnumerable<IGrouping<string, WorkOrderReachViewModel>> datas)
        {
            foreach (var group in datas)
            {
                double comRate = 0;
                double _closeRate = 0;
                double _totalQty = 0;
                double reachQty = 0;
                double _closeQty = 0;
                _totalQty = group.Where(p => p.RowName == "工单总数".L10N()).Sum(p => p.Data);
                reachQty = group.Where(p => p.RowName == FINISHED_ONTIME_NUMBER.L10N()).Sum(p => p.Data);
                _closeQty = group.Where(p => p.RowName == ORDER_FINISHED_NUMBER.L10N()).Sum(p => p.Data);
                if (_totalQty != 0)
                {
                    comRate = Math.Round(reachQty / _totalQty, 4);
                    _closeRate = Math.Round(_closeQty / _totalQty, 4);
                }

                dics.Add("工单总数:" + group.Key.L10N(), _totalQty);
                dics.Add(FINISHED_ONTIME_NUMBER + ":" + group.Key.L10N(), reachQty);
                dics.Add(ORDER_FINISHED_NUMBER + ":" + group.Key.L10N(), _closeQty);
                dics.Add("准时达成率:" + group.Key.L10N(), comRate);
                dics.Add("工单完工率:" + group.Key.L10N(), _closeRate);
            }
        }

        /// <summary>
        /// 单元格数据展示
        /// </summary>
        /// <param name="sender">s</param>
        /// <param name="e">e</param>
        private void pivotGrid_CustomCellDisplayText(object sender, PivotCellDisplayTextEventArgs e)
        {
            var fieldName = e.GetFieldValue(rowfield)?.ToString();
            if (fieldName == "工单总数" || fieldName == FINISHED_ONTIME_NUMBER || fieldName == ORDER_FINISHED_NUMBER)
            {
                e.DisplayText = e.Value.ToString();
            }
        }

        /// <summary>
        /// 初始化属性名称
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">参数</param>
        private void pivotGrid_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {
            if (e.ValueType == FieldValueType.Total)
                e.DisplayText = e.DisplayText.Replace(" Total", string.Empty);
            if (e.Field?.FieldName == WorkOrderReachViewModel.YearProperty.Name)
            {
                e.DisplayText = "{0}年".L10nFormat(e.Value.ToString());
            }
            else if (e.Field?.FieldName == WorkOrderReachViewModel.MonthProperty.Name)
            {
                e.DisplayText = e.Value.ToString().Substring(5).L10N();
            }
            else if (e.Field?.FieldName == WorkOrderReachViewModel.WeekProperty.Name)
            {
                e.DisplayText = e.Value.ToString().Substring(5).L10N();
            }
            else if (e.Field?.FieldName == WorkOrderReachViewModel.PlanDateProperty.Name)
            {
                e.DisplayText = "{0}号".L10nFormat(((DateTime)e.Value).ToString("dd"));
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 属性单元格选择变更事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void pivotGrid_CellSelectionChanged(object sender, RoutedEventArgs e)
        {
            var fieldName = pivotGrid.SelectedCellInfo.RowValueInfo.Field?.FieldName;
            var fieldValue = pivotGrid.SelectedCellInfo.RowValueInfo.Value?.ToString();
            if (fieldName.IsNullOrEmpty() || fieldValue.IsNullOrEmpty())
            {
                return;
            }
        }

        /// <summary>
        /// 导出命令
        /// </summary>
        /// <param name="sender">命令对象</param>
        /// <param name="e">对象参数</param>
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            new ReportHelper().ExportToExcel(new List<DevExpress.Xpf.Printing.IPrintableControl> { pivotGrid, chart });
        }

        /// <summary>
        /// 保存当前布局命令
        /// </summary>
        /// <param name="sender">命令对象</param>
        /// <param name="e">命令参数</param>
        private void BtnClick_SaveLayOut(object sender, RoutedEventArgs e)
        {
            ReportHelper.SaveLayoutToXml(pivotGrid, _reportViewModel?.LayoutFileName);
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">控件参数</param>
        private void pivotGrid_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            var fieldName = e.GetFieldValue(rowfield)?.ToString();
            if (fieldName == "准时达成率")
            {
                var selectedCellInfo = pivotGrid.SelectedCellInfo;

                var colFieldValue = selectedCellInfo.ColumnValueInfo.Value?.ToString();

                string fpyTitle = string.Empty;
                if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(WorkOrderReachViewModel.PlanDate))
                {
                    fpyTitle = DateTime.Parse(colFieldValue).ToString("yyyy年MM月dd日");
                }
                else if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(WorkOrderReachViewModel.Year))
                {
                    fpyTitle = colFieldValue + "年";
                }
                else if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(WorkOrderReachViewModel.Month))
                {
                    fpyTitle = colFieldValue;
                }
                else if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(WorkOrderReachViewModel.Week))
                {
                    fpyTitle = colFieldValue;
                }
                else
                {
                    //
                }
                var cri = ReportViewModel.Criteria;
                cri.ColumnFieldName = e.ColumnField.FieldName;
                cri.ColumnFieldValue = colFieldValue;

                var template = new ListUITemplate(typeof(WoReachDetailViewModel), ViewConfig.ListView);
                var ui = template.CreateUI();
                ui.MainView.Data = RT.Service.Resolve<WorkOrderReachController>().GetWoReachDetailList(cri);
                var key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(WoReachDetailViewModel), null);
                CRT.Workbench.Close(key);

                CRT.Workbench.ShowView(key, w =>
               {
                   w.Title = "{0}工单准时达成率明细".L10nFormat(fpyTitle);
                   return ui.Control;
               });
            }
        }

        /// <summary>
        /// FieldArea改变时触发事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件包含参数</param>
        private void PivotGrid_FieldAreaChanged(object sender, PivotFieldEventArgs e)
        {
            BindingChartByChartDisplayMode();
        }

        /// <summary>
        /// 根据ColumnArea绑定图表
        /// </summary>
        public void BindingChartByChartDisplayMode()
        {
            var fileds = pivotGrid.Fields.Where(p => p.Area == DevExpress.Xpf.PivotGrid.FieldArea.ColumnArea).ToList();
            if (fileds.Count == 0)
            {
                ////BindingChart(ChartDisplayMode.Total);
                return;
            }

            var filedDay = fileds.FirstOrDefault(p => p.Caption == "日");
            if (filedDay != null)
            {
                BindingChart(ChartDisplayMode.Day);
                return;
            }

            var filedWeek = fileds.FirstOrDefault(p => p.Caption == "周");
            if (filedWeek != null)
            {
                BindingChart(ChartDisplayMode.Week);
                return;
            }

            var filedMonth = fileds.FirstOrDefault(p => p.Caption == "月");
            if (filedMonth != null)
            {
                BindingChart(ChartDisplayMode.Montth);
                return;
            }

            var filedYear = fileds.FirstOrDefault(p => p.Caption == "年");
            if (filedYear != null)
            {
                BindingChart(ChartDisplayMode.Year);
            }
        }

        /// <summary>
        /// 柏拉图按维度显示
        /// </summary>
        /// <param name="chartDisplayMode">维度</param>
        public void BindingChart(ChartDisplayMode chartDisplayMode)
        {
            GetData(chartDisplayMode);
            BarSideSerie.Points.Clear();
            BarSideSerie.Points.AddRange(lsBarPoint);
            BarSideSerie2.Points.Clear();
            BarSideSerie2.Points.AddRange(lsBar2Point);
            BarSideSerie3.Points.Clear();
            BarSideSerie3.Points.AddRange(lsBar3Point);
            LineSerie.Points.Clear();
            LineSerie.Points.AddRange(lsLinePoint);
            LineSerie.ArgumentScaleType = ScaleType.Qualitative;

            LineSerie2.Points.Clear();
            LineSerie2.Points.AddRange(lsLine2Point);
            LineSerie2.ArgumentScaleType = ScaleType.Qualitative;

            double minY = Math.Max(0, lsLinePoint.Count > 0 ? lsBar2Point.Select(v => v.Value).Min() : 0);
            double maxY = Math.Max(3, lsLinePoint.Count > 0 ? lsBarPoint.Select(v => v.Value).Max() : 3);
            _AxisY.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = minY, MaxValue = maxY };
            _AxisY.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = minY, MaxValue = maxY };
            _SecondaryAxisY2D.WholeRange = new DevExpress.Xpf.Charts.Range() { MinValue = _secondaryAxisYMinValue, MaxValue = _secondaryAxisYMaxValue };
            _SecondaryAxisY2D.VisualRange = new DevExpress.Xpf.Charts.Range() { MinValue = _secondaryAxisYMinValue, MaxValue = _secondaryAxisYMaxValue };

            const double group_width = 40;
            double barWidth = lsLinePoint.Count * 1.0 / 10;
            barWidth = Math.Min(0.7, barWidth);
            barWidth = Math.Max(0.1, barWidth);
            BarSideSerie.BarWidth = barWidth; //柱形图宽度
            BarSideSerie2.BarWidth = barWidth;
            BarSideSerie3.BarWidth = barWidth;
            diagram.SetAxisXZoomRatio(0);
            if (lsBar2Point.Count > 0 && diagram.ActualWidth > 0)
            {
                var unit_w = diagram.ActualWidth * 0.7 / lsBarPoint.Count;
                if (unit_w < group_width)
                {
                    var p = (group_width - unit_w) / group_width;
                    diagram.SetAxisXZoomRatio(p);
                }
            }

            diagram.ScrollAxisXTo(0);
            BarSideSerie.Animate();
            BarSideSerie2.Animate();
            BarSideSerie3.Animate();
            LineSerie.Animate();
            LineSerie2.Animate();
            chart.UpdateData();

            ReportViewModel.PersistenceStatus = PersistenceStatus.Unchanged;
        }

        /// <summary>
        /// 获取数据并加工转化
        /// </summary>
        /// <param name="chartDisplayMode">柏拉图按维度显示</param>
        private void GetData(ChartDisplayMode chartDisplayMode)
        {
            if (ReportViewModel != null && ReportViewModel.WorkOrderReachList.Any())
            {
                lsBarPoint.Clear();
                lsBar2Point.Clear();
                lsBar3Point.Clear();
                lsLinePoint.Clear();
                lsLine2Point.Clear();

                Dictionary<string, double> closeQtyDicValue;
                Dictionary<string, double> reachQtyDicValue;
                Dictionary<string, double> totalQtyDicValue;
                Dictionary<string, double> comRateDicValue;
                Dictionary<string, double> closeRateDicValue;

                HandelData(chartDisplayMode, out closeQtyDicValue, out reachQtyDicValue, out totalQtyDicValue, out comRateDicValue, out closeRateDicValue);

                foreach (var item in new Dictionary<string, double>())
                {
                    lsBar3Point.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

                foreach (var item in reachQtyDicValue)
                {
                    lsBar2Point.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

                foreach (var item in totalQtyDicValue)
                {
                    lsBarPoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

                foreach (var item in comRateDicValue)
                {
                    lsLinePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

                foreach (var item in closeRateDicValue)
                {
                    lsLine2Point.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

                if (comRateDicValue.Count > 0 || closeRateDicValue.Count > 0)
                {
                    //设置第二纵坐标最小值和最大值
                    var min = comRateDicValue.Values.Min() > closeRateDicValue.Values.Min() ? closeRateDicValue.Values.Min() : comRateDicValue.Values.Min();
                    var max = comRateDicValue.Values.Max() > closeRateDicValue.Values.Max() ? comRateDicValue.Values.Max() : closeRateDicValue.Values.Max();
                    _secondaryAxisYMinValue = min;
                    _secondaryAxisYMaxValue = max;

                    if (_secondaryAxisYMinValue == _secondaryAxisYMaxValue)
                    {
                        _secondaryAxisYMinValue = 0;
                    }

                    if (_secondaryAxisYMaxValue == 0)
                    {
                        _secondaryAxisYMaxValue = 1;
                    }
                }
            }
            else
            {
                lsBarPoint.Clear();
                lsBar2Point.Clear();
                lsBar3Point.Clear();
                lsLinePoint.Clear();
                lsLine2Point.Clear();
                _secondaryAxisYMinValue = 0;
                _secondaryAxisYMaxValue = 1;
            }
        }

        /// <summary>
        /// 自定义单元格值
        /// </summary>
        /// <param name="sender">PivotGrid控件</param>
        /// <param name="e">单元格事件参数</param>
        private void pivotGrid_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            ////当时最低层级且按日期排布的话则不进行自定义
            ////if (e.ColumnField?.FieldName == nameof(WorkOrderReachViewModel.PlanDate)) return;

            //若行属性为自定义时，则去相应Key的自定义值
            if (e.RowField?.SummaryType == FieldSummaryType.Custom && dics != null)
            {
                var parentField = e.GetRowFields().FirstOrDefault(p => p.AreaIndex == e.RowField.AreaIndex - 1);
                string parentRowFieldValue = string.Empty;
                if (parentField != null)
                    parentRowFieldValue += ":" + e.GetFieldValue(parentField);
                var rowFieldValue = e.GetFieldValue(e.RowField);
                var colFieldValue = e.GetFieldValue(e.ColumnField);
                if (dics.ContainsKey(rowFieldValue + ":" + colFieldValue + parentRowFieldValue))
                    e.Value = dics[rowFieldValue + ":" + colFieldValue + parentRowFieldValue];
                e.Handled = true;
            }
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="chartDisplayMode">chartDisplayMode</param>
        /// <param name="closeQtyDicValue">closeQtyDicValue</param>
        /// <param name="reachQtyDicValue">reachQtyDicValue</param>
        /// <param name="totalQtyDicValue">totalQtyDicValue</param>
        /// <param name="comRateDicValue">comRateDicValue</param>
        /// <param name="closeRateDicValue">closeRateDicValue</param>
        private void HandelData(ChartDisplayMode chartDisplayMode, out Dictionary<string, double> closeQtyDicValue, out Dictionary<string, double> reachQtyDicValue,
            out Dictionary<string, double> totalQtyDicValue, out Dictionary<string, double> comRateDicValue, out Dictionary<string, double> closeRateDicValue)
        {
            closeQtyDicValue = new Dictionary<string, double>();
            reachQtyDicValue = new Dictionary<string, double>();
            totalQtyDicValue = new Dictionary<string, double>();
            comRateDicValue = new Dictionary<string, double>();
            closeRateDicValue = new Dictionary<string, double>();
            var list = ReportViewModel.WorkOrderReachList;
            if (chartDisplayMode == ChartDisplayMode.Day)
            {
                var datas = list.OrderBy(p => p.PlanDate).GroupBy(p => p.PlanDate);
                foreach (var group in datas)
                {
                    double comRate = 0;
                    double _closeRate = 0;
                    double _totalQty = 0;
                    double reachQty = 0;
                    double _closeQty = 0;
                    _totalQty = group.Where(p => p.RowName == "工单总数".L10N()).Sum(p => p.Data);
                    reachQty = group.Where(p => p.RowName == FINISHED_ONTIME_NUMBER.L10N()).Sum(p => p.Data);
                    _closeQty = group.Where(p => p.RowName == ORDER_FINISHED_NUMBER.L10N()).Sum(p => p.Data);
                    if (_totalQty != 0)
                    {
                        comRate = Math.Round(reachQty / _totalQty, 4);
                        _closeRate = Math.Round(_closeQty / _totalQty, 4);
                    }

                    DateTime date = group.Key;
                    totalQtyDicValue.Add(date.ToString("M/d"), _totalQty);
                    reachQtyDicValue.Add(date.ToString("M/d"), reachQty);
                    closeQtyDicValue.Add(date.ToString("M/d"), _closeQty);
                    comRateDicValue.Add(date.ToString("M/d"), comRate);
                    closeRateDicValue.Add(date.ToString("M/d"), _closeRate);
                }
            }
            else if (chartDisplayMode == ChartDisplayMode.Week || chartDisplayMode == ChartDisplayMode.Montth || chartDisplayMode == ChartDisplayMode.Year)
            {
                var datas = list.OrderBy(p => p.Week).GroupBy(p => chartDisplayMode == ChartDisplayMode.Week ? p.Week : (chartDisplayMode == ChartDisplayMode.Montth ? p.Month : p.Year));
                foreach (var group in datas)
                {
                    double comRate = 0;
                    double _closeRate = 0;
                    double _totalQty = 0;
                    double reachQty = 0;
                    double _closeQty = 0;
                    _totalQty = group.Where(p => p.RowName == "工单总数".L10N()).Sum(p => p.Data);
                    reachQty = group.Where(p => p.RowName == FINISHED_ONTIME_NUMBER.L10N()).Sum(p => p.Data);
                    _closeQty = group.Where(p => p.RowName == ORDER_FINISHED_NUMBER.L10N()).Sum(p => p.Data);
                    if (_totalQty != 0)
                    {
                        comRate = Math.Round(reachQty / _totalQty, 4);
                        _closeRate = Math.Round(_closeQty / _totalQty, 4);
                    }

                    totalQtyDicValue.Add(group.Key.L10N(), _totalQty);
                    reachQtyDicValue.Add(group.Key.L10N(), reachQty);
                    closeQtyDicValue.Add(group.Key.L10N(), _closeQty);
                    comRateDicValue.Add(group.Key.L10N(), comRate);
                    closeRateDicValue.Add(group.Key.L10N(), _closeRate);
                }
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 客制按RowName列排序
        /// </summary>
        /// <param name="sender">s</param>
        /// <param name="e">e</param>
        private void pivotGrid_CustomFieldSort(object sender, PivotCustomFieldSortEventArgs e)
        {
            if (e.Field.FieldName == "RowName")
            {
                e.Result = e.Value1.ToString().Length < e.Value2.ToString().Length ? 1 : 0;
                e.Handled = true;
            }
        }
    }
}
