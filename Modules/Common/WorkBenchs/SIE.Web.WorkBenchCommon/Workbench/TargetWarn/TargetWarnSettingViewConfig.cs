using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.Web.Command;
using SIE.MetaModel.View;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Web.WorkBenchCommon._Extensions_;
using SIE.Web.WorkBenchCommon.Workbench.TargetWarn.Commands;

namespace SIE.Web.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定视图配置
    /// </summary>
    internal class TargetWarnSettingViewConfig : WebViewConfig<TargetWarnSetting>
	{
		/// <summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.HasDelegate(TargetWarnSetting.NameProperty);
            View.FormEdit();
		}
		
		/// <summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Code);
			View.Property(p => p.Name);
            View.ChildrenProperty(p => p.TargetWarnDetailList).UseViewGroup(TargetWarnDetailViewConfig.ReadonlyView).HasLabel("达成率目标设定");
        }
		
		/// <summary>
		/// 配置明细视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
            View.UseCommands(typeof(SaveTargetWarnSettingCommand).FullName);
            View.Property(p => p.Code).UseDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingCodeDic(); });
            View.Property(p => p.Name).UseKpiDropDownEditor(() => { return RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingNameDic(string.Empty); });
            View.ChildrenProperty(p => p.TargetWarnDetailList).HasLabel("达成率目标设定").UseViewGroup(TargetWarnDetailViewConfig.ListView).LazyLoad(false);
        }
    }
}
