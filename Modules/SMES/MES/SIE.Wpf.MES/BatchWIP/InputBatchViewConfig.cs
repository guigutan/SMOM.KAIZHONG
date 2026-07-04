using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Commands;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Inspects.Commands;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.BatchWIP.Packings;
using SIE.Wpf.MES.BatchWIP.Packings.Commands;
using SIE.Wpf.MES.BatchWIP.Repairs.Commands;
using SIE.Wpf.MES.BatchWIP.ViewBehaviors;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 转入批次视图配置
    /// </summary>
    internal class InputBatchViewConfig : WPFViewConfig<InputBatch>
    {
        /// <summary>
        /// 检验采集转入批次视图组
        /// </summary>
        public static readonly string BatchInspectView = "BatchInspectView";

        /// <summary>
        /// 批次包装视图
        /// </summary>
        public const string BatchPackingView = "BatchPacking";

        /// <summary>
        /// 批次维修视图
        /// </summary>
        public const string BatchRepairingView = "BatchRepairing";

        /// <summary>
        /// 批次过站视图
        /// </summary>

        public const string BatchMoveViewStr = "BatchMoveViewStr";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchInspectView, BatchPackingView, BatchRepairingView);
            View.ClearCommands();
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(ShowSplitQtyEditorBehavior));
            if (ViewGroup == BatchInspectView)
                ConfigBatchInspectView();
            else if (ViewGroup == BatchPackingView)
                ConfigBatchPackingView();
            else if (ViewGroup == BatchRepairingView)
                ConfigBatchRepairingView();
            else if (ViewGroup == BatchMoveViewStr)
            {
                BatchMoveView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchAssemblyViewModel), typeof(BatchMoveViewModel));
            View.InlineEdit();
            View.UseCommands(typeof(ListEditCommand), typeof(RemoveInputBatchCommand));
            View.Property(p => p.BatchNo).Readonly();
            View.Property(p => p.ContainerNo).Readonly();
            View.Property(p => p.SubBatchNo).Readonly();
            View.Property(p => p.Qty).UseSpinEditor(e => { e.MinValue = 0;}).Readonly().UseListSetting(e => e.ListGridWidth = 100);
            View.Property(p => p.InputDate).Readonly().UseListSetting(e => e.ListGridWidth = 160);
            View.Property(p => p.RemainQty).UseSpinEditor(e => { e.MinValue = 0; }).Readonly().UseListSetting(e => e.ListGridWidth = 100);
            View.Property(p => p.SplitQty).UseBatchSplitQtyEditor().Readonly(p => p.SplitReadOnly);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DefectList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置批次检验采集转入视图
        /// </summary>
        public void ConfigBatchInspectView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.AssignAuthorize(typeof(BatchInspectViewModel));
            View.UseCommands(typeof(BatchDefectiveKeyInCommand));
            View.UseCommands(typeof(InputBatchRemoveCommand), typeof(InputBatchSplitCommand), typeof(InputBatchMergeCommand), typeof(InputBatchScrapCommand), typeof(PrintChildBatchCommand), typeof(BatchOutputCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().Show();
                View.Property(p => p.ContainerNo).Readonly().Show();
                View.Property(p => p.RemainQty).HasLabel("当前数量").Readonly().Show();
                View.Property(p => p.Qty).Readonly().Show();
                View.Property(p => p.ScrapQty).Readonly().Show();
                View.Property(p => p.NgQty).Readonly().Show();
                View.Property(p => p.DefectDisplay).Readonly().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DefectList).Show(ChildShowInWhere.Hide);

            }
        }

        /// <summary>
        /// 配置批次检验包装转入视图
        /// </summary>
        private void ConfigBatchPackingView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(Packings.BatchPackingViewModel), typeof(BatchPackingViewModel));
            View.UseCommands(typeof(ListEditCommand), typeof(InputBatchRemoveCommand), typeof(BatchPackingCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().Show();
                View.Property(p => p.ContainerNo).Readonly().Show();
                View.Property(p => p.Qty).UseSpinEditor(e => { e.MinValue = 0; }).Readonly().Show();
                View.Property(p => p.InputDate).Readonly().Show();
                View.Property(p => p.NgQty).UseSpinEditor(e => { e.MinValue = 0; }).Readonly().Show();
                View.Property(p => p.RemainQty).UseSpinEditor(e => { e.MinValue = 0; }).Readonly().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DefectList).Show(ChildShowInWhere.Hide);

            }
        }

        /// <summary>
        /// 配置批次维修转入视图
        /// </summary>
        private void ConfigBatchRepairingView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(Repairs.BatchRepairViewModel));
            View.AddBehavior(typeof(LoadDefectsBehavior));
            View.UseCommands(typeof(InputBatchRemoveCommand), typeof(RepairBatchOutputCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().Show();
                View.Property(p => p.ContainerNo).Readonly().Show();
                View.Property(p => p.RemainQty).HasLabel("当前数量").Readonly().Show();
                View.Property(p => p.Qty).Readonly().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DefectList).Show(ChildShowInWhere.Hide);

            }
        }

        /// <summary>
        /// 批次过站视图
        /// </summary>
        private void BatchMoveView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(BatchMoveViewModel));
            View.UseCommands(typeof(InputBatchRemoveCommand), typeof(InputBatchSplitCommand), typeof(InputBatchMergeCommand), typeof(InputBatchScrapCommand), typeof(PrintChildBatchCommand), typeof(BatchOutputCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().Show();
                View.Property(p => p.ContainerNo).Readonly().Show();
                View.Property(p => p.RemainQty).HasLabel("当前数量").Readonly().Show();
                View.Property(p => p.Qty).Readonly().Show();
                View.Property(p => p.ScrapQty).Readonly().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DefectList).Show(ChildShowInWhere.Hide);

            }
        }
    }
}