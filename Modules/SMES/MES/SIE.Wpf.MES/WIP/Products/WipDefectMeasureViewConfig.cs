using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷维修措施视图配置
    /// </summary>
    internal class WipDefectMeasureViewConfig : WPFViewConfig<WipDefectMeasure>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.MeasureCode);
            View.Property(p => p.MeasureName);
            View.Property(p => p.MeasureDesc);
        }
    }
}