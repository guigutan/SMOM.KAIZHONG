using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// KpiAnalysis.xaml 的交互逻辑
    /// </summary>
    public partial class KpiAnalysis : ComponentItem
    {
        /// <summary>
        /// 绩效分析组件属性对象
        /// </summary>
        private KpiAnalysisProperty property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public KpiAnalysis()
        {
            InitializeComponent();
            property = UseProperty<KpiAnalysisProperty>();
        }

        /// <summary>
        /// 运行逻辑
        /// </summary>
        protected override void OnRun()
        {
            LoadData();
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        void LoadData()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                List<string> codeList = new List<string>();
                if (!string.IsNullOrEmpty(property.Category))
                {
                    codeList.AddRange(property.Category.Split(';').ToList());
                }

                grid.ItemsSource = RT.Service.Resolve<QuotaTargetSettingController>().GetCurrMonthKpiData(codeList);
            }));
        }

        /// <summary>
        /// 单击设置按钮
        /// </summary>    
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            string key = typeof(QuotaTargetSetting).GetQualifiedName();
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = "KPI目标设定".L10N();
                var ui = new ListUITemplate(typeof(QuotaTargetSetting), ViewConfig.ListView, key).CreateUI();

                return ui;
            });
        }
    }

    /// <summary>
    /// 绩效分析组件属性
    /// </summary>
    public class KpiAnalysisProperty : ComponentProperty
    {
        /// <summary>
        /// 模块
        /// </summary>
        [Description("如：品质类、交期类、效率类.支持多选，以';'分割"), DisplayName("模块")]
        public string Category { get; set; }
    }

    /// <summary>
    /// 预警标识 转换器
    /// </summary>
    public class KipGradeColorConverterMarkupExtension : MarkupExtension, IValueConverter
    {
        static KipGradeColorConverterMarkupExtension Instance = new KipGradeColorConverterMarkupExtension();

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var kpi = value as KpiAnalysisData;
            if (kpi != null)
            {
                if (kpi.KpiGrade == null)
                    return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.GrayBrush });
                if (kpi.KpiGrade == KpiGrade.Good)
                    return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.DarkYellowBrush });
                if (kpi.KpiGrade == KpiGrade.Great)
                    return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.GreenBrush });
                if (kpi.KpiGrade == KpiGrade.Poor)
                    return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.RedBrush });
            }
            return Binding.DoNothing;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 当在派生类中实现时，返回一个对象，该对象作为此标记扩展的目标属性的值。
        /// </summary>
        /// <param name="serviceProvider">标记扩展服务的服务提供者</param>
        /// <returns>对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
    }
}
