using SIE.EMS.AssetDisposals;
using SIE.EMS.SpareParts;
using SIE.MetaModel.View;
using SIE.Web.EMS.AssetDisposals.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetDisposals
{
    /// <summary>
    ///备件回收清单视图配置
    /// </summary>
    public class AssetDisposalSparePartViewConfig : WebViewConfig<AssetDisposalSparePart>
	{
		/// <summary>
		/// 备件回收清单编辑视图
		/// </summary>
		public const string EditAssetDisposalSparePartViewGroup = "EditAssetDisposalSparePartViewGroup";

		/// <summary>
		/// 备件回收条码打印视图
		/// </summary>
		public const string PrintAssetDisposalSparePartSnViewGroup = "PrintAssetDisposalSparePartSnViewGroup";

		/// <summary>
		/// 备件回收序列号新增视图
		/// </summary>
		public const string AddAssetDisposalSparePartSnViewGroup = "AddAssetDisposalSparePartSnViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetDisposalSparePartViewGroup, PrintAssetDisposalSparePartSnViewGroup, AddAssetDisposalSparePartSnViewGroup });

			if (ViewGroup == EditAssetDisposalSparePartViewGroup)
			{
				ConfigEditAssetDisposalSparePartView();
			}

			if (ViewGroup == PrintAssetDisposalSparePartSnViewGroup)
			{
				ConfigPrintAssetDisposalSparePartSnView();
			}

			if (ViewGroup == AddAssetDisposalSparePartSnViewGroup)
			{
				ConfigAddAssetDisposalSparePartSnView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommand(typeof(ImportAssetDisposalSparePartCommand).FullName);
			View.UseCommand("SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
			View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.PrintAssetDisposalSparePartSnCommand");
			View.DisableEditing();
			View.Property(p => p.SparePartCode).ShowInList(width: 120);
			View.Property(p => p.SparePartName);
			View.Property(p => p.Specification);
			View.Property(p => p.SpartType);
			View.Property(p => p.ControlMethod);
			View.Property(p => p.LotNo);
			View.Property(p => p.Sn);
			View.Property(p => p.Qty);
			View.Property(p => p.UnitName);
			View.Property(p => p.QualityStatus);
			View.Property(p => p.Warehouse);
		}

		///<summary>
		/// 配置备件回收清单编辑视图
		/// </summary>
		protected void ConfigEditAssetDisposalSparePartView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalSparePartDetailsBehavior");
			View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalSparePartCommand");
			View.UseCommand(WebCommandNames.Delete);
			View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalSparePartSnCommand");
			using (View.OrderProperties())
            {
				View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
					keyValues.Add(nameof(e.Specification), nameof(e.SparePart.Specification));
					keyValues.Add(nameof(e.SpartType), nameof(e.SparePart.SpartType));
					keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
					keyValues.Add(nameof(e.UnitName), nameof(e.SparePart.UnitName));
					m.DicLinkField = keyValues;
				}).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SparePartController>().GetSpareParts(pagingInfo, keyword);
                }).Cascade(p => p.Sn, null).ShowInList(width:120).Show();
                View.Property(p => p.SparePartName).Readonly().Show();
                View.Property(p => p.Specification).Readonly().Show();
                View.Property(p => p.SpartType).Readonly().Show();
                View.Property(p => p.ControlMethod).Readonly().Show();
				View.Property(p => p.LotNo).Readonly().ShowInList(width: 120).Show();
                View.Property(p => p.Sn).Readonly(p=>p.ControlMethod != SIE.EMS.SpareParts.Enums.ControlMethod.Sn).ShowInList(width: 120).Show();
                View.Property(p => p.Qty).Readonly(p=>p.ControlMethod == SIE.EMS.SpareParts.Enums.ControlMethod.Sn).Show();
                View.Property(p => p.UnitName).Readonly().Show();
                View.Property(p => p.QualityStatus).HasLabel("质量状态".L10N()+"*").Show();
                View.Property(p => p.WarehouseId).UseDataSource((e, p, o) =>
				{
					return RT.Service.Resolve<SparePartController>().GetZereCostWarehouses(p, o);
				}).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

		///<summary>
		/// 配置备件回收条码打印视图
		/// </summary>
		protected void ConfigPrintAssetDisposalSparePartSnView()
		{
			View.Property(p => p.PrintTemplateId).UseDataSource((e, p, r) =>
			{
				return RT.Service.Resolve<AssetDisposalController>().GetPrintTemplatesByType(typeof(AssetDisposalSparePartPrintable).GetQualifiedName(), p, r);
			}).Show();
		}

		///<summary>
		/// 配置备件回收序列号新增视图
		/// </summary>
		protected void ConfigAddAssetDisposalSparePartSnView()
		{
			View.UseDetail(2);
			View.AssignAuthorize(typeof(AssetDisposalSparePart));
			View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.AddAndPrintAssetDisposalSparePartSnCommand");
			using (View.OrderProperties()) 
			{
				View.Property(p => p.SparePartId).UseDataSource((e, p, o) =>
				{
					return RT.Service.Resolve<SparePartController>().GetSnControlSpareParts(p, o);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
					m.DicLinkField = keyValues;
				}).Show();
				View.Property(p => p.SparePartName).Readonly().Show();
				View.Property(p => p.Qty).UseSpinEditor(m=>m.MinValue = 1).Show();
				View.Property(p => p.QualityStatus).Show();
				View.Property(p => p.WarehouseId).UseDataSource((e, p, o) =>
				{
					return RT.Service.Resolve<SparePartController>().GetZereCostWarehouses(p, o);
				}).Show();
				View.Property(p => p.PrintTemplateId).UseDataSource((e, p, r) =>
				{
					return RT.Service.Resolve<AssetDisposalController>().GetPrintTemplatesByType(typeof(AssetDisposalSparePartPrintable).GetQualifiedName(), p, r);
				}).Show();
			}
		}

		/// <summary>
		/// 配置导入视图
		/// </summary>
		protected override void ConfigImportView()
		{
			View.PropertyRef(p => p.AssetDisposal.No).ImportIndexer().HasLabel("处置单号");
			View.PropertyRef(p => p.SparePart.SparePartCode).ImportIndexer().HasLabel("备件编码");

			View.Property(p => p.LotNo);
			View.Property(p => p.Sn);
			View.Property(p => p.Qty).UseSpinEditor(p =>
			{
				p.MinValue = 1;
				p.AllowNegative = false;
				p.AllowDecimals = false;
				p.AllowBlank = false;
			});
			View.Property(p => p.QualityStatus);
            View.PropertyRef(p => p.Warehouse.Code).HasLabel("入库仓库编码");
		}
	}
}
