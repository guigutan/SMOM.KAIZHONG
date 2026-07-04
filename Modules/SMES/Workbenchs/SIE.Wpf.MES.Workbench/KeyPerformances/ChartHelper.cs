using DevExpress.Xpf.Charts;
using SIE.MES.Workbench.KeyPerformances;
using SIE.WorkBenchCommon.Workbench.KPI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES.Workbench.KeyPerformances
{
    public class ChartHelper
    {
        /// <summary>
        /// 创建图表限制值，根据用户Id，图表类型
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="title">限制值名称</param>
        /// <param name="type">目标值类型</param>
        /// <returns>限制线</returns>     
        public ConstantLine CreateConstantLine(string _groupName, string _title, int month, double workshopId)
        {
            QuotaTargetDetail quotaTargetDetail = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaMonthTargetSetting(_groupName, _title, month, workshopId);
            if (quotaTargetDetail == null) return new ConstantLine();
            TargetSetting tg = new TargetSetting() { TargetSettingType = TargetSettingType.PlanReachedPercent, TargetValue = (double)quotaTargetDetail.Target / 100 };
            return CreateConstantLine(tg);
        }

        /// <summary>
        /// 创建图表限制值
        /// </summary>
        /// <param name="setting">目标值设置</param>
        /// <param name="title">限制值名称</param>
        /// <returns>限制线</returns>
        public ConstantLine CreateConstantLine(TargetSetting setting, string title = "目标")
        {
            if (setting == null) return null;
            var targetLine = new ConstantLine(setting.TargetValue, title.L10N())
            {
                Title = new ConstantLineTitle() { Content = setting.TargetValue.ToString("P0"), Padding = new Thickness(-20, 0, 0, 0) },
                Brush = Brushes.Green,
                LineStyle = new LineStyle { DashStyle = new DashStyle(new List<double>() { 5, 5 }, 0) },
                LegendText = title.L10N(),
            };

            targetLine.Title.Foreground = targetLine.Brush;
            return targetLine;
        }

        /// <summary>
        /// 创建Series2D
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="argPath">Argument路径</param>
        /// <param name="valuePath">Value路径</param>
        /// <returns>Series</returns>
        public AreaSeries2D CreateSeries2D(string title, string argPath, string valuePath)
        {
            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = Color.FromRgb(30, 144, 255); //道奇蓝
            var series = new AreaSeries2D()
            {
                DisplayName = title.L10N(),
                Brush = scb,
                Transparency = 0.8,
                Foreground = Brushes.SkyBlue,
                MarkerVisible = true,
                LabelsVisibility = true,
                CrosshairLabelVisibility = true,
                //AnimationAutoStartMode = AnimationAutoStartMode.SetStartState,
                ArgumentScaleType = ScaleType.DateTime,
                ArgumentDataMember = argPath,
                ValueDataMember = valuePath,
                MarkerModel = new CircleMarker2DModel(),
                CrosshairLabelPattern = "{V:P1}",
                ShowInLegend = true
            };

            series.Label = new SeriesLabel
            {
                Background = Brushes.Transparent,
                Opacity = 0.75,
                ConnectorVisible = false,
                ResolveOverlappingMode = ResolveOverlappingMode.JustifyAroundPoint,
                Indent = 20,
                TextPattern = "{V:P1}"
            };

            series.Label.ElementTemplate = CreateLabelElementTemplate();
            return series;
        }

        /// <summary>
        /// 创建X轴可覆盖默认设置
        /// </summary>
        /// <returns>默认设置</returns>
        public AxisLabelResolveOverlappingOptions CreateXOptions()
        {
            AxisLabelResolveOverlappingOptions opt = new AxisLabelResolveOverlappingOptions()
            {
                AllowHide = false,
                AllowRotate = true,
                AllowStagger = true,
                MinIndent = 0
            };

            return opt;
        }

        /// <summary>
        /// 创建Label的样式模板
        /// </summary>
        /// <returns></returns>
        public DataTemplate CreateLabelElementTemplate()
        {
            var template = new DataTemplate();
            FrameworkElementFactory factoryText =
                                new FrameworkElementFactory(typeof(Label));

            factoryText.SetValue(Label.ContentProperty, new Binding("Text"));
            factoryText.SetValue(Label.OpacityProperty, 0.75);
            factoryText.SetValue(Label.BackgroundProperty, Brushes.Transparent);
            template.VisualTree = factoryText;
            return template;
        }

        /// <summary>
        /// 显示下钻视图
        /// </summary>
        /// <param name="element">视图元素</param>
        /// <param name="title">Tab页标题</param>
        /// <param name="viewModelKey">ModuleKey</param>
        public void ShowDetailView(FrameworkElement element, string title, string viewModelKey)
        {

            CRT.Workbench.ShowView(viewModelKey, v =>
           {
               v.Title = title.L10N();
               return element;
           });
        }

        public DevExpress.Xpf.Charts.Range GetYRange(double minValue, double maxValue)
        {
            var range = new DevExpress.Xpf.Charts.Range
            {
                MinValue = minValue,
                MaxValue = maxValue
            };


            AxisY2D.SetAlwaysShowZeroLevel(range, false);

            return range;
        }
    }
}
