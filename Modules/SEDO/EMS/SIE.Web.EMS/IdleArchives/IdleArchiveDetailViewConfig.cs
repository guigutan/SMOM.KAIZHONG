using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.IdleArchives;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Equipments.Extensions;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.IdleArchives
{
	/// <summary>
	/// 设备清单视图配置
	/// </summary>
	public class IdleArchiveDetailViewConfig : WebViewConfig<IdleArchiveDetail>
	{
		/// <summary>
		/// 编辑视图
		/// </summary>
		public const string EditView = "EditView";

		/// <summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(EditView);
			if (ViewGroup == EditView)
			{
				DetailListView();
			}
		}

		/// <summary>
		/// 明细编辑视图
		/// </summary>
		protected void DetailListView()
		{
			View.UseCommands("SIE.Web.EMS.IdleArchives.Commands.AddIdleArchivesDetailCommand", WebCommandNames.Delete);
			using (View.OrderProperties())
			{
				View.Property(p => p.EquipAccountId).UseDataSource((e, page, code) =>
				{
					var entity = e as IdleArchiveDetail;
					if (entity == null)
					{
						return new EntityList<EquipAccountSelect>();
					}
					return RT.Service.Resolve<IdleArchivesController>().GetEquipAccounts(entity, code, page);
				}).UsePagingLookUpEditor((m, r) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(r.ModelCode), nameof(r.EquipAccount.ModelCode));
					keyValues.Add(nameof(r.ModelName), nameof(r.EquipAccount.ModelName));
					keyValues.Add(nameof(r.EquipAccountName), nameof(r.EquipAccount.Name));
					keyValues.Add(nameof(r.EquipAccountCode), nameof(r.EquipAccount.Code));
					keyValues.Add(nameof(r.FixedAssetsAccountCode), nameof(r.EquipAccount.FixedAssetsAccountCode));
					keyValues.Add(nameof(r.FixedAssetsAccountName), nameof(r.EquipAccount.FixedAssetsAccountName));
					keyValues.Add(nameof(r.Specifications), nameof(r.EquipAccount.Specifications));
					m.DicLinkField = keyValues;
				}).ShowInList(120).HasLabel("设备编码");
				View.Property(p => p.EquipAccountName).ShowInList(120).Readonly();
				View.Property(p => p.ModelCode).ShowInList(120).Readonly();
				View.Property(p => p.ModelName).ShowInList(120).Readonly();
				View.Property(p => p.Specifications).ShowInList(80).Readonly();
				View.Property(p => p.FixedAssetsAccountCode).ShowInList(120).Readonly();
				View.Property(p => p.FixedAssetsAccountName).ShowInList(120).Readonly();
				View.Property(p => p.Deadline).UseDateEditor().ShowInList(140).DefaultValue(null).Readonly(p=>(p.IdleArchiveType!=IdleArchiveType.Archive&& p.IdleArchiveType!= IdleArchiveType.Idle));//闲置和封存是才填写
				View.Property(p => p.WorkshopId).HasLabel("车间").ShowInList(120).UseFactoryWorkshopEditor();
				View.Property(p => p.ResourceId).HasLabel("产线").ShowInList(120).UseWorkShopResourceEditor( workShopIdPropertyName: "WorkshopId");
				View.Property(p => p.Location).ShowInList(80);
				View.Property(p => p.KeeperId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
                }).HasLabel("保管人").ShowInList(100);
			}
		}

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
		{ 	  
			View.DisableEditing(); 
			View.Property(p => p.EquipAccountId).ShowInList(120).HasLabel("设备编码");
			View.Property(p => p.EquipAccountName).ShowInList(120);
			View.Property(p => p.ModelCode).ShowInList(120);
			View.Property(p => p.ModelName).ShowInList(120);
			View.Property(p => p.Specifications).ShowInList(80);
			View.Property(p => p.FixedAssetsAccountCode).ShowInList(120);
			View.Property(p => p.FixedAssetsAccountName).ShowInList(120);
			View.Property(p => p.Deadline).UseDateEditor().ShowInList(130);
			View.Property(p => p.WorkshopId).HasLabel("车间").ShowInList(80);
			View.Property(p => p.ResourceId).HasLabel("产线").ShowInList(80); 
			View.Property(p => p.Location).ShowInList(150) ;
			View.Property(p => p.KeeperId).HasLabel("保管人").ShowInList(70);

		}
		
		///<summary>
		/// 配置明细视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
			View.Property(p => p.EquipAccountId).HasLabel("设备");
			View.Property(p => p.WorkshopId).HasLabel("车间");
			View.Property(p => p.ResourceId).HasLabel("产线");
			View.Property(p => p.Deadline);
			View.Property(p => p.Location);
			View.Property(p => p.KeeperId).HasLabel("保管人");
		}
	}
}