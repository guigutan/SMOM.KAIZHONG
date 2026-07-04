using SIE.Domain;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.EMS.Purchases._Extensions_;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收明细视图配置
    /// </summary>
    public class FixtureReceiveDetailViewConfig : WebViewConfig<FixtureReceiveDetail>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public static readonly string EditView = "EditView";

        /// <summary>
        /// 扫描视图
        /// </summary>
        public static readonly string ScanView = "ScanView";

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
            View.Property(p => p.LineNo).HasLabel("行号").ShowInList(80);
            View.Property(p => p.PurchaseOrderId).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100);
            View.Property(p => p.SupplierId).ShowInList(120).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.CustomerId).ShowInList(120).HasLabel("客户编码");
            View.Property(p => p.CustomerName).ShowInList(200);
            View.Property(p => p.FixtureEncodeId).ShowInList(100).Readonly();
            View.Property(p => p.ModelCode).ShowInList(80).Readonly();
            View.Property(p => p.ModelName).ShowInList(80).Readonly();
            View.Property(p => p.ManageMode).ShowInList(80).Readonly();
            View.Property(p => p.Unit).ShowInList(50);
            View.Property(p => p.ExemptionInspect).ShowInList(50);
            View.Property(p => p.Price).ShowInList(120);
            View.Property(p => p.TaxRate).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(70);
            View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.Qty).ShowInList(80);
            View.Property(p => p.RecivedQty).ShowInList(100);
            View.Property(p => p.WarehouseId).ShowInList(100);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.FixtureReceiveSnList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.DisableEditing();
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.FixtureEncodeId).ShowInList(100);
            View.Property(p => p.PurchaseOrderId).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100);
        }

        /// <summary>
        /// 默认明细视图配置
        /// </summary>
        private void DefualtDetailConfig()
        {
            View.AssignAuthorize(typeof(FixtureReceive));
            View.AddBehavior("SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDetailBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.FixtureReceives.Commands.AddReceiveDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(50).Readonly();
                View.Property(p => p.PurchaseOrderId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as FixtureReceiveDetail;
                    if (entity == null)
                    {
                        return new EntityList<PurchaseOrder>();
                    }
                    if (entity.ReceiveType != ReceiveType.Giveaway)
                    {
                        var types = new List<PurchaseObjectType> { PurchaseObjectType.Fixture };
                        return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrders(entity.FactoryId, entity.DepartmentId, types, pagingInfo, keyword);
                    }
                    else//正品接收时候
                    {
                        return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrders(entity.FactoryId, entity.DepartmentId, pagingInfo, keyword);
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("SupplierId_Display", nameof(e.PurchaseOrder.SupplierName));
                    keyValues.Add(nameof(e.SupplierId), nameof(e.PurchaseOrder.SupplierId));
                    keyValues.Add(nameof(e.SupplierName), nameof(e.PurchaseOrder.SupplierName));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ReceiveType == ReceiveType.Customer || p.ReceiveType == ReceiveType.Lease || p.ReceiveType == ReceiveType.Other)
                .ShowInList(130).Cascade(p => p.PurchaseOrderItemId, null);
                View.Property(p => p.PurchaseOrderItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as FixtureReceiveDetail;
                    if (entity == null || entity.PurchaseOrderId == null)
                    {
                        return new EntityList<PurchaseOrderItem>();
                    }

                    if (entity.ReceiveType != ReceiveType.Giveaway)
                    {
                        return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrderItems(entity.PurchaseOrderId.Value, pagingInfo, keyword);
                    }
                    else
                    {
                        return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrderItemsNoStatus(entity.PurchaseOrderId.Value, pagingInfo, keyword);
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    if (e.ReceiveType != ReceiveType.Giveaway)
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.Price), nameof(e.PurchaseOrderItem.Price));
                        keyValues.Add(nameof(e.TaxRate), nameof(e.PurchaseOrderItem.TaxRate));
                        keyValues.Add(nameof(e.PriceNoTax), nameof(e.PurchaseOrderItem.PriceNoTax));
                        m.DicLinkField = keyValues;
                    }
                }).HasLabel("采购单行号").ShowInList(100).Readonly(p => p.ReceiveType == ReceiveType.Customer || p.ReceiveType == ReceiveType.Lease || p.ReceiveType == ReceiveType.Other);
                View.Property(p => p.FixtureEncodeId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.ModelCode));
                    keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
                    keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureEncode.ManageMode));
                    keyValues.Add(nameof(e.ExemptionInspect), nameof(e.FixtureEncode.Exemption));
                    keyValues.Add("UnitId_Display", nameof(e.FixtureEncode.UnitName));
                    keyValues.Add(nameof(e.UnitId), nameof(e.FixtureEncode.UnitId));
                    m.DicLinkField = keyValues;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(keyword, pagingInfo);
                }).HasLabel("工治具编码").ShowInList(130).Readonly(p => p.FixtureEncodeIdReadOnly);

                View.Property(p => p.ModelCode).ShowInList(80).Readonly();
                View.Property(p => p.ModelName).ShowInList(80).Readonly();
                View.Property(p => p.ManageMode).ShowInList(80).Readonly();
                View.Property(p => p.ExemptionInspect).Readonly().ShowInList(50);
                View.Property(p => p.Unit).ShowInList(80).Readonly(p => p.PurchaseOrderItemId != null);
                View.Property(p => p.Price).Readonly(p => p.ReceiveType == ReceiveType.Giveaway).UseSpinEditor(p => p.MinValue = 0).ShowInList(120).Cascade(p => p.WarehouseId, null);
                View.Property(p => p.TaxRate).Readonly(p => p.ReceiveType == ReceiveType.Purchase || p.ReceiveType == ReceiveType.Giveaway)
                   .UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(70);
                View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130).Readonly();
                View.Property(p => p.Qty).HasLabel("接收数量".L10N()+"*").UseSpinEditor(p =>
                {
                    p.MinValue = 1;
                    p.AllowDecimals = false;
                }).ShowInList(80);
                View.Property(p => p.RecivedQty).ShowInList(100).Readonly();
                
                OtherCloumns();
                View.ChildrenProperty(p => p.FixtureReceiveSnList).Show(ChildShowInWhere.Hide);
            }

        }
        /// <summary>
        /// 隐藏字段
        /// </summary>
        private void OtherCloumns()
        {
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as FixtureReceiveDetail;
                if (entity == null)
                {
                    return new EntityList<Warehouse>();
                }
                return RT.Service.Resolve<FixtureReceiveController>().DetailGetWarehouses(entity,pagingInfo, keyword);
            }).ShowInList(100);
            View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = keyValues;
            }).ShowInList(120).HasLabel("供应商编码").Readonly(p => p.ReceiveType == ReceiveType.Customer || p.PurchaseOrderId > 0);
            View.Property(p => p.SupplierName).ShowInList(200).Readonly();
            View.Property(p => p.CustomerId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
                m.DicLinkField = keyValues;
            }).ShowInList(120).HasLabel("客户编码").Readonly(p => p.ReceiveType != ReceiveType.Customer);
            View.Property(p => p.CustomerName).ShowInList(200).Readonly();
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }



        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            DefualtDetailConfig();
        }

        /// <summary>
        /// 扫描视图
        /// </summary>
        protected void ConfigScanView()
        {
            View.DisableEditing();
            DefualtDetailConfig();
            View.ClearCommands();
        }
    }
}