using SIE.Domain;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 保养执行报修视图配置
    /// </summary>
    public class EquipRepairViewModelViewConfig : WebViewConfig<EquipRepairViewModel>
    {
		/// <summary>
		/// 故障信息
		/// </summary>
		public const string ErrorInfoGroup = "ErrorInfoGroup";

		/// <summary>
		/// 保养执行跳转维修
		/// </summary>
		public const string MaintainToRepairGroup = "MaintainToRepairGroup";

		/// <summary>
		/// 配置默认视图
		/// </summary>
		protected override void ConfigView()
        {
            View.FormEdit();
			View.DeclareExtendViewGroup(new string[] { ErrorInfoGroup,MaintainToRepairGroup });
			if (ViewGroup == ErrorInfoGroup)
			{
				View.DomainName("故障信息录入");
				ErrorInfo();
			}
			if (ViewGroup == MaintainToRepairGroup)
				MaintainToRepair();
		}

		/// <summary>
		/// 
		/// </summary>
        protected override void ConfigDetailsView()
        {
            base.ConfigDetailsView();
        }

		/// <summary>
		/// 保养执行维修
		/// </summary>
		public void MaintainToRepair()
		{
			View.AssignAuthorize(typeof(CheckPlanViewModel));
			using (View.OrderProperties())
			{
                View.HasDetailColumnsCount(4);

				#region 设备
				View.Property(p => p.EquipAccountId).UsePagingLookUpEditor((m, e) =>
				{
                    Dictionary<string, string> keyValues = new Dictionary<string, string>()
                    {
                        { nameof(e.EquipAccountName), nameof(e.EquipAccount.Name) },
                        { nameof(e.EquipAccountMode), nameof(e.EquipAccount.EquipModel.Name) },
                        { nameof(e.EquipAccountType), nameof(e.EquipAccount.EquipModel.EquipType.TypeName) },
                        { nameof(e.ResourceName), nameof(e.EquipAccount.Resource.Name) },
                        { nameof(e.UseDepartment), nameof(e.EquipAccount.UseDepartment.Name) },
                        { nameof(e.InstallationLocation), nameof(e.EquipAccount.InstallationLocation) },
                        { nameof(e.ProcessName), nameof(e.EquipAccount.Process.Name) },
                        { nameof(e.WorkShopName), nameof(e.EquipAccount.WorkShop.Name) }
                    };
                    m.DicLinkField = keyValues;
				}).HasLabel("设备编码").Readonly().Show(ShowInWhere.All);
				View.Property(p => p.EquipAccountName).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.EquipAccountMode).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.EquipAccountType).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.ResourceName).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.UseDepartment).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.InstallationLocation).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.ProcessName).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.WorkShopName).Readonly().Show(ShowInWhere.All);
				#endregion

				View.Property(p => p.ApplyRepairEmployeeId).HasLabel("报修人").Readonly().Show(ShowInWhere.All);
				View.Property(p => p.ApplyRepairDate).Readonly().Show(ShowInWhere.All);
				View.AttachDetailChildrenProperty(typeof(EquipRepairViewModel), (c) =>
				{
					var item = c.Parent as EquipRepairViewModel;
					item = RF.GetById<EquipRepairViewModel>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
					return item;
				}, ErrorInfoGroup).HasLabel("故障信息录入").Show(ChildShowInWhere.All).OrderNo=1;
				View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.All).OrderNo=2;
			}
		}

		#region 子页签
		/// <summary>
		/// 故障信息录入
		/// </summary>
		protected void ErrorInfo()
		{
			View.ClearCommands();
			View.HasDetailColumnsCount(2);
			View.AssignAuthorize(typeof(CheckPlanViewModel));
			using (View.OrderProperties())
			{
				View.Property(p => p.ProduceState).Show(ShowInWhere.All);
				View.Property(p => p.DeviceAbnormalId).UseDataSource((e, c, r) =>
				{
					var entity = e as EquipRepairViewModel;
					return RT.Service.Resolve<RepairController>().GetDeviceAbnormal(entity.EquipAccountId, r, c);
				}).Show(ShowInWhere.All).HasLabel("故障现象");
				View.Property(p => p.UrgentDegree).Show(ShowInWhere.All);
				View.Property(p => p.DeviceAbnormalRemark).Show(ShowInWhere.All).ShowInDetail(columnSpan: 1, rowSpan: 2).UseMemoEditor();
				View.Property(p => p.DeviceAbnormalCode).Show(ShowInWhere.All);
			}
		}
		#endregion
	}
}
