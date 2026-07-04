using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Web.WorkBenchCommon._Extensions_;
using SIE.Web.WorkBenchCommon.Workbench.KPI.Commands;
using SIE.WorkBenchCommon.Workbench.KPI;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// KPI目标设定视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class QuotaTargetSettingViewConfig : WebViewConfig<QuotaTargetSetting>
    {

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
            View.AddBehavior("SIE.Web.WorkBenchCommon.Workbench.KPI.Behaviors.QuotaTargetSettingBehavior");
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.DataType);
            View.Property(p => p.Dimension).UseEnumEditor();
            View.Property(p => p.EntType).UseEnumEditor();
            View.Property(p => p.Enterprise).UsePagingLookUpEditor();
            View.ChildrenProperty(p => p.QuotaTargetDetailList).UseViewGroup(QuotaTargetDetailViewConfig.ReadonlyView).LazyLoad(false);

        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.WorkBenchCommon.Workbench.KPI.Behaviors.QuotaTargetSettingEditBehavior");
            View.UseCommands(typeof(SaveDetailCommand).FullName);
            View.HasDetailColumnsCount(4);
            View.Property(p => p.Dimension).UseEnumEditor();
            View.Property(p => p.EntType).Readonly(p => p.Dimension != KPIDimension.Enterprise)
                .UseListSetting(e => { e.HelpInfo = "指标维度为库存组织可编辑"; });
            View.Property(p => p.Enterprise).UsePagingLookUpEditor(e => { e.ReloadDataOnPopping = true; }).UseDataSource((o, p, e) =>
                {
                    var item = o as QuotaTargetSetting;
                    if (!item.EntType.HasValue) return new EntityList<Enterprise>();
                    var rst = RT.Service.Resolve<EnterpriseController>().GetEnterprises(item.EntType.Value, p, e);
                    rst.ForEach(c => c.TreePId = null);//不需要层级关系
                    return rst;
                }).Readonly(p => p.Dimension != KPIDimension.Enterprise)
                .UseListSetting(e => { e.HelpInfo = "指标维度为库存组织可编辑"; });
            View.Property(p => p.Code).HasLabel("指标分类").UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingCodeDic(); });
            View.Property(p => p.Name).HasLabel("指标名称").UseKpiDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingNameDic(string.Empty); });
            View.Property(p => p.DataType).HasLabel("周期类型").UseEnumEditor(p => { p.FilterCategoery = "NEW"; }).ShowInList();
            View.Property(p => p.ValueType).UseEnumEditor();
            View.ChildrenProperty(p => p.QuotaTargetDetailList).UseViewGroup(QuotaTargetDetailViewConfig.ListView).LazyLoad(false);
        }
    }
}