using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.MetaModel.View;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.SpareParts
{
	/// <summary>
	/// 备件库存查询视图配置
	/// </summary>
	internal class StoreSummaryViewConfig : WebViewConfig<StoreSummary>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			var isComputeAvgCost = RT.Service.Resolve<SparePartController>().IsComputeAvgCost();
			View.UseCommand(WebCommandNames.ExportXls);
			View.UseCommand(WebCommandNames.ExportXlsAll);
			View.UseCommand(WebCommandNames.ExportXlsSelection);
			View.DisableEditing();
			View.AddBehavior("SIE.Web.EMS.SpareParts.Behaviors.StoreSummaryBehavior");
			using (View.OrderProperties())
			{
				View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, r) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(r.SparePartCode), nameof(r.SparePart.SparePartCode));
					keyValues.Add(nameof(r.SparePartName), nameof(r.SparePart.SparePartName));
					keyValues.Add(nameof(r.Specification), nameof(r.SparePart.Specification));
					keyValues.Add(nameof(r.ItemCategory), nameof(r.SparePart.ItemCategoryName));
					keyValues.Add(nameof(r.SpartType), nameof(r.SparePart.SpartType));
					keyValues.Add(nameof(r.Unit), nameof(r.SparePart.UnitName));
					keyValues.Add(nameof(r.ControlMethod), nameof(r.SparePart.ControlMethod));
					keyValues.Add(nameof(r.SafeStock), nameof(r.SparePart.SafeStock));
					m.DicLinkField = keyValues;
				}).ShowInList(width:140);

				View.Property(p => p.SparePartName);
				View.Property(p => p.Specification);
				View.Property(p => p.ItemCategory);
				View.Property(p => p.SpartType);
				View.Property(p => p.Unit);
				View.Property(p => p.ControlMethod);
				View.Property(p => p.SafeStock);
				View.Property(p => p.RotNumber);
				View.Property(p => p.GoodNumber);
				View.Property(p => p.SumNumber);

				if (isComputeAvgCost) 
				{
					View.Property(p => p.AverageCost).UseSpinEditor(m => m.DecimalPrecision = 2);
				}
				
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
				View.ChildrenProperty(p => p.StoreSummaryWarehouseList).Show(ChildShowInWhere.Hide);
				View.ChildrenProperty(p => p.StoreSummaryStockList).Show(ChildShowInWhere.Hide);
				View.ChildrenProperty(p => p.StoreSummaryLocationList).Show(ChildShowInWhere.Hide);
				View.ChildrenProperty(p => p.StoreSummaryDepotList).Show(ChildShowInWhere.Hide);
				View.AttachChildrenProperty(typeof(StoreSummaryWarehouse), (e) =>
                {
					EntityList<StoreSummaryWarehouse> storeWhList = new EntityList<StoreSummaryWarehouse>();
					var args = e as ChildPagingDataWithParentEntityArgs;
					StoreSummary entity = null;
					if (args.ParentEntity != null)
						entity = args.ParentEntity.ToJsonObject<StoreSummary>();
					else
						entity = RF.GetById<StoreSummary>(args.Parent.GetId());
					if (entity != null)
						storeWhList.AddRange(RT.Service.Resolve<SparePartController>().GetStoreSummaryWarehouseList(args.SortInfo, args.PagingInfo, entity));
					return storeWhList;
                }, ViewConfig.ListView).Show(ChildShowInWhere.All);
				View.AttachChildrenProperty(typeof(StoreSummaryStock), (e) =>
				{
					EntityList<StoreSummaryStock> storeWhList = new EntityList<StoreSummaryStock>();
					var args = e as ChildPagingDataWithParentEntityArgs;
					StoreSummary entity = null;
					if (args.ParentEntity != null)
						entity = args.ParentEntity.ToJsonObject<StoreSummary>();
					else
						entity = RF.GetById<StoreSummary>(args.Parent.GetId());
					if (entity != null)
						storeWhList.AddRange(RT.Service.Resolve<SparePartController>().GetStoreSummaryStockList(args.SortInfo, args.PagingInfo, entity));
					return storeWhList;
				}, ViewConfig.ListView).Show(ChildShowInWhere.All);
				View.AttachChildrenProperty(typeof(StoreSummaryLot), (e) =>
				{
					EntityList<StoreSummaryLot> storeLotList = new EntityList<StoreSummaryLot>();
					var args = e as ChildPagingDataWithParentEntityArgs;
					StoreSummary entity = null;
					if (args.ParentEntity != null)
						entity = args.ParentEntity.ToJsonObject<StoreSummary>();
					else
						entity = RF.GetById<StoreSummary>(args.Parent.GetId());
					if (entity != null)
						return RT.Service.Resolve<SparePartController>().GetStoreSummaryLotList(args.SortInfo, args.PagingInfo, entity);
					return storeLotList;
				}, ViewConfig.ListView,true).Show(ChildShowInWhere.All);
				View.ChildrenProperty(p => p.StoreSummaryDetailList);
			}

        }
	}
}