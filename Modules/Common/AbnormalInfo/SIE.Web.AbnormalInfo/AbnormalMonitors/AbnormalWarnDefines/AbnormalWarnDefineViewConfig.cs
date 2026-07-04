using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Resources.Employees;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常预警定义视图配置
	/// </summary>
	internal class AbnormalWarnDefineViewConfig : WebViewConfig<AbnormalWarnDefine>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.InlineEdit();
			View.UseDefaultCommands(); 
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.JoinEmployeeNames).Show().UsePagingLookUpGridPopupEditor(p =>
			{
				p.AllowBlank = false;
				p.LinkField = AbnormalWarnDefine.JoinEmployeeIdsProperty.Name;
				p.Model = typeof(Employee).FullName;
				p.DisplayField= Employee.NameProperty.Name;
				p.XType = "employeecombopopup";
				p.Editable = false;
			});
			View.Property(p => p.PushModuleId).HasLabel("推送模块");
			View.ChildrenProperty(p => p.UpgradeRuleList).HasLabel("推送升级机制");
		}
        protected override void ConfigQueryView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.Name);
		}

        protected override void ConfigSelectionView()
        {
			View.Property(p => p.Code).ShowInList();
			View.Property(p => p.Name).ShowInList();
			View.Property(p => p.PushModuleId).HasLabel("推送模块").ShowInList(); 
		}

    }
}