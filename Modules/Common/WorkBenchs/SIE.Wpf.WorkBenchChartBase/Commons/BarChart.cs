using DevExpress.Xpf.Charts;
using SIE.WorkBenchChartBase.Commons;
using SIE.WorkBenchChartBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 柱形图
    /// </summary>
    //[ChartDefinitionAttribute("E:\\壁纸\\guanlan_gaoshou-007.jpg", "XYDiagram2DChart", BarChart._title)]
    public class BarChart : LineChart
    {
        private const string _title = "柱形图";

        /// <summary>
        /// 创建Series
        /// </summary>
        protected override List<Series> CreateSeries()
        {
            return new List<Series> { new BarSideBySideSeries2D() };
        }

        /// <summary>
        /// 构建数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        protected override BaseChartViewModel BuildDataContext()
        {
            return new TestChartViewModel { Title = _title.L10N(), ChartAlertLevel = ChartAlertLevel.Red };
        }

        /// <summary>
        /// 新建命令委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //命令委托执行方法
        }

        /// <summary>
        /// 打印图片
        /// </summary>
        /// <returns></returns>
        public byte[] PrintToImage()
        {
            const byte[] data = null;
            return data;
        }
    }
}
