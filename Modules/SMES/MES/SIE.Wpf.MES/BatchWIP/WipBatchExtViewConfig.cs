using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Moves;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 生产批次视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class WipBatchExtViewConfig : WPFViewConfig<WipBatch>
    {
        #region 是否生产批次 IsWipBatch
        /// <summary>
        /// 是否生产批次
        /// </summary> 
        public static readonly Property<bool> IsWipBatchProperty = P<WipBatch>.RegisterExtensionReadOnly("IsWipBatch", typeof(WipBatchExtViewConfig),
            GetIsWipBatch, WipBatch.IsChildProperty);

        /// <summary>
        /// 是否生产批次
        /// </summary>
        public static bool GetIsWipBatch(WipBatch me)
        {
            return !me.IsChild;
        }
        #endregion

        /// <summary>
        /// 批次选择视图
        /// </summary>
        public static readonly string BatchSelectView = "BatchSelectView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchSelectView);
            if (ViewGroup == BatchSelectView)
                ConfigBatchSelectView();
        }

        /// <summary>
        /// 配置批次选择视图
        /// </summary>
        private void ConfigBatchSelectView()
        {
            View.AssignAuthorize(typeof(Assemblys.BatchAssemblyViewModel), typeof(BatchInspectViewModel), typeof(BatchMoveViewModel));
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrder.No).HasLabel("工单编号").Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).Show(ShowInWhere.All);
                View.Property(p => p.Qty).HasLabel("批次数量").Show(ShowInWhere.All);
                View.Property(IsWipBatchProperty).HasLabel("是否生产批次").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrder.PlanBeginDate).HasLabel("计划开始时间").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrder.PlanEndDate).HasLabel("计划结束时间").Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BatchList).Show(ChildShowInWhere.Hide);
            }
        }
    }

    /// <summary>
    /// 生产批次查询实体
    /// </summary>
    internal class WipBatchCriteriaViewConfig : WPFViewConfig<WipBatchCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.BatchNo).Show(ShowInWhere.All);
            }
        }
    }
}