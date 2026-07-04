using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.WorkBenchCommon.Workbench.KPI;
using System.Windows.Data;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// KPI目标设定视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class QuotaTargetSettingViewConfig : WPFViewConfig<QuotaTargetSetting>
    {
        #region 企业层级及企业模型是否只读 IsReadOnlyProperty
        /// <summary>
        /// 企业层级及企业模型是否只读
        /// </summary>
        [Label("企业层级只读")]
        public static readonly Property<bool> IsReadOnlyPropertyProperty = P<QuotaTargetSetting>.RegisterExtensionReadOnly("IsReadOnlyProperty", typeof(QuotaTargetSettingViewConfig),
            GetIsReadOnlyProperty, QuotaTargetSetting.DimensionProperty);

        /// <summary>
        /// 企业层级及企业模型是否只读
        /// </summary>
        public static bool GetIsReadOnlyProperty(QuotaTargetSetting me)
        {
            if (me.Dimension == KPIDimension.InvOrg)
            {
                me.EntType = null;
                me.Enterprise = null;
            }
            return me.Dimension != KPIDimension.Enterprise;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(QuotaTargetSetting.NameProperty);
            View.FormEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(QuotaTargetSettingBehavior));
            View.UseCommands(typeof(AddQuotaTargetSettingCommand));
            View.UseCommands(typeof(EditQuotaTargetSettingCommand));
            View.UseCommands(WPFCommandNames.ListDelete);
            View.UseCommands(WPFCommandNames.ListSave);

            View.Property(p => p.Code).UseQuotaCategoryEditor();
            View.Property(p => p.Name).UseQuotaNameEditor(e => { e.UpperLevelProperty = QuotaTargetSetting.CodeProperty; e.ReloadDataOnPopping = true; });
            View.Property(p => p.DataType);
            View.Property(p => p.Dimension).UseEnumEditor();
            View.Property(p => p.EntType).UseEnumEditor();
            View.Property(p => p.Enterprise).UsePagingLookUpEditor();
            View.ChildrenProperty(p => p.QuotaTargetDetailList).IsVisible = false;

            View.AttachChildrenProperty(typeof(QuotaTargetDetail), c =>
            {
                var main = c.Parent as QuotaTargetSetting;
                if (main != null)
                    return main.QuotaTargetDetailList;
                return new EntityList<QuotaTargetDetail>();
            }).HasLabel("年目标值设定").Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(QuotaTargetDetail), c =>
            {
                var main = c.Parent as QuotaTargetSetting;
                if (main != null)
                    return main.QuotaTargetDetailList;
                return new EntityList<QuotaTargetDetail>();
            }, QuotaTargetDetailViewConfig.UnComMonth).HasLabel("月目标值设定").Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(QuotaTargetDetail), c =>
            {
                var main = c.Parent as QuotaTargetSetting;
                if (main != null)
                    return main.QuotaTargetDetailList;
                return new EntityList<QuotaTargetDetail>();
            }, QuotaTargetDetailViewConfig.UnComWeek).HasLabel("周目标值设定").Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            View.AddBehavior(typeof(QuotaTargetSettingBehavior));
            View.UseCommands(typeof(SaveQuotaTargetDetailCommand));
            View.Property(p => p.Dimension).UseEnumEditor();
            View.Property(p => p.EntType).UseEnumEditor().Readonly(IsReadOnlyPropertyProperty)
                .UseListSetting(e => { e.HelpInfo = "指标维度为库存组织可编辑"; });
            View.Property(p => p.Enterprise).UsePagingLookUpEditor(e => { e.ReloadDataOnPopping = true; }).UseDataSource((o, p, e) =>
                {
                    var item = o as QuotaTargetSetting;
                    if (!item.EntType.HasValue) return new EntityList<Enterprise>();
                    var rst = RT.Service.Resolve<EnterpriseController>().GetEnterprises(item.EntType.Value, p, e);
                    return rst;
                }).Readonly(IsReadOnlyPropertyProperty)
                .UseListSetting(e => { e.HelpInfo = "指标维度为库存组织可编辑"; });
            View.Property(p => p.Code).HasLabel("指标分类").UseQuotaCategoryEditor();
            View.Property(p => p.Name).HasLabel("指标名称").UseQuotaNameEditor(e => { e.UpperLevelProperty = QuotaTargetSetting.CodeProperty; e.ReloadDataOnPopping = true; });
            View.Property(p => p.DataType).HasLabel("周期类型").UseEnumEditor(p => { p.Filter = "NEW"; p.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged; }).ShowInList();
            View.Property(p => p.ValueType).UseEnumEditor();
            View.AttachChildrenProperty(typeof(QuotaTargetDetail), c =>
            {
                var main = c.Parent as QuotaTargetSetting;
                if (main != null)
                    return main.QuotaTargetDetailList;
                return new EntityList<QuotaTargetDetail>();
            }, QuotaTargetDetailViewConfig.ComYear).HasLabel("年目标值设定").Show(ChildShowInWhere.Detail);

            View.AttachChildrenProperty(typeof(QuotaTargetDetail), c =>
            {
                var main = c.Parent as QuotaTargetSetting;
                if (main != null)
                    return main.QuotaTargetDetailList;
                return new EntityList<QuotaTargetDetail>();
            }, QuotaTargetDetailViewConfig.ComMonth).HasLabel("月目标值设定").Show(ChildShowInWhere.Detail);

            View.AttachChildrenProperty(typeof(QuotaTargetDetail), c =>
            {
                var main = c.Parent as QuotaTargetSetting;
                if (main != null)
                    return main.QuotaTargetDetailList;
                return new EntityList<QuotaTargetDetail>();
            }, QuotaTargetDetailViewConfig.ComWeek).HasLabel("周目标值设定").Show(ChildShowInWhere.Detail);
        }
    }
}