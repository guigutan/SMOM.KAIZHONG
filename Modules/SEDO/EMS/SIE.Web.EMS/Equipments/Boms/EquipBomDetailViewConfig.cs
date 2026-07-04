using SIE.Domain;
using SIE.EMS.Equipments.Boms;
using SIE.MetaModel.View;
using SIE.Web.EMS.Equipments.Boms.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Boms
{
	/// <summary>
	/// 设备BOM明细视图配置
	/// </summary>
	public class EquipBomDetailViewConfig : WebViewConfig<EquipBomDetail>
	{
		/// <summary>
		/// 设备BOM明细选择视图
		/// </summary>
		public const string EquipBomDetailSelectViewGroup = "EquipBomDetailSelectViewGroup";

		/// <summary>
		/// 设备台账设备BOM明细视图
		/// </summary>
		public const string AccountEquipBomDetailViewGroup = "AccountEquipBomDetailViewGroup";

		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EquipBomDetailSelectViewGroup });
			if (ViewGroup == EquipBomDetailSelectViewGroup)
			{
				EquipBomDetailSelectView();
			}
			if (ViewGroup == AccountEquipBomDetailViewGroup)
			{
				ConfigAccountEquipBomDetailView();
			}
		}

		/// <summary>
		/// 审核视图
		/// </summary>
		protected void EquipBomDetailSelectView()
		{
			View.HasDetailColumnsCount(1);
			View.Property(p => p.EquipBomDetailSelect).UseDataSource((e, c, r) =>
			{
				var entity = e as EquipBomDetail;
				return RT.Service.Resolve<EquipBomController>().GetDowngradeEquipBomDetails(entity, r, c);
			}).HasLabel("目标父备件").Show(ShowInWhere.All);
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseCommands(typeof(AddRootSparePartCommand).FullName);
			View.UseCommands(typeof(InsertChildSparePartCommand).FullName);
			View.UseCommands(WebCommandNames.Edit);
			View.UseCommands(WebCommandNames.Delete);
			View.UseCommands(WebCommandNames.Save);
			View.UseCommands(typeof(UpgradeSparePartCommand).FullName);
			View.UseCommands(typeof(DowngradeSparPartCommand).FullName);
			View.Property(p => p.SparePartCode).Readonly();
			View.Property(p => p.SparePart).UseDataSource((e, c, r) =>
			{				
				return RT.Service.Resolve<EquipBomController>().GetSpareParts(r, c);
			}).UsePagingLookUpEditor((m, e) =>
			{
				Dictionary<string, string> keyValues = new Dictionary<string, string>();
				keyValues.Add(nameof(e.SparePartCode), nameof(e.SparePartCode));
				keyValues.Add(nameof(e.SparePartName), nameof(e.SparePartName));
				keyValues.Add(nameof(e.Specification), nameof(e.Specification));
				keyValues.Add(nameof(e.SparePartType), "OriginalItemCode");
				keyValues.Add(nameof(e.LifeTime), nameof(e.LifeTime));
				keyValues.Add(nameof(e.UseTime), nameof(e.UseTime));
				keyValues.Add(nameof(e.UnitName), nameof(e.UnitName));
                keyValues.Add(nameof(e.StockQty), "GoodNumber");
                m.DicLinkField = keyValues;
				m.DisplayField = EquipBomDetail.SparePartNameProperty.Name;
				m.BindDisplayField = EquipBomDetail.SparePartNameProperty.Name;
			}).ShowInList(width:140).HasLabel("备件名称");
			View.Property(p => p.Specification).Readonly();
			View.Property(p => p.SparePartType).Readonly();
			View.Property(p => p.SparePartQty).HasLabel("数量".L10N()+"*");
			View.Property(p => p.StockQty).Readonly();
			View.Property(p => p.SparePartSite);
			View.Property(p => p.LifeTime).Readonly();
			View.Property(p => p.UseTime).ShowInList(width:140).Readonly();
			View.Property(p => p.UnitName).Readonly();
			View.Property(p => p.EquipBomDetailSelect).Show(ShowInWhere.Hide);
		}

		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
			View.Property(p => p.SparePartCode);
			View.Property(p => p.SparePartName);
			View.Property(p => p.Specification);
            View.Property(p => p.SparePartType);
            View.Property(p => p.SparePartQty);
            View.Property(p => p.UnitName);
        }

		/// <summary>
		/// 配置设备台账设备BOM明细视图
		/// </summary>
		public void ConfigAccountEquipBomDetailView()
		{
			using (View.OrderProperties())
			{
				View.Property(p => p.SparePartCode).Readonly().Show();
				View.Property(p => p.SparePartName).Readonly().Show();
				View.Property(p => p.SparePartType).Readonly().Show();
				View.Property(p => p.SparePartQty).Readonly().Show();
				View.Property(p => p.StockQty).Readonly().Show();
				View.Property(p => p.UnitName).Readonly().Show();
			}
		}
	}
}