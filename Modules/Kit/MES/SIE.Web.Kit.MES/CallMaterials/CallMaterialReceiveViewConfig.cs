using SIE.Kit.MES.CallMaterials;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 物料接收视图配置
    /// </summary>
    internal class CallMaterialReceiveViewConfig : WebViewConfig<CallMaterialReceive>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Label).Readonly();
            View.Property(p => p.ItemCode).HasLabel("物料编码").Readonly();
            View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
            View.Property(p => p.ItemSpecification).HasLabel("规格型号").Readonly();
            View.Property(p => p.BatchNo).Readonly();
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 0).Readonly();
            View.Property(p => p.DistStation).Readonly();
            View.Property(p => p.IsLoadItem).UseCheckEditor().Readonly();
            View.Property(p => p.LoadStation).Readonly();
            View.Property(p => p.RemainQty).Readonly();
            View.Property(p => p.Resource).Readonly();
            View.Property(p => p.WorkShopName).HasLabel("车间").Readonly();
            View.Property(p => p.WorkOrderNo).HasLabel("工单号").Readonly();
            View.Property(p => p.BillNo).HasLabel("叫料单号").Readonly();
            View.Property(p => p.ReceiveBy).Readonly();
            View.Property(p => p.ReceiveDate).ShowInList(width: 120).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Resource);
            View.Property(p => p.WorkOrderNo).HasLabel("工单号").Readonly(false);
            View.Property(p => p.BillNo).HasLabel("叫料单号").Readonly(false);
            View.Property(p => p.ReceiveBy);
            View.Property(p => p.ReceiveDate).UseDateRangeEditor(e => { e.DateFormat = "Y/m/d"; e.DateRangeType = ObjectModel.DateRangeType.Month; });
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}
