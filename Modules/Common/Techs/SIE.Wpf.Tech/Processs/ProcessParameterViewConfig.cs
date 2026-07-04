using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Wpf.Resources;

namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// 工序参数视图设置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ProcessParameterViewConfig : WPFViewConfig<ProcessParameter>
    {
        /// <summary>
        /// 工艺路线维护工序的工序参数视图
        /// </summary>
        public const string ProcessParameterView = "ProcessParameterView";

        #region 脚本只读 IsReadOnly
        /// <summary>
        /// 脚本只读
        /// </summary> 
        public static readonly Property<bool> IsReadOnlyProperty = P<ProcessParameter>.RegisterExtensionReadOnly("IsReadOnly", typeof(ProcessParameterViewConfig),
            GetIsReadOnly, ProcessParameter.TypeProperty);

        /// <summary>
        /// 脚本只读
        /// </summary>
        /// <param name="me">工序参数</param>
        /// <returns>非客制化结果返回true，否则返回false</returns>
        public static bool GetIsReadOnly(ProcessParameter me)
        {
            return me.Type != ResultTypeForDesign.Custom;
        }
        #endregion

        #region 类型过滤 TypeFilter
        /// <summary>
        /// 类型过滤
        /// </summary>
        public static readonly Property<string> TypeFilterProperty = P<ProcessParameter>.RegisterExtensionReadOnly("TypeFilter", typeof(ProcessParameterViewConfig),
            GetTypeFilter, ProcessParameter.IdProperty);

        /// <summary>
        /// 类型过滤
        /// </summary>
        /// <param name="me">工序参数</param>
        /// <returns>批次检验返回空字符，否则返回common</returns>
        public static string GetTypeFilter(ProcessParameter me)
        {
            if (me.Process.Type.HasValue && me.Process.Type == ProcessType.BatchPqc)
                return string.Empty;
            else
                return "Common";
        }
        #endregion

        /// <summary>
        /// 配置默认视图 
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProcessParameterView);
            if (ViewGroup == ProcessParameterView)
                ConfigProcessParameterView();
        }

        /// <summary>
        /// 配置工序参数视图（工艺连续维护工序使用）
        /// </summary>
        private void ConfigProcessParameterView()
        {
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Type).ShowInList();
                View.Property(p => p.Script).ShowInList().UseMemoEditor().Readonly(IsReadOnlyProperty);
                View.Property(p => p.Description).ShowInList();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListCopy, typeof(ListSaveCommand));
            View.Property(p => p.Type).UseCustomEnumEditor(p => p.CategoryProperty = TypeFilterProperty);
            View.Property(p => p.Script).UseMemoEditor().Readonly(IsReadOnlyProperty);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands(typeof(ListAddCommand), typeof(ListDeleteCommand));
            View.Property(p => p.Type).UseEnumEditor();
            View.Property(p => p.Script).UseMemoEditor().Readonly(IsReadOnlyProperty);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Type);
            View.Property(p => p.Type);
            View.Property(p => p.Script).UseMemoEditor();
            View.Property(p => p.Description);
        }
    }
}