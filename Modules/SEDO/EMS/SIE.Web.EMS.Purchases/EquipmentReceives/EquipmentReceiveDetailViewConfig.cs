using SIE.Domain;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.EMS.Purchases._Extensions_;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 接收明细视图配置
    /// </summary>
    public class EquipmentReceiveDetailViewConfig : WebViewConfig<EquipmentReceiveDetail>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.LineNo).HasLabel("接收行号").ShowInList(80);
            View.Property(p => p.PurchaseOrderId).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100);
            View.Property(p => p.PurchaseOrderDescription).ShowInList(250);
            View.Property(p => p.EquipModelId).HasLabel("设备型号编码").ShowInList(130);
            View.Property(p => p.EquipModelName).ShowInList(100);
            View.Property(p => p.Giveaway).ShowInList(50);
            View.Property(p => p.Price).ShowInList(60);
            View.Property(p => p.Qty).ShowInList(80);
            View.Property(p => p.RecivedQty).ShowInList(100);
            View.Property(p => p.WarehouseId).ShowInList(100);
            View.Property(p => p.WorkshopId).ShowInList(100);
            View.Property(p => p.SupplierId).ShowInList(120).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.CustomerId).ShowInList(120).HasLabel("客户编码");
            View.Property(p => p.CustomerName).ShowInList(200);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.SnList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.EquipModelId).HasLabel("设备型号编码").ShowInList(130);
            View.Property(p => p.EquipModelName).ShowInList(100);
            View.Property(p => p.PurchaseOrderId).ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(EquipmentReceive));
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentReceives.ReceiveDetailBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentReceives.Commands.AddReceiveDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(50).Readonly();
                View.Property(p => p.PurchaseOrderId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as EquipmentReceiveDetail;
                    if (entity == null)
                    {
                        return new EntityList<PurchaseOrder>();
                    }
                    var types = new List<PurchaseObjectType> { PurchaseObjectType.Equipment };
                    return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrders(entity.FactoryId, entity.DepartmentId, types, pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("SupplierId_Display", nameof(e.PurchaseOrder.SupplierName));
                    keyValues.Add(nameof(e.SupplierId), nameof(e.PurchaseOrder.SupplierId));
                    keyValues.Add(nameof(e.SupplierName), nameof(e.PurchaseOrder.SupplierName));
                    keyValues.Add(nameof(e.PurchaseOrderDescription), nameof(e.PurchaseOrder.Description));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ReceiveType == ReceiveType.Customer || p.ReceiveType == ReceiveType.Lease || p.ReceiveType == ReceiveType.Other)
                .ShowInList(130).Cascade(p => p.PurchaseOrderItemId, null);

                View.Property(p => p.PurchaseOrderItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as EquipmentReceiveDetail;
                    if (entity == null || entity.PurchaseOrderId == null)
                    {
                        return new EntityList<PurchaseOrderItem>();
                    }
                    return RT.Service.Resolve<PurchaseOrderController>().DetailGetOrderItems(entity.PurchaseOrderId.Value, pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Price), nameof(e.PurchaseOrderItem.Price));
                    m.DicLinkField = keyValues;
                }).HasLabel("采购单行号").ShowInList(100).Readonly(p => p.ReceiveType == ReceiveType.Customer || p.ReceiveType == ReceiveType.Lease || p.ReceiveType == ReceiveType.Other);

                View.Property(p => p.PurchaseOrderDescription).ShowInList(200).Readonly();

                View.Property(p => p.EquipModelId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                    m.DicLinkField = keyValues;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
                }).HasLabel("设备型号编码").ShowInList(130);

                View.Property(p => p.EquipModelName).ShowInList(150).Readonly();
                View.Property(p => p.Giveaway).Readonly(p => p.ReceiveType != ReceiveType.Purchase || (p.OrderEquipModelId != null && p.OrderEquipModelId != p.EquipModelId))
                    .ShowInList(50);
                View.Property(p => p.Price).Readonly(p => p.ReceiveType == ReceiveType.Giveaway).UseSpinEditor(p => p.MinValue = 0).ShowInList(60).Cascade(p => p.WarehouseId, null);
                View.Property(p => p.Qty).HasLabel("接收数量".L10N()+"*").UseSpinEditor(p => p.MinValue = 1).ShowInList(80);
                View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as EquipmentReceiveDetail;
                    if (entity == null)
                    {
                        return new EntityList<Warehouse>();
                    }
                    return RT.Service.Resolve<EquipmentReceiveSnController>().DetailGetWarehouses(entity.Price <= 0, pagingInfo, keyword);
                }).UseListSetting(p => { p.HelpInfo = "单价为空或单价等于0时，仅能选零成本仓。单据不等于0时，仅能选非零成本仓"; }).ShowInList(100);

                View.Property(p => p.WorkshopId).UseDataSource((source, pagingInfo, keyword) =>
                  {
                      var entity = source as EquipmentReceiveDetail;
                      if (entity == null)
                      {
                          return new EntityList<Enterprise>();
                      }
                      return RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, entity.FactoryId);
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
                View.ChildrenProperty(p => p.SnList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}