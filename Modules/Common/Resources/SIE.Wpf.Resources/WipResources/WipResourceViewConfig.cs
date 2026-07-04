using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.WipResources.Commands;

namespace SIE.Wpf.Resources.WipResources
{
    /// <summary>
    /// 生产资源视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class WipResourceViewConfig : WPFViewConfig<WipResource>
    {
        #region 启用状态-未启用 StateStop
        /// <summary>
        /// 启用状态-未启用
        /// 停用后的资源，所有信息（包括页签信息）不能编辑
        /// </summary>
        [Label("未启用")]
        public static readonly Property<bool> StateStopProperty = P<WipResource>.RegisterExtensionReadOnly("StateStop", typeof(WipResourceViewConfig),
            GetStateStop, WipResource.ResourceStateProperty);

        /// <summary>
        /// 启用状态-未启用
        /// </summary>
        /// <param name="me">生产资源</param>
        /// <returns>bool</returns>
        public static bool GetStateStop(WipResource me)
        {
            bool rtn = false;
            if (me.SourceType == SyncSourceType.Enterprise || me.SourceType == SyncSourceType.Equipment)
            {
                rtn = true;
            }
            else if (me.ResourceState == ResourceState.Stop)
            {
                rtn = true;
            }

            return rtn;
        }
        #endregion 启用状态-未启用 StateStop

        #region 启用状态-已启用停用 StateActivedStop
        /// <summary>
        /// 启用状态-已启用
        /// 启用或停用状态，不允许修改
        /// </summary>
        [Label("已启用")]
        public static readonly Property<bool> StateActivedStopProperty = P<WipResource>.RegisterExtensionReadOnly("StateActivedStop", typeof(WipResourceViewConfig),
            GetStateActivedStop, WipResource.ResourceStateProperty);

        /// <summary>
        /// 启用状态-已启用
        /// </summary>
        /// <param name="me">资源暂停</param>
        /// <returns>bool</returns>
        public static bool GetStateActivedStop(WipResource me)
        {
            return me.ResourceState == ResourceState.Actived || me.ResourceState == ResourceState.Diseffect;
        }
        #endregion

        #region 可用状态 IsEnableReadonly
        /// <summary>
        /// 可用状态
        /// </summary>
        [Label("可用状态")]
        public static readonly Property<bool> IsEnableReadonly = P<WipResource>.RegisterExtensionReadOnly("IsEnableReadonly", typeof(WipResourceViewConfig),
            GetIsEnableReadonly, WipResource.ResourceStateProperty);

        /// <summary>
        /// 可用状态
        /// </summary>
        /// <param name="me">生产资源</param>
        /// <returns>bool</returns>
        public static bool GetIsEnableReadonly(WipResource me)
        {
            bool rtn = true;
            if (me.ResourceState != ResourceState.Stop)
            {
                rtn = false;
            }

            return rtn;
        }
        #endregion 可用状态 IsEnableReadonly

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("生产资源").HasDelegate(WipResource.CodeProperty);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(WipResourceEditCommand));
            View.UseCommands(typeof(ListSaveCommand));
            View.UseCommands(typeof(WipResourceExportCommand));
            View.UseCommands(typeof(WipResourceRefreshCommand), typeof(SynWipResSettingCommand));
            View.UseCommands(typeof(WipResourceEnableCommand), typeof(WipResourceStopCommand));

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly();
                View.Property(p => p.Name).Readonly();
                View.Property(p => p.SourceType).Readonly();
                View.Property(p => p.IsOutMade).Readonly(StateActivedStopProperty);
                View.Property(p => p.WorkShopId).UseShopEditor().Readonly(StateStopProperty);
                View.Property(p => p.FactoryId).UseFactoryEditor(p => p.DisplayMember = nameof(Enterprise.Name)).Readonly(StateStopProperty);
                View.Property(p => p.ProcessTechTypeId).Readonly(StateActivedStopProperty);
                View.Property(p => p.SchemeId).UseSchemeLookUpEditor();
                View.Property(p => p.ResourceState).Readonly(true);
                View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1).Readonly(IsEnableReadonly);
                View.Property(p => p.AutomationType).Readonly(StateActivedStopProperty);
                View.Property(p => p.Sequence).UseSpinEditor(e => e.MinValue = 0);
                View.AttachDetailChildrenProperty(typeof(WipResource), (sr) =>
                {
                    return sr.Parent as WipResource;
                }).HasLabel("资源信息").HasOrderNo(10);
            }
        }

        /// <summary>
        /// 表单视图配置(弹窗视图配置)
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDetail(columnCount: 3);

            View.Property(p => p.Code).Readonly(true);
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.SourceType).Readonly();
            View.Property(p => p.IsOutMade).Readonly(StateActivedStopProperty);
            View.Property(p => p.WorkShop).UseShopEditor().HasLabel("所属车间").Show(ShowInWhere.All).Readonly(StateStopProperty);
            View.Property(p => p.Factory).UseFactoryEditor().HasLabel("所属工厂").Show(ShowInWhere.All).Readonly(StateStopProperty);
            View.Property(p => p.ProcessTechType).Readonly(StateActivedStopProperty);
            View.Property(p => p.Scheme).UseSchemeLookUpEditor().HasLabel("日历方案").Show(ShowInWhere.All);
            View.Property(p => p.ResourceState).Readonly(true);
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1).Readonly(IsEnableReadonly);
            View.Property(p => p.TaktTime).Readonly(IsEnableReadonly);
            View.Property(p => p.AutomationType).Readonly(StateActivedStopProperty);
            View.Property(p => p.Sequence).UseSpinEditor(e => e.MinValue = 0);
        }

        /// <summary>
        /// 选择视图配置(选择框视图配置)
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).HasLabel("资源编号").Show(ShowInWhere.All);
            View.Property(p => p.Name).HasLabel("资源名称").Show(ShowInWhere.All);
            View.Property(p => p.SourceType).Show(ShowInWhere.All);
            View.Property(p => p.WorkShop).UseShopEditor().HasLabel("所属车间").Show(ShowInWhere.All);
            View.Property(p => p.ProcessTechType).HasLabel("制程类型").Show(ShowInWhere.All);
        }
    }
}