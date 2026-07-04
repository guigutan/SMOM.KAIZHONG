using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.PivotGrid;
using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SIE.Wpf.MES.DashBoard.Reports
{
    /// <summary>
    /// 直通率报表基础控件
    /// Interaction logic for DirectRateBasePivotGridControl.xaml
    /// </summary>
    public partial class DirectRateBasePivotGridControl : UserControl
    {
        /// <summary>
        /// 预警值标题
        /// </summary>
        private const string _alertTitle = "预警值";

        /// <summary>
        /// 目标值标题
        /// </summary>
        private const string _goalTitle = "目标值";

        /// <summary>
        /// 样式规则条数
        /// </summary>
        private int _formatConditionCount;

        /// <summary>
        /// 用于存储对应行数据是否添加样式规则
        /// </summary>
        private Dictionary<string, bool?> _dics = new Dictionary<string, bool?>();

        /// <summary>
        /// 报表控件的ViewModel
        /// </summary>
        private ReportBaseViewModel _reportViewModel { get; set; }

        /// <summary>
        /// 报表控件的ViewModel
        /// </summary>
        public ReportBaseViewModel ReportViewModel
        {
            get { return _reportViewModel; }
            set { _reportViewModel = value; }
        }

        /// <summary>
        /// 样式规则
        /// </summary>
        List<FormatCondition> FormatConditions = new List<FormatCondition>();

        /// <summary>
        /// 值预警行
        /// </summary>
        ConstantLineCollection ConstantLines
        {
            get { return ((XYDiagram2D)chart.Diagram).AxisY.ConstantLinesBehind; }
        }

        /// <summary>
        /// 自定义求和字典（Key:行属性+":"+时间，Value:百分值）
        /// </summary>
        Dictionary<string, decimal> dics;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DirectRateBasePivotGridControl()
        {
            InitializeComponent();

            this.DataContextChanged += (o, e) =>
            {
                ClearFormatConditions();

                _reportViewModel = this.DataContext as ReportBaseViewModel;
                _formatConditionCount = _reportViewModel.CountFormatConditions();

                if (pivotGrid.Fields.Count <= 0)
                {
                    pivotGrid.Fields.AddRange(new ReportHelper().InitPivotGridFields(_reportViewModel.DirectRateList.EntityType));
                    ReportHelper.RestoreLayoutFromXml(pivotGrid, _reportViewModel.LayoutFileName);
                }

                dics = _reportViewModel.GetCustomSummeries();
            };

            this.pivotGrid.CustomCellDisplayText += (o, e) =>
            {
                if (pivotGrid.FormatConditions.Count == 0 && _formatConditionCount * 2 == FormatConditions.Count)
                {
                    FormatConditions.ForEach(p => pivotGrid.AddFormatCondition(p));
                }
            };
        }

        /// <summary>
        /// 初始化预警和目标界限（待调整）
        /// </summary>
        void InitAlertAndGoalLines(double alertValue, double goalValue)
        {
            ConstantLines.Clear();
            ConstantLine alertConstantLine = new ConstantLine(alertValue, _alertTitle.L10N());
            alertConstantLine.BorderThickness = new Thickness(2);
            alertConstantLine.Brush = new SolidColorBrush(Colors.Red);
            alertConstantLine.Title = new ConstantLineTitle { Content = _alertTitle + ": " + alertValue.ToString("P2"), ShowBelowLine = true, Foreground = new SolidColorBrush(Colors.Red) };
            alertConstantLine.Title.Padding = new Thickness(0, -10, 0, 0);
            alertConstantLine.Title.ShowBelowLine = true;

            ConstantLine goalConstantLine = new ConstantLine(goalValue, _goalTitle.L10N());
            goalConstantLine.BorderThickness = new Thickness(2);
            goalConstantLine.Brush = new SolidColorBrush(Colors.Green);
            goalConstantLine.Title = new ConstantLineTitle { Content = _goalTitle + ": " + goalValue.ToString("P2"), ShowBelowLine = true, Foreground = new SolidColorBrush(Colors.Green) };
            goalConstantLine.Title.Padding = new Thickness(0, 0, 0, -10);
            goalConstantLine.Title.ShowBelowLine = false;

            ConstantLines.AddRange(new ConstantLine[]
            {
                    alertConstantLine,
                    goalConstantLine
            });

            ////foreach (ConstantLine constantLine in ConstantLines)
            ////    constantLine.Title.Alignment = ConstantLineTitleAlignment.Far;
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
            if (e.Field?.FieldName == DirectRateBaseViewModel.YearProperty.Name)
            {
                e.DisplayText = "{0}年".L10nFormat(e.Value.ToString());
            }
            else if (e.Field?.FieldName == DirectRateBaseViewModel.MonthProperty.Name)
            {
                e.DisplayText = e.Value.ToString().Substring(5).L10N();
            }
            else if (e.Field?.FieldName == DirectRateBaseViewModel.WeekProperty.Name)
            {
                e.DisplayText = e.Value.ToString().Substring(5).L10N();
            }
            else if (e.Field?.FieldName == DirectRateBaseViewModel.DateProperty.Name)
            {
                e.DisplayText = "{0}号".L10nFormat(((DateTime)e.Value).ToString("dd"));
            }
            else
            {
                //
            }

            CreateFormatConditions(e);
        }

        /// <summary>
        /// 初始化样式规则
        /// </summary>
        /// <param name="e">属性DisplayText参数</param>
        private void CreateFormatConditions(PivotFieldDisplayTextEventArgs e)
        {
            if ((e.ValueType == FieldValueType.Value || e.ValueType == FieldValueType.Total) && !e.IsColumn)
            {
                var rowFiledName = e.Field.FieldName;
                var rowFieldValue = e.Value?.ToString();

                string parentFieldName = string.Empty;
                string parentFieldValue = string.Empty;
                string parentExpression = string.Empty;

                //拼接父Field表达式
                var parentField = e.GetHigherLevelFields().FirstOrDefault();
                if (parentField != null)
                {
                    parentFieldName = parentField.FieldName;
                    parentFieldValue = e.GetHigherLevelFieldValue(parentField).ToString();
                    if (parentFieldValue.IsNotEmpty())
                    {
                        parentExpression = " and [{0}] == '{1}'".FormatArgs(parentField.Name, parentFieldValue);
                    }
                }

                //样式是否添加的字典Key，格式为父值+子值（车间名+产线名）
                string key = parentFieldValue + rowFieldValue;
                if (!_dics.ContainsKey(key))
                {
                    FpySetting setting = _reportViewModel.GetFpySetting(rowFiledName, rowFieldValue, parentFieldName, parentFieldValue);
                    _dics[key] = true;

                    if (setting.PersistenceStatus == PersistenceStatus.New)
                    {
                        return;
                    }

                    ////if (parentExpression.IsNullOrEmpty())
                    ////    parentExpression += "and [{0}] is null".FormatArgs(e.fe);

                    var colFields = pivotGrid.Fields.Where(p => p.Area == DevExpress.Xpf.PivotGrid.FieldArea.ColumnArea || p.Area == DevExpress.Xpf.PivotGrid.FieldArea.FilterArea);
                    colFields.ForEach(p =>
                    {
                        FormatCondition alertCondition = new FormatCondition
                        {
                            MeasureName = e.DataField.Name,
                            ApplyToSpecificLevel = true,
                            ValueRule = ConditionRule.Expression,
                            Value1 = true,
                            Expression = "[{0}] == '{1}'{2} and [{3}] <= {4} and ![{3}] is null".FormatArgs(e.Field.Name, rowFieldValue, parentExpression, e.DataField.Name, setting.Alarm / 100),
                            // = "[{0}] <= {1} and ![{0}] is null".FormatArgs(e.DataField.Name, setting.Alarm / 100),
                            Format = new Format { Background = Brushes.Red },
                            RowName = e.Field.Name,
                            ColumnName = p.Name
                        };

                        FormatCondition goalCondition = new FormatCondition
                        {
                            MeasureName = e.DataField.Name,
                            ApplyToSpecificLevel = true,
                            ValueRule = ConditionRule.Expression,
                            Value1 = true,
                            Expression = "[{0}] == '{1}'{2} and [{3}] >= {4} and ![{3}] is null".FormatArgs(e.Field.Name, rowFieldValue, parentExpression, e.DataField.Name, setting.Desired / 100),
                            //Expression = "[{0}] >= {1} and ![{0}] is null".FormatArgs(e.DataField.Name, setting.Desired / 100),
                            Format = new Format { Background = Brushes.Green },
                            RowName = e.Field.Name,
                            ColumnName = p.Name
                        };

                        FormatConditions.Add(goalCondition);
                        FormatConditions.Add(alertCondition);
                    });
                }
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
            var parentFiedName = pivotGrid.SelectedCellInfo.RowValueInfo.Parent?.Field.FieldName;
            var parentFiedValue = pivotGrid.SelectedCellInfo.RowValueInfo.Parent?.Value.ToString();
            if (fieldName.IsNullOrEmpty() || fieldValue.IsNullOrEmpty())
            {
                return;
            }

            var setting = _reportViewModel.GetFpySetting(fieldName, fieldValue, parentFiedName, parentFiedValue);

            if (setting.PersistenceStatus == PersistenceStatus.New)
            {
                return;
            }
            //初始化折线图的预警线和目标线
            InitAlertAndGoalLines((double)setting.Alarm / 100, (double)setting.Desired / 100);
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
        /// 默认设置命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ReportHelper.ResetLayoutToDefault(pivotGrid, _reportViewModel?.LayoutFileName);
        }

        /// <summary>
        /// 目标参数设置命令
        /// </summary>
        /// <param name="sender">命令对象</param>
        /// <param name="e">命令参数</param>
        private void BtnClick_ArgsSetting(object sender, RoutedEventArgs e)
        {
            if (_reportViewModel != null)
            {
                var moduleKey = CRT.Service.Resolve<IFindModule>().FindModuleKey(_reportViewModel.GetType());
                var attr = _reportViewModel.GetType().GetCustomAttributes(typeof(ArgsSettingAttribute), false)[0] as ArgsSettingAttribute;
                new ReportHelper().ShowSettingView(attr.ArgType, ViewConfig.ListView, moduleKey);
            }
        }

        /// <summary>
        /// 自定义折线图数据源
        /// </summary>
        /// <param name="sender">PivotGrid对象</param>
        /// <param name="e">参数</param>
        private void pivotGrid_CustomChartDataSourceRows(object sender, PivotCustomChartDataSourceRowsEventArgs e)
        {
            List<PivotChartDataSourceRow> rows = new List<PivotChartDataSourceRow>();
            e.Rows.ForEach(p =>
            {
                if (p.Value.ToString().IsNotEmpty())
                {
                    rows.Add(p);
                }

                if (p.ColumnValueInfo.Value != null)
                {
                    p.Argument = CustomChartDispalyText(p.ColumnValueInfo);
                }

                if (p.RowValueInfo.ValueType == FieldValueType.Total)
                {
                    p.Series = ((string)p.Series).Replace(" Total", string.Empty);
                }
            });

            if (rows.Count > 0)
            {
                e.Rows.Clear();
                rows.ForEach(p => e.Rows.Add(p));
            }

            chart.Legend?.CustomItems?.Clear();
            chart.Legend?.CustomItems?.Add(new CustomLegendItem
            {
                Text = e.Rows[0].Series.ToString().L10N(),
                MarkerBrush = Brushes.SkyBlue
            });

            //自定义Y轴范围
            CustomAxisYRange(e);
        }

        /// <summary>
        /// 自定义Y轴范围
        /// </summary>
        /// <param name="e">事件参数</param>
        private void CustomAxisYRange(PivotCustomChartDataSourceRowsEventArgs e)
        {
            if (e.Rows.All(p => p.Value.ToString().IsNotEmpty()))
            {
                var maxValue = double.Parse(e.Rows.Max(p => p.Value).ToString());
                var minValue = double.Parse(e.Rows.Min(p => p.Value).ToString());
                var range = new DevExpress.Xpf.Charts.Range
                {
                    MaxValue = maxValue > 0.9 ? 1 : maxValue + 0.1,
                    MinValue = minValue > 0.1 ? minValue - 0.1 : 0,
                };

                _AxisY.WholeRange = range;
                _AxisY.VisualRange = range;
            }
        }

        /// <summary>
        /// 自定义折线图X轴DisplayText
        /// </summary>
        /// <param name="columnValueInfo">列值信息</param>
        /// <returns>显示字符串</returns>
        private string CustomChartDispalyText(PivotFieldValueEventArgs columnValueInfo)
        {
            switch (columnValueInfo.Field.FieldName)
            {
                case nameof(DirectRateBaseViewModel.Year):
                    return "{0}年".L10nFormat(columnValueInfo.Value.ToString());
                case nameof(DirectRateBaseViewModel.Month):
                    return "{0}".L10nFormat(columnValueInfo.Value.ToString());
                case nameof(DirectRateBaseViewModel.Week):
                    return "{0}".L10nFormat(columnValueInfo.Value.ToString());
                case nameof(DirectRateBaseViewModel.Date):
                    return ((DateTime)columnValueInfo.Value).ToString("MM-dd");
                default:
                    return ((DateTime)columnValueInfo.Value).ToString("MM-dd");
            }
        }

        /// <summary>
        /// 清除样式规则
        /// </summary>
        private void ClearFormatConditions()
        {
            _dics.Clear();
            FormatConditions.Clear();
            pivotGrid.FormatConditions.Clear();
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">控件参数</param>
        private void pivotGrid_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            if ((e.RowValueType == FieldValueType.Value || e.RowValueType == FieldValueType.Total) && e.ColumnValueType != FieldValueType.GrandTotal)
            {
                if (!_reportViewModel.IsOpenDefectStatisticsChart(e.RowField.FieldName))
                    return;

                var selectedCellInfo = pivotGrid.SelectedCellInfo;
                var rowFieldValue = selectedCellInfo.RowValueInfo.Value?.ToString();
                var parentFiedName = selectedCellInfo.RowValueInfo.Parent?.Field.FieldName;
                var parentFiedValue = selectedCellInfo.RowValueInfo.Parent?.Value.ToString();

                var colFieldValue = selectedCellInfo.ColumnValueInfo.Value?.ToString();

                string fpyTitle = string.Empty;
                if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(DirectRateBaseViewModel.Date))
                {
                    fpyTitle = DateTime.Parse(colFieldValue).ToString("yyyy年MM月dd日");
                }
                else if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(DirectRateBaseViewModel.Year))
                {
                    fpyTitle = colFieldValue + "年";
                }
                else if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(DirectRateBaseViewModel.Month))
                {
                    fpyTitle = colFieldValue;
                }
                else if (selectedCellInfo.ColumnValueInfo.Field.FieldName == nameof(DirectRateBaseViewModel.Week))
                {
                    fpyTitle = colFieldValue;
                }
                else
                {
                    //
                }

                var criteria = new DefectStatisticsCriteria
                {
                    RowFieldName = e.RowField.FieldName,
                    RowFieldValue = rowFieldValue,
                    RowParentFieldName = parentFiedName,
                    RowParentFieldValue = parentFiedValue,
                    ColumnFieldName = e.ColumnField.FieldName,
                    ColumnFieldValue = colFieldValue
                };

                ////new ReportHelper().ShowProcessFpyView( _reportViewModel.GetType(), _reportViewModel.GetDefectTopDataSource(criteria), _reportViewModel.GetProcessFpyDataSource(criteria), e.RowField.Caption);
                new ReportHelper().ShowProcessFpyView(_reportViewModel, criteria, e.RowField.Caption, "{0}工序直通率统计图({1})".L10nFormat(rowFieldValue, fpyTitle));
            }
        }

        /// <summary>
        /// 自定义单元格值
        /// </summary>
        /// <param name="sender">PivotGrid控件</param>
        /// <param name="e">单元格事件参数</param>
        private void pivotGrid_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            //当时最低层级且按日期排布的话则不进行自定义
            if (e.RowField == null || e.ColumnField == null
                || e.RowField?.AreaIndex == 1 && e.ColumnField?.FieldName == nameof(DirectRateBaseViewModel.Date))
            {
                return;
            }

            //若行属性为自定义时，则去相应Key的自定义值
            if (e.RowField?.SummaryType == FieldSummaryType.Custom && dics != null)
            {
                var parentField = e.GetRowFields().FirstOrDefault(p => p.AreaIndex == e.RowField.AreaIndex - 1);
                string parentRowFieldValue = string.Empty;
                if (parentField != null)
                {
                    parentRowFieldValue += ":" + e.GetFieldValue(parentField);
                }

                var rowFieldValue = e.GetFieldValue(e.RowField);
                var colFieldValue = e.GetFieldValue(e.ColumnField);
                if (dics.ContainsKey(colFieldValue + parentRowFieldValue + ":" + rowFieldValue))
                {
                    e.Value = dics[colFieldValue + parentRowFieldValue + ":" + rowFieldValue];
                }

                e.Handled = true;
            }
        }
    }
}
