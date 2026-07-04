using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.Wpf.Command;

namespace SIE.Wpf.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定视图配置
    /// </summary>
    internal class TargetWarnSettingViewConfig : WPFViewConfig<TargetWarnSetting>
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
            View.UseCommands(typeof(ListAddCommand), typeof(ListEditCommand), typeof(ListDeleteCommand), typeof(ListSaveCommand));
			View.Property(p => p.Code).UseQuotaCategoryEditor();
			View.Property(p => p.Name).UseQuotaNameEditor(e => { e.UpperLevelProperty = TargetWarnSetting.CodeProperty; e.ReloadDataOnPopping = true; });
            View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
            View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
            View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.TargetWarnDetailList).HasLabel("达成率目标设定");
        }
		
		/// <summary>
		/// 配置明细视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
            View.UseCommands(typeof(FormSaveCommand));
            View.AddBehavior(typeof(TargetWarnSettingChangeQuota));
            View.Property(p => p.Code).UseQuotaCategoryEditor();
            View.Property(p => p.Name).UseQuotaNameEditor(e => { e.UpperLevelProperty = TargetWarnSetting.CodeProperty; e.ReloadDataOnPopping = true; });
            View.AttachChildrenProperty(typeof(TargetWarnDetail), c =>
            {
                var main = c.Parent as TargetWarnSetting;
                if (main != null)
                    return main.TargetWarnDetailList;
                return new EntityList<TargetWarnDetail>();
            }, TargetWarnDetailViewConfig.ComTargetWarn).HasLabel("达成率目标设定").Show(ChildShowInWhere.Detail);
            View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
            View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
            View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
            View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
        }
    }
}
