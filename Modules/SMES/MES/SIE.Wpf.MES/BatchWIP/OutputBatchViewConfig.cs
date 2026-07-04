using SIE.Domain;
using SIE.Items.Items;
using SIE.MES.BatchWIP;
using SIE.ObjectModel;
using SIE.Wpf.Command;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Commands;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Inspects.Commands;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.BatchWIP.ViewBehaviors;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 转出批次视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class OutputBatchViewConfig : WPFViewConfig<OutputBatch>
    {
        #region 产品等级只读属性 GradeReadOnly
        /// <summary>
        /// 产品等级只读属性
        /// </summary>
        [Label("产品等级只读属性")]
        public static readonly Property<bool> GradeReadOnlyProperty = P<OutputBatch>.RegisterExtensionReadOnly("GradeReadOnly", typeof(OutputBatchViewConfig),
            GetGradeReadOnly, OutputBatch.IsNgProperty);

        /// <summary>
        /// 产品等级只读属性
        /// </summary>
        public static bool GetGradeReadOnly(OutputBatch me)
        {
            bool rtn = false;
            if (me.IsNg)
                rtn = true;
            else
                rtn = false;

            return rtn;
        }
        #endregion

        /// <summary>
        /// 检验采集转入批次视图组
        /// </summary>
        public static readonly string BatchInspectView = "BatchInspectView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchInspectView);
            View.ClearCommands();
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(OutputViewBehavior));
            if (ViewGroup == BatchInspectView)
                ConfigBatchInspectView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchAssemblyViewModel), typeof(BatchMoveViewModel));
            View.InlineEdit();
            View.UseCommands(typeof(ListEditCommand), typeof(GenerateChildBatchCommand), typeof(PrintChildBatchCommand), typeof(RemoveOutputBatchCommand), typeof(BatchOutputCommand));
            View.Property(p => p.ContainerNo).Readonly();
            View.Property(p => p.SubBatchNo).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.BatchNo).Readonly();
            View.ChildrenProperty(p => p.RelationBatchList).Visible(false);
        }

        /// <summary>
        /// 配置批次检验采集转入视图
        /// </summary>
        private void ConfigBatchInspectView()
        {
            View.AssignAuthorize(typeof(BatchInspectViewModel));
            View.ClearCommands();
            View.UseCommands(typeof(ListEditCommand), typeof(GenerateChildBatchCommandInspect), typeof(PrintChildBatchCommand), typeof(RemoveOutputBatchCommand), typeof(BatchOutputCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.ContainerNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SubBatchNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.BatchNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Grade).UseProductGradeLookupEditor(p => { p.DisplayMember = nameof(ProductGrade.Name); }).Show(ShowInWhere.All).Readonly(GradeReadOnlyProperty);
                View.Property(p => p.IsNg).Show(ShowInWhere.All).Readonly();
                View.ChildrenProperty(p => p.RelationBatchList).Visible(false);
            }
        }
    }
}