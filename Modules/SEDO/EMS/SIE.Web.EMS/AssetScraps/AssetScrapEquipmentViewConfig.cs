using SIE.Domain;
using SIE.EMS.AssetScraps;
using SIE.EMS.Equipments;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetScraps
{
	/// <summary>
	/// 报废申请设备清单视图配置
	/// </summary>
	public class AssetScrapEquipmentViewConfig : WebViewConfig<AssetScrapEquipment>
	{
		/// <summary>
		/// 设备清单编辑视图
		/// </summary>
		public const string EditAssetScrapEquipmentViewGroup = "EditAssetScrapEquipmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetScrapEquipmentViewGroup });

			if (ViewGroup == EditAssetScrapEquipmentViewGroup)
			{
				ConfigEditAssetScrapEquipmentView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			string toolTip = "汇总近一年数据".L10N();
			View.DisableEditing();
			View.Property(p => p.EquipAccountCode).ShowInList(width: 120);
			View.Property(p => p.EquipAccountName);
			View.Property(p => p.Alias);
			View.Property(p => p.EquipModelCode);
			View.Property(p => p.EquipModelName);
			View.Property(p => p.Specifications);
			View.Property(p => p.FixedAssetsAccountCode);
			View.Property(p => p.FixedAssetsAccountName);
			View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType; e.CatalogReloadData = true; });
			View.Property(p => p.Reason);
			View.Property(p => p.UsefulLife);
			View.Property(p => p.OriginalAssetsValue);
			View.Property(p => p.ScrapNetValue);
			View.Property(p => p.RepairHours).UseSpinEditor(m => m.DecimalPrecision = 2).UseListSetting(m=>m.HelpInfo= toolTip).ShowInList(width:130);
			View.Property(p => p.MaintenanceHours).UseSpinEditor(m => m.DecimalPrecision = 2).UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130);
			View.Property(p => p.SparePartCost).UseSpinEditor(m => m.DecimalPrecision = 2).UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130);
			View.Property(p => p.OutRepairCost).UseSpinEditor(m => m.DecimalPrecision = 2).UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130);
			View.Property(p => p.TotalRepairHours).UseSpinEditor(m => m.DecimalPrecision = 2).UseListSetting(m => m.HelpInfo = "汇总工时".L10N()).ShowInList(width: 130);
			View.Property(p => p.TotalSparePartCost).UseSpinEditor(m => m.DecimalPrecision = 2).UseListSetting(m => m.HelpInfo = "汇总备件成本".L10N()).ShowInList(width: 130);
		}

		///<summary>
		/// 配置设备清单编辑视图
		/// </summary>
		protected void ConfigEditAssetScrapEquipmentView()
		{
			string toolTip = "汇总近一年数据".L10N();
			View.AddBehavior("SIE.Web.EMS.AssetScraps.Behaviors.AssetScrapEquipmentDetailsBehavior");
            View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.AddAssetScrapEquipmentCommand");
            View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.DeleteAssetScrapEquipmentCommand");
            using (View.OrderProperties())
            {
				View.Property(p => p.EquipAccountId).UseDataSource((e, c, r) =>
					{
						var entity = e as AssetScrapEquipment;
						return RT.Service.Resolve<EquipController>().GetEquipAccounts(c, entity, r);
					}).UsePagingLookUpEditor((m, e) =>
					{
						Dictionary<string, string> keyValues = new Dictionary<string, string>();
						keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
						keyValues.Add(nameof(e.Alias), nameof(e.EquipAccount.Alias));
						keyValues.Add(nameof(e.EquipModelCode), nameof(e.EquipAccount.ModelCode));
						keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipAccount.ModelName));
						keyValues.Add(nameof(e.Specifications), nameof(e.EquipAccount.Specifications));
						keyValues.Add(nameof(e.FixedAssetsAccountCode), nameof(e.EquipAccount.FixedAssetsAccountCode));
						keyValues.Add(nameof(e.FixedAssetsAccountName), nameof(e.EquipAccount.FixedAssetsAccountName));
						keyValues.Add(nameof(e.UsefulLife), nameof(e.EquipAccount.UsefulLife));
						keyValues.Add(nameof(e.OriginalAssetsValue), nameof(e.EquipAccount.OriginalAssetsValue));
						m.DicLinkField = keyValues;
					}).HasLabel("设备编码").ShowInList(width:120).Show();
				View.Property(p => p.ScrapType).HasLabel("报废类型".L10N()+"*").UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType; e.CatalogReloadData = true; }).Show();
				View.Property(p => p.Reason).HasLabel("报废原因".L10N() + "*").Show();
				View.Property(p => p.EquipAccountName).Readonly().Show();
				View.Property(p => p.Alias).Readonly().Show();
				View.Property(p => p.EquipModelCode).Readonly().Show();
				View.Property(p => p.EquipModelName).Readonly().Show();
                View.Property(p => p.Specifications).Readonly().Show();
				View.Property(p => p.FixedAssetsAccountCode).Readonly().Show();
				View.Property(p => p.FixedAssetsAccountName).Readonly().Show();
				View.Property(p => p.UsefulLife).Readonly().Show();
				View.Property(p => p.OriginalAssetsValue).Readonly().Show();
				View.Property(p => p.ScrapNetValue).UseSpinEditor(m => m.DecimalPrecision = 2).Show();
				View.Property(p => p.RepairHours).DefaultValue(0).Readonly()
					.UseSpinEditor(m => m.DecimalPrecision = 2)
					.UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130).Show();
				View.Property(p => p.MaintenanceHours).DefaultValue(0).Readonly()
					.UseSpinEditor(m => m.DecimalPrecision = 2)
					.UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130).Show();
				View.Property(p => p.SparePartCost).DefaultValue(0).Readonly()
					.UseSpinEditor(m => m.DecimalPrecision = 2)
					.UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130).Show();
				View.Property(p => p.OutRepairCost).DefaultValue(0).Readonly()
					.UseSpinEditor(m => m.DecimalPrecision = 2)
					.UseListSetting(m => m.HelpInfo = toolTip).ShowInList(width: 130).Show();
				View.Property(p => p.TotalRepairHours).DefaultValue(0).Readonly()
					.UseSpinEditor(m => m.DecimalPrecision = 2)
					.UseListSetting(m => m.HelpInfo = "汇总工时".L10N()).ShowInList(width: 130).Show();
				View.Property(p => p.TotalSparePartCost).DefaultValue(0).Readonly()
					.UseSpinEditor(m => m.DecimalPrecision = 2)
					.UseListSetting(m => m.HelpInfo = "汇总备件成本".L10N()).ShowInList(width: 130).Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
	}
}
