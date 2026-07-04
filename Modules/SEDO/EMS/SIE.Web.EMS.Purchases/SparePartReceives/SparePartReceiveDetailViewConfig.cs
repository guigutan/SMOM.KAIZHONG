using SIE.Domain;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.SparePartReceives.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收明细视图配置
    /// </summary>
    public class SparePartReceiveDetailViewConfig : WebViewConfig<SparePartReceiveDetail>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 扫描视图
        /// </summary>
        public const string ScanView = "ScanView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView, ScanView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
            if (ViewGroup == ScanView)
            {
                ConfigScanView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartReceives.Commands.OnekeyReceiveCommand",
                typeof(ImportLotSnCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.PurchaseOrderId).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).ShowInList(100);
            View.Property(p => p.SupplierId).HasLabel("供应商编码").ShowInList(100);
            View.Property(p => p.SupplierName).ShowInList(150);
            View.Property(p => p.PurchaseObjectType).ShowInList(120);
            View.Property(p => p.SparePartId).HasLabel("备件编码").ShowInList(130);
            View.Property(p => p.SparePartName).ShowInList(100);
            View.Property(p => p.ControlMethod).ShowInList(100);
            View.Property(p => p.ExemptionInspect).ShowInList(50).Readonly(p=>p.ReceiveBillStatus== SIE.EMS.Purchases.Enums.ReceiveBillStatus.Completed);
            View.Property(p => p.Qty).ShowInList(80);
            View.Property(p => p.RecivedQty).ShowInList(100);
            View.Property(p => p.UnitName).ShowInList(60);
            View.Property(p => p.Price).ShowInList(130);
            View.Property(p => p.TaxRate).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(70);
            View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.WarehouseId).HasLabel("接收仓库").ShowInList(100);
            View.Property(p => p.OutDepotLineNo).ShowInList(150);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.SnList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.LotList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.SparePartId).HasLabel("备件编码").ShowInList(130);
            View.Property(p => p.SparePartName).ShowInList(100);
            View.Property(p => p.PurchaseOrderId).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).ShowInList(100);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.SparePartReceive.ReceiveNo).HasLabel("接收单号");
            View.Property(p => p.LineNo).HasLabel("接收行号");
            View.Property(p => p.LotNo);
            View.Property(p => p.Sn);
            View.Property(p => p.OriginalSn);
            View.Property(p => p.ImportQty);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(SparePartReceive));
            View.AddBehavior("SIE.Web.EMS.Purchases.SparePartReceives.ReceiveDetailBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartReceives.Commands.AddReceiveDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(50).Readonly();
                View.Property(p => p.PurchaseOrderId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as SparePartReceiveDetail;
                    if (entity == null)
                    {
                        return new EntityList<PurchaseOrder>();
                    }
                    var types = new List<PurchaseObjectType> { PurchaseObjectType.SparePart, PurchaseObjectType.Excipients, PurchaseObjectType.Tool };
                    return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrders(entity.FactoryId, entity.DepartmentId, types, pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.PurchaseObjectType), nameof(e.PurchaseOrder.PurchaseObjectType));
                    keyValues.Add("SupplierId_Display", nameof(e.PurchaseOrder.SupplierName));
                    keyValues.Add(nameof(e.SupplierId), nameof(e.PurchaseOrder.SupplierId));
                    keyValues.Add(nameof(e.SupplierName), nameof(e.PurchaseOrder.SupplierName));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ReceiveType == ReceiveType.Customer || p.ReceiveType == ReceiveType.Lease || p.ReceiveType == ReceiveType.Other)
                .ShowInList(130).Cascade(p => p.PurchaseOrderItemId, null);
                View.Property(p => p.PurchaseOrderItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as SparePartReceiveDetail;
                    if (entity == null || entity.PurchaseOrderId == null)
                    {
                        return new EntityList<PurchaseOrderItem>();
                    }
                    return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrderItems(entity.PurchaseOrderId.Value, pagingInfo, keyword, entity.ReceiveType);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Price), nameof(e.PurchaseOrderItem.Price));
                    keyValues.Add(nameof(e.TaxRate), nameof(e.PurchaseOrderItem.TaxRate));
                    keyValues.Add(nameof(e.PriceNoTax), nameof(e.PurchaseOrderItem.PriceNoTax));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ReceiveType == ReceiveType.Customer || p.ReceiveType == ReceiveType.Lease || p.ReceiveType == ReceiveType.Other).ShowInList(100);
                View.Property(p => p.PurchaseObjectType).Readonly().ShowInList(120);
                View.Property(p => p.SparePartId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as SparePartReceiveDetail;
                    if (entity == null)
                    {
                        return new EntityList<SparePart>();
                    }
                    return RT.Service.Resolve<SparePartController>().GetSparePartsToState(pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                    keyValues.Add(nameof(e.ExemptionInspect), nameof(e.SparePart.ExemptionInspect));
                    keyValues.Add(nameof(e.UnitName), nameof(e.SparePart.UnitName));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ReceiveType == ReceiveType.Purchase).HasLabel("备件编码").ShowInList(130);
                View.Property(p => p.SparePartName).Readonly().ShowInList(100);
                View.Property(p => p.ControlMethod).Readonly().ShowInList(100);
                View.Property(p => p.Price).Readonly(p => p.ReceiveType == ReceiveType.Purchase || p.ReceiveType == ReceiveType.Giveaway)
                    .ShowInList(130).Cascade(p => p.WarehouseId, null);
                View.Property(p => p.TaxRate).Readonly(p => p.ReceiveType == ReceiveType.Purchase || p.ReceiveType == ReceiveType.Giveaway)
                    .UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(70);
                View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130).Readonly();
                View.Property(p => p.ExemptionInspect).Readonly().ShowInList(50);
                View.Property(p => p.Qty).HasLabel("接收数量".L10N() + "*").UseSpinEditor(p => p.MinValue = 0).ShowInList(80);
                View.Property(p => p.UnitName).Readonly().ShowInList(60);

                //编辑视图配置
                ConfigEditDetailView();
            }
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditDetailView()
        {
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SparePartReceiveDetail;
                if (entity == null)
                {
                    return new EntityList<Warehouse>();
                }
                return RT.Service.Resolve<EquipmentReceiveSnController>().DetailGetWarehouses(entity.Price <= 0, pagingInfo, keyword);
            }).HasLabel("接收仓库").UseListSetting(p => { p.HelpInfo = "单价为空或单价等于0时，仅能选零成本仓。单据不等于0时，仅能选非零成本仓"; }).ShowInList(100);
            View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = keyValues;
            }).Readonly(p => p.PurchaseOrderId > 0).HasLabel("供应商编码").ShowInList(100);
            View.Property(p => p.SupplierName).Readonly().ShowInList(150);
            View.Property(p => p.PartOutDepotDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SparePartReceiveDetail;
                if (entity == null)
                {
                    return new EntityList<PartOutDepotDetail>();
                }
                return RT.Service.Resolve<SparePartReceiveController>().GetPartOutDepotDetails(entity.SparePartId, pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                m.DisplayField = "OutDepotLineNo";
            }).Readonly(p => p.ReceiveType != ReceiveType.Outsourced).ShowInList(width: 150);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.SnList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.LotList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 扫描视图
        /// </summary>
        protected void ConfigScanView()
        {
            View.DisableEditing();
            View.AssignAuthorize(typeof(SparePartReceive));
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(50);
                View.Property(p => p.PurchaseOrderNo).ShowInList(130);
                View.Property(p => p.PurchaseOrderLine).ShowInList(100);
                View.Property(p => p.SupplierCode).ShowInList(100);
                View.Property(p => p.SupplierName).ShowInList(150);
                View.Property(p => p.PurchaseObjectType).ShowInList(120);
                View.Property(p => p.SparePartCode).ShowInList(130);
                View.Property(p => p.SparePartName).ShowInList(100);
                View.Property(p => p.ControlMethod).ShowInList(100);
                View.Property(p => p.Qty).ShowInList(80);
                View.Property(p => p.RecivedQty).ShowInList(100);
                View.Property(p => p.UnitName).ShowInList(60);
                View.Property(p => p.WarehouseName).HasLabel("接收仓库").ShowInList(100);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.SnList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.LotList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}