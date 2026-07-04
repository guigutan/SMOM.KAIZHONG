using SIE.MES.BatchWIP;

namespace SIE.Web.MES.BatchProductRoutings
{
    /// <summary>
    /// 批次产品工艺路线条码查询视图
    /// </summary>
    public class BatchCriteriaViewConfig : WebViewConfig<BatchCriteria>
    {
        /// <summary>
        /// 批次产品工艺路线视图
        /// </summary>
        public const string BatchProductRoutingView = "BatchProductRoutingView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchProductRoutingView);
            if (ViewGroup == BatchProductRoutingView)
                ConfigBatchProductRoutingView();
        }

        /// <summary>
        /// 批次产品工艺路线 视图配置方法
        /// </summary>
        private void ConfigBatchProductRoutingView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.WipBatchNo).Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).Show(ShowInWhere.Hide);
            }
        }
    }
}