using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷维修措施视图配置
    /// </summary>
    internal class BatchWipDefectMeasureViewConfig : WPFViewConfig<BatchWipDefectMeasure>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion));
            View.ClearCommands();
            View.Property(p => p.MeasureCode);
            View.Property(p => p.MeasureName);
            View.Property(p => p.MeasureDesc);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);

        }
    }
}