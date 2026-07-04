using SIE.Kit.MES.CallMaterials;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 物料退料视图配置类
    /// </summary>
    internal class CallMaterialWithdrawalViewConfig : WebViewConfig<CallMaterialWithdrawal>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 视图配置
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemLabel).Readonly();
                View.Property(p => p.RemainQty).Readonly();
                View.Property(p => p.WithdrawalQty).Readonly();
                View.Property(p => p.BatchNo).Readonly();
                View.Property(p => p.WithdrawalDate).Readonly();
                View.Property(p => p.WithdrawalByName).HasLabel("退料人员").Readonly();
                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
                View.Property(p => p.StationName).HasLabel("工位名称").Readonly();
                View.Property(p => p.ResourceName).HasLabel("资源名称").Readonly();
                View.Property(p => p.WorkOrderNo).Readonly();
                View.Property(p => p.WarehouseName).HasLabel("接收仓库名称").Readonly();
                View.Property(p => p.WorkShopName).HasLabel("车间").Readonly();
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemLabel);
                View.Property(p => p.BatchNo);
                View.Property(p => p.ItemCode).HasLabel("物料编码").Readonly(false);
                View.Property(p => p.WorkOrderNo).Readonly(false);
                View.Property(p => p.ResourceCode).HasLabel("资源编码").Readonly(false);
            }
        }
    }
}
