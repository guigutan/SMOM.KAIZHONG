using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;

namespace SIE.Wpf.MES.BatchProductRoutings
{
    /// <summary>
    /// 批次产品工艺路线实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class BatchExtViewConfig : WPFViewConfig<WipBatch>
    {
        /// <summary>
        /// 批次产品工艺路线视图组
        /// </summary>
        public const string BatchProductViewGroup = "BatchProductViewGroup";

        #region 生产批次 WipBatchNo
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> WipBatchNoPropertyExt = P<WipBatch>.RegisterExtensionReadOnly("WipBatchNoExt", typeof(BatchExtViewConfig),
            GetWipBatchNo, WipBatch.IdProperty);

        /// <summary>
        /// 生产批次
        /// </summary>
        public static string GetWipBatchNo(WipBatch me)
        {
            return me.IsChild ? (me as SubWipBatch).WipBatch?.BatchNo : me.BatchNo;
        }
        #endregion

        #region 批次号 SubBatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> SubBatchNoPropertyExt = P<WipBatch>.RegisterExtensionReadOnly("SubBatchNoExt", typeof(BatchExtViewConfig),
            GetSubBatchNo, WipBatch.IdProperty);

        /// <summary>
        /// 批次号
        /// </summary>
        public static string GetSubBatchNo(WipBatch me)
        {
            return me.IsChild ? me.BatchNo : "";
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchProductViewGroup);
            if (ViewGroup == BatchProductViewGroup)
                ConfigBatchProductView();
        }

        /// <summary>
        /// 配置批次产品视图
        /// </summary>
        private void ConfigBatchProductView()
        {
            View.AssignAuthorize(typeof(BatchWipProductRouting));
            using (View.OrderProperties())
            {
                View.Property(WipBatchNoPropertyExt).ShowInList();
                View.Property(SubBatchNoPropertyExt).ShowInList();
                View.Property(p => p.WorkOrderNo).ShowInList();
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.RemainQty).ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}