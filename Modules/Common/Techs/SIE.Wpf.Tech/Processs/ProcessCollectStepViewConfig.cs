using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Wpf.Resources;
using SIE.Wpf.Tech.Processs.Commands;

namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// 视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class ProcessCollectStepViewConfig : WPFViewConfig<ProcessCollectStep>
    {
        /// <summary>
        /// 工艺路线维护工序的采集步骤视图
        /// </summary>
        public const string ProcessCollectStepView = "ProcessCollectStepView";

        #region 出入类型只读 IsReadOnly
        /// <summary>
        /// 出入类型只读
        /// </summary> 
        public static readonly Property<bool> IsReadOnlyProperty = P<ProcessCollectStep>.RegisterExtensionReadOnly("IsReadOnly", typeof(ProcessCollectStepViewConfig),
            GetIsReadOnly, ProcessCollectStep.PlugTypeProperty);

        /// <summary>
        /// 出入类型只读
        /// </summary>
        /// <param name="me">工序采集步骤</param>
        /// <returns>返回false</returns>
        public static bool GetIsReadOnly(ProcessCollectStep me)
        {
            if (me.PlugType == null || me.PlugType == PlugType.In)
                me.IsGenerateBatch = false;
            return false;
        }
        #endregion 

        #region 批次类型过滤 BatchTypeFilter
        /// <summary>
        /// 批次类型过滤
        /// </summary>
        public static readonly Property<string> BatchTypeFilterProperty = P<ProcessCollectStep>.RegisterExtensionReadOnly("BatchTypeFilter", typeof(ProcessCollectStepViewConfig),
            GetBatchTypeFilter, ProcessCollectStep.IdProperty);

        /// <summary>
        /// 批次类型过滤
        /// </summary>
        /// <param name="me">工序采集步骤</param>
        /// <returns>批次工序返回Batch，否则返回Single</returns>
        public static string GetBatchTypeFilter(ProcessCollectStep me)
        {
            if (me.Process.Type.HasValue && me.Process.Type.Value >= ProcessType.BatchAssembly)
                return "Batch";
            else
                return "Single";
        }
        #endregion

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProcessCollectStepView);
            if (ViewGroup == ProcessCollectStepView)
                ConfigProcessCollectStepView();
        }

        /// <summary>
        /// 配置工艺路线维护工序的采集步骤视图
        /// </summary>
        private void ConfigProcessCollectStepView()
        {
            View.UseCommands(typeof(ProcessCollectStepPopupAddCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.UseCommands(typeof(ProcessCollectStepUpCommand), typeof(ProcessCollectStepDownCommand));
            View.RemoveCommands(typeof(RedoCommand), typeof(UndoCommand), typeof(ListCopyCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.BarcodeType).UseCustomEnumEditor(p => p.CategoryProperty = BatchTypeFilterProperty).ShowInList();
                View.Property(p => p.PlugType).ShowInList().Readonly(IsReadOnlyProperty);
                View.Property(p => p.IsGenerateBatch).ShowInList().Readonly(p => p.PlugType != PlugType.Out);
                View.Property(p => p.IsUnbound).ShowInList();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.ReplaceCommands(typeof(ListAddCommand), typeof(ProcessCollectStepPopupAddCommand));
            View.UseCommands(typeof(ProcessCollectStepUpCommand), typeof(ProcessCollectStepDownCommand));
            View.RemoveCommands(typeof(RedoCommand), typeof(UndoCommand), typeof(ListCopyCommand), typeof(ListSaveCommand));
            View.Property(p => p.BarcodeType).UseCustomEnumEditor(p => p.CategoryProperty = BatchTypeFilterProperty);
            View.Property(p => p.PlugType).Readonly(IsReadOnlyProperty);
            View.Property(p => p.IsGenerateBatch).Readonly(p => p.PlugType != PlugType.Out);
            View.Property(p => p.IsUnbound);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(typeof(ListAddCommand), typeof(ProcessCollectStepPopupAddCommand));
            View.UseCommands(typeof(ProcessCollectStepUpCommand), typeof(ProcessCollectStepDownCommand));
            View.RemoveCommands(typeof(RedoCommand), typeof(UndoCommand));
            View.Property(p => p.BarcodeType).UseCustomEnumEditor(p => p.CategoryProperty = BatchTypeFilterProperty);
            View.Property(p => p.PlugType).Readonly(IsReadOnlyProperty);
            View.Property(p => p.IsGenerateBatch).Readonly(p => p.PlugType != PlugType.Out);
            View.Property(p => p.IsUnbound);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.BarcodeType);
            View.Property(p => p.PlugType);
            View.Property(p => p.IsGenerateBatch);
            View.Property(p => p.IsUnbound);
        }
    }
}