using SIE.MES.BatchWIP;

namespace SIE.Wpf.MES.BatchProductRoutings
{
    /// <summary>
    /// 批次产品工艺路线查询实体视图配置
    /// </summary>
    internal class BatchCriteriaViewConfig : WPFViewConfig<BatchCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.WipBatchNo).Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).Show(ShowInWhere.All);
            }
        }
    }
}