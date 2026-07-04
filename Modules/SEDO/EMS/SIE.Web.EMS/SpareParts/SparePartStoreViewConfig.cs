using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.MetaModel.View;
using SIE.Web.Common.Configs.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 备件入库视图配置
    /// </summary>
    public class SparePartStoreViewConfig : WebViewConfig<SparePartStore>
    {
        /// <summary>
        /// 入库视图
        /// </summary>
        public const string StoreDetailsViewGroup = "StoreDetailsViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { StoreDetailsViewGroup, });

            if (ViewGroup == StoreDetailsViewGroup)
            {
                ConfigStoreDetailsView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand(WebCommandNames.Add);
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.SparePartStoreCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.WholeBillStoreCommand");
            View.UseCommands(WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.FormEdit();
            View.UseClientOrder();
            View.Property(p => p.StoreCode);
            View.Property(p => p.InboundType);
            View.Property(p => p.ReceiveNo).ShowInList(width: 120).Show(ShowInWhere.Hide);
            View.Property(p => p.AcceptanceNo).ShowInList(width: 120).Show(ShowInWhere.Hide);
            View.Property(p => p.DisposalNo).ShowInList(width: 120).Show(ShowInWhere.Hide);
            View.Property(p => p.LinkCode).ShowInList(width: 135);
            View.Property(p => p.InboundStatus);
            View.Property(p => p.StoreDateTime).UseDateEditor().ShowInList(width: 100);
            View.Property(p => p.WarehouseOperatorId);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.SupplierName);
            View.ChildrenProperty(p => p.StoreDetailList).Show(ChildShowInWhere.Hide);

            View.AssociateChildrenProperty(SparePartStore.StoreDetailListProperty,
                e =>
                {
                    var arg = e as ChildPagingDataWithParentEntityArgs;
                    var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<SparePartStore>();

                    if (parent == null)
                    {
                        return new EntityList<StoreDetail>();
                    }
                    else
                    {
                        return RT.Service.Resolve<SparePartController>().GetSparePartStoreDetailList(arg.SortInfo, arg.PagingInfo, parent);
                    }
                }).HasLabel("入库明细");
        }

        /// <summary>
        /// 添加视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            double? nullValue = null;
            bool trueValue = true;

            View.AddBehavior("SIE.Web.EMS.SpareParts.Behaviors.ScanStoreDetailBehavior");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.SubmitSparePartStoreCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.ResetSparePartStoreDetailsCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.SparePartStoreConfirmCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.StoreCode).Readonly().Show();
                View.Property(p => p.InboundType).Readonly().DefaultValue(90).Show();
                View.Property(p => p.StorePartType)
                    .Cascade(p => p.WarehouseId, null)
                    .Readonly(p => p.IsExistDetail == trueValue).HasLabel("拆机件/原件*").Show();
                View.Property(p => p.WarehouseId).UseDataSource((e, p, o) =>
                {
                    SparePartStore entity = e as SparePartStore;
                    return RT.Service.Resolve<SparePartController>().GetWarehouseBySparePartStore(p, entity, o);
                }).Cascade(p => p.SparePartId, null)
                  .Cascade(p => p.SparePartName, null)
                  .Cascade(p => p.ControlMethod, null)
                  .Cascade(p => p.IsReplacement, null).Readonly(p => p.StorePartType == null || p.IsExistDetail == trueValue).Show();

                View.Property(p => p.SparePartId).UseDataSource((e, p, o) =>
                         {
                             SparePartStore entity = e as SparePartStore;
                             return RT.Service.Resolve<SparePartController>().GetSparePartBySparePartStore(p, entity, o);
                         }).UsePagingLookUpEditor((m, e) =>
                         {
                             Dictionary<string, string> keyValues = new Dictionary<string, string>();
                             keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                             keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                             keyValues.Add(nameof(e.IsReplacement), nameof(e.SparePart.IsReplacement));
                             keyValues.Add(nameof(e.IsSelectSparePart), nameof(e.SparePart.IsImportItem));
                             keyValues.Add(nameof(e.Number), nameof(e.SparePart.LifeTime));
                             m.DicLinkField = keyValues;
                         })
                         .Readonly(p => p.WarehouseId == nullValue)
                         .Cascade(p => p.QualityStatus, null)
                         .Cascade(p => p.Number, null)
                         .Cascade(p => p.UnitPrice, null)
                         .Cascade(p => p.StorageLocationId, null)
                         .Cascade(p => p.PartOutDepotDetailId, null)
                         .Cascade(p => p.IsCreateNewLabel, null).HasLabel("备件编码".L10N() + "*").Show();

                View.Property(p => p.SparePartName).Readonly().Show();
                View.Property(p => p.ControlMethod).Readonly().Show();
                View.Property(p => p.IsReplacement).Readonly().Show();

                View.Property(p => p.QualityStatus).Readonly(p => p.WarehouseId == nullValue || p.SparePartId == nullValue).HasLabel("质量状态".L10N() + "*").Show();

                View.Property(p => p.Number).UseSpinEditor(m => m.MinValue = 1).Readonly(p => p.WarehouseId == nullValue || p.SparePartId == nullValue || p.ControlMethod == ControlMethod.Sn).HasLabel("数量".L10N() + "*").Show();
                View.Property(p => p.UnitPrice).UseSpinEditor(m =>
                {
                    m.MinValue = 0;
                }).Readonly(p => p.WarehouseId == nullValue || p.SparePartId == nullValue).Show();

                View.Property(p => p.StorageLocationId).UseDataSource((e, p, o) =>
                {
                    SparePartStore entity = e as SparePartStore;
                    return RT.Service.Resolve<StoreSummaryController>().GetStorageLocationForSparePartStore(entity.WarehouseId, entity.SparePartId, entity.ControlMethod);
                }).Readonly(p => p.WarehouseId == nullValue || p.SparePartId == nullValue).HasLabel("库位".L10N() + "*").Show();

                View.Property(p => p.PartOutDepotDetailId).UseDataSource((e, p, o) =>
                {
                    SparePartStore entity = e as SparePartStore;
                    return RT.Service.Resolve<OutDepotController>().GetPartOutDepotDetailListForStore(p, entity, o);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.CanReturnQty), nameof(e.PartOutDepotDetail.CanReturnQty));
                    m.DicLinkField = keyValues;
                    m.DisplayField = "OutDepotLineNo";
                }).Readonly(p => p.WarehouseId == nullValue || p.SparePartId == nullValue).Show();
                View.Property(p => p.CanReturnQty).Readonly().Show();

                View.Property(p => p.IsCreateNewLabel).Readonly(p => p.WarehouseId == nullValue
                || p.SparePartId == nullValue || p.ControlMethod == ControlMethod.Batch
                || p.ControlMethod == ControlMethod.ItemCode
                || (p.ControlMethod == ControlMethod.Sn && p.StorePartType == StorePartType.NewPart))
                    .ShowInDetail("33.3%", columnSpan: 2).Readonly().Show();

                View.Property(p => p.Message)
                    .DefaultValue("请先维护【拆机件/原件】/【仓库】！".L10N())
                    .ShowInDetail(columnSpan: 4).Readonly().Show().HasLabel("消息框");
                View.Property(p => p.ScanValue)
                    .UseDisplayEditor(p => { p.XType = "SparePartStoreScanValueEditor"; })
                    .ShowInDetail(columnSpan: 4).Show().HasLabel("扫描框");

                View.ChildrenProperty(p => p.StoreDetailList).HasLabel("入库明细").Show(ChildShowInWhere.All).ViewGroup = "AddStoreDetailViewGroup";
            }
        }

        /// <summary>
        /// 入库视图配置
        /// </summary>
        protected void ConfigStoreDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.SpareParts.Behaviors.SparePartStoreBehavior");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.SaveSparePartStoreDetailsCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.SubmitSparePartStoreCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.StoreCode).Readonly().Show();
                View.Property(p => p.InboundType).Readonly().Show();
                View.Property(p => p.WarehouseId).Readonly().Show();
                View.Property(p => p.StorageLocationId).UseDataSource((e, p, o) =>
                {
                    SparePartStore entity = e as SparePartStore;
                    return RT.Service.Resolve<StoreSummaryController>().GetStorageLocationForStoreDetails(entity.WarehouseId, o, p);
                }).Show();
                View.Property(p => p.QualityStatus).Readonly(p => p.QualityStatus != null).Show();
                View.Property(p => p.SparePartId).UseDataSource((e, p, o) =>
                {
                    SparePartStore entity = e as SparePartStore;
                    return RT.Service.Resolve<SparePartController>().GetSparePartByStoreDetails(p, entity, o);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                    keyValues.Add(nameof(e.IsReplacement), nameof(e.SparePart.IsReplacement));
                    keyValues.Add(nameof(e.Number), nameof(e.SparePart.LifeTime));
                    keyValues.Add(nameof(e.UnitPrice), nameof(e.SparePart.UnitPrice));
                    keyValues.Add(nameof(e.IsSelectSparePart), nameof(e.SparePart.IsImportItem));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.StorageLocationId == null || p.QualityStatus == null).Show();

                View.Property(p => p.SparePartName).Readonly().Show();
                View.Property(p => p.ControlMethod).Readonly().Show();
                View.Property(p => p.Number).Readonly().Show();
                View.Property(p => p.UnitPrice).Readonly().Show();
                View.Property(p => p.IsReplacement).Readonly().ShowInDetail("50%", columnSpan: 2).Show();

                View.Property(p => p.Message)
                    .DefaultValue("请先维护【库位】！".L10N())
                    .ShowInDetail(columnSpan: 4).Readonly().Show().HasLabel("消息框");
                View.Property(p => p.ScanValue)
                    .UseDisplayEditor(p => { p.XType = "StoreDetailsScanValueEditor"; })
                    .ShowInDetail(columnSpan: 4).Show().HasLabel("扫描框");

                View.AssociateChildrenProperty(SparePartStore.StoreDetailListProperty,
                e =>
                {
                    var arg = e as ChildPagingDataWithParentEntityArgs;
                    var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<SparePartStore>();

                    if (parent == null)
                    {
                        return new EntityList<StoreDetail>();
                    }
                    else
                    {
                        return RT.Service.Resolve<SparePartController>().GetSparePartStoreDetailList(arg.SortInfo, arg.PagingInfo, parent.Id);
                    }
                }, "StoreDetailViewGroup").HasLabel("入库明细").Show(ChildShowInWhere.All);
            }
        }
    }
}