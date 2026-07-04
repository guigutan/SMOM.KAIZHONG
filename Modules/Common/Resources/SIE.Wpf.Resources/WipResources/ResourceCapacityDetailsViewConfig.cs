using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Wpf.Resources.WipResources.Commands;

namespace SIE.Wpf.Resources.WipResources
{
    /// <summary>
    /// 详细资源视图配置类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ResourceCapacityDetailsViewConfig : WPFViewConfig<ResourceCapacityDetails>
    {
        #region 资源数量 ResourceNumberReadonly
        /// <summary>
        /// 资源数量
        /// </summary>
        [Label("资源数量")]
        public static readonly Property<bool> ResourceNumberReadonly = P<ResourceCapacityDetails>.RegisterExtensionReadOnly("ResourceNumberReadonly", typeof(ResourceCapacityDetailsViewConfig),
            GetResourceNumber, ResourceCapacityDetails.WipResourceProperty);

        /// <summary>
        /// 资源数量
        /// </summary>
        /// <param name="me">详细资源实体</param>
        /// <returns>bool</returns>
        public static bool GetResourceNumber(ResourceCapacityDetails me)
        {
            bool rtn = true;
            if (me.WipResource.ResourceType == ResourceType.ClusterResource 
                && me.WipResource.ResourceState != ResourceState.Stop)
            {
                rtn = false;
            }

            return rtn;
        }
        #endregion  资源数量 ResourceNumberReadonly

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            ////base.ConfigView();
            View.DomainName("详细产能").HasDelegate(ResourceCapacityDetails.IdProperty);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.ClearCommands();  //清除所有命令按钮
            View.UseCommands(typeof(ResCapDelEditCommand), typeof(ResCapDelExportCommand));

            using (View.OrderProperties())
            {
                View.Property(p => p.CalendarTime).UseDateEditor().Readonly(); 
                View.Property(p => p.ResourceNumber).UseSpinEditor(e => e.MinValue = 1).Readonly(ResourceNumberReadonly); //只有集群资源类型该值才可编辑
                View.Property(p => p.Efficiency).UseSpinEditor(e => { e.MinValue = 1; e.MaxValue = 100; }).Readonly(WipResourceViewConfig.StateStopProperty);
                View.Property(p => p.ReserveCap).UseSpinEditor(e => { e.MinValue = 0; e.MaxValue = 99; }).Readonly(WipResourceViewConfig.StateStopProperty);
            }
        }
    }
}
