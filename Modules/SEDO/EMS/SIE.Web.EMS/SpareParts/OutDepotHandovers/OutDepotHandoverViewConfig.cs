using SIE.Domain;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.MetaModel.View;
using SIE.Web.Common.Configs.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepotHandovers
{
	/// <summary>
	/// 备件出库交接视图配置
	/// </summary>
	public class OutDepotHandoverViewConfig : WebViewConfig<OutDepotHandover>
	{
		/// <summary>
		/// 扫码交接视图
		/// </summary>
		public const string ScanHandoverDetailsViewGroup = "ScanHandoverDetailsViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { ScanHandoverDetailsViewGroup });

			if (ViewGroup == ScanHandoverDetailsViewGroup)
			{
				ConfigScanHandoverDetailsView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommand("SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands.ScanHandoverDetailCommand");
			View.UseCommand("SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands.WholeBillHandoverCommand");
			View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
			View.DisableEditing();
			View.Property(p => p.HandoverNo).ShowInList(width:120);
			View.Property(p => p.OutDepotNo).ShowInList(width: 120);
			View.Property(p => p.GetDepartmentName);
			View.Property(p => p.OutDepotDate).UseDateEditor().ShowInList(width: 120);
			View.Property(p => p.HandOverStatus);
			View.ChildrenProperty(c => c.OutDepotHandoverDetailList).Show(ChildShowInWhere.Hide);
			View.AttachChildrenProperty(typeof(OutDepotHandoverDetail), (e) =>
			{
				EntityList<OutDepotHandoverDetail> handoverDetailList = new EntityList<OutDepotHandoverDetail>();
				var args = e as ChildPagingDataWithParentEntityArgs;
				OutDepotHandover entity = null;

				if (args.ParentEntity != null)
				{
					entity = args.ParentEntity.ToJsonObject<OutDepotHandover>();
				}
				else
				{
					entity = RF.GetById<OutDepotHandover>(args.Parent.GetId());
				}

				if (entity != null)
				{
					handoverDetailList.AddRange(RT.Service.Resolve<OutDepotController>().GetOutDepotHandoverDetailList(args.SortInfo, args.PagingInfo, entity));
				}
				return handoverDetailList;
			}).HasLabel("接收明细");
		}

		/// <summary>
		/// 配置扫码交接视图
		/// </summary>
		protected void ConfigScanHandoverDetailsView()
		{
			double? nullValue = null;
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
			View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepotHandovers.Behaviors.ScanHandoverDetailBehavior");
			View.UseCommand("SIE.Web.EMS.SpareParts.OutDepotHandovers.Commands.ResetScanHandoverDetailCommand");
			View.HasDetailColumnsCount(3);
			using (View.OrderProperties())
			{

				View.Property(p => p.Message).ShowInDetail(columnSpan: 3).Readonly().Show().HasLabel("消息框");
				View.Property(p => p.ScanValue)
					.UseDisplayEditor(p => { p.XType = "HandoverDetailScanValueEditor"; })
					.ShowInDetail(columnSpan: 3).Show().HasLabel("扫描框");

				View.Property(p => p.OutDepotHandoverBillId).UseDataSource((e, p, o) =>
				{
					OutDepotHandover handover = e as OutDepotHandover;
					return RT.Service.Resolve<OutDepotHandoverController>().GetOutDepotHandoverBySparePart(p, handover.SparePartId, o);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add("OutDepotHandoverBillId", nameof(e.Id));
					keyValues.Add("OutDepotHandoverBillId_Display", nameof(e.HandoverNo));
					keyValues.Add(nameof(e.OutDepotNo), nameof(e.OutDepotNo));
					keyValues.Add(nameof(e.SparePartId), nameof(e.SparePartId));
					keyValues.Add("SparePartId_Display", nameof(e.SparePartCode));
					keyValues.Add(nameof(e.SparePartName), nameof(e.SparePartName));
					keyValues.Add(nameof(e.ControlMethod), nameof(e.ControlMethod));
					keyValues.Add(nameof(e.Barcode), nameof(e.Barcode));
					keyValues.Add(nameof(e.Qty), nameof(e.Qty));
					keyValues.Add(nameof(e.ReceiveQty), nameof(e.ReceiveQty));
					m.DicLinkField = keyValues;
				}).Show();

				View.Property(p => p.HandoverNo).Show(ShowInWhere.Hide);
				View.Property(p => p.OutDepotNo).Readonly().Show();

				View.Property(p => p.SparePartId)
					.UseDataSource((e, p, o) =>
					{
						OutDepotHandover handover = e as OutDepotHandover;
						return RT.Service.Resolve<OutDepotHandoverController>().GetSparePartByOutDepotHandover(p, handover, o);
					}).UsePagingLookUpEditor((m, e) =>
					{
						Dictionary<string, string> keyValues = new Dictionary<string, string>();
						keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
						keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
						keyValues.Add(nameof(e.IsSelectSparePart), nameof(e.SparePart.IsReplacement));
						m.DicLinkField = keyValues;
					}).Readonly(p=>p.OutDepotHandoverBillId == nullValue).Show();
				
				View.Property(p => p.Barcode).Readonly().HasLabel("序列号/批次号").Show();
				View.Property(p => p.ControlMethod).Readonly().Show();
				View.Property(p => p.SparePartName).Readonly().Show();
				View.Property(p => p.Qty).Readonly().Show();
				View.Property(p => p.ReceiveQty).Readonly().Show();

				View.AttachChildrenProperty(typeof(OutDepotHandoverDetail), (e) =>
				{
					EntityList<OutDepotHandoverDetail> outDepotDetailList = new EntityList<OutDepotHandoverDetail>();
					var args = e as ChildPagingDataWithParentEntityArgs;
					OutDepotHandover entity = null;

					if (args.ParentEntity != null)
					{
						entity = args.ParentEntity.ToJsonObject<OutDepotHandover>();
					}
					else
					{
						entity = RF.GetById<OutDepotHandover>(args.Parent.GetId());
					}

					if (entity != null)
					{
						outDepotDetailList.AddRange(RT.Service.Resolve<OutDepotHandoverController>().GetOutDepotHandoverDetails(entity.OutDepotHandoverBillId, entity.SparePartId, args.SortInfo, args.PagingInfo));
					}
					return outDepotDetailList;
				}, "ScanOutDepotHandoverDetailViewGroup").HasLabel("接收明细").Show(ChildShowInWhere.All);
			}
		}

		/// <summary>
		/// 选择视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
			View.Property(p => p.HandoverNo).ShowInList(width: 120);
			View.Property(p => p.OutDepotNo).ShowInList(width: 120);
			View.Property(p => p.GetDepartmentName);
			View.Property(p => p.OutDepotDate).UseDateEditor().ShowInList(width: 120);
			View.Property(p => p.HandOverStatus);
		}
	}
}