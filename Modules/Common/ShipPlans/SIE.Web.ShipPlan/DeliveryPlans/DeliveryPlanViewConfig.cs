using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.ShipPlan;
using SIE.ShipPlan.ViewModels;
using SIE.Warehouses;
using SIE.Web.Inventory;
using SIE.Web.Items._Extentions_;
using SIE.Web.ShipPlan.Commands;
using SIE.Web.Warehouses;
using System;
using System.Collections.Generic;

namespace SIE.Web.ShipPlan
{
    /// <summary>
    /// 发货计划视图配置
    /// </summary>
    internal class DeliveryPlanViewConfig : WebViewConfig<DeliveryPlan>
    {
        /// <summary>
        ///  扩展查看视图
        /// </summary>
        public const string StockPlanKittingViewGroup = "StockPlanKittingViewGroup";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { StockPlanKittingViewGroup });
            View.AssignAuthorize(typeof(DeliveryPlan));
            if (ViewGroup == StockPlanKittingViewGroup)
            {
                StockPlanKittingView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.InlineEdit();
            View.UseGridSelectionModel();
            View.UseCommands(typeof(AddDeliveryPlanCommand).FullName, WebCommandNames.Edit, typeof(DeleteDeliveryPlanCommand).FullName, typeof(SaveDeliveryPlanCommand).FullName, typeof(AuditDeliveryPlanCommand).FullName, typeof(CreateSoCommand).FullName, typeof(AssignWarehouseCommand).FullName,
               typeof(ForceCompleteCommand).FullName,
               typeof(KittingCommand).FullName,
               "SIE.Web.ShipPlan.Commands.MergeCreateCommand"
               );
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.CreateQty).UseItemUnitEditor(p => p.MinValue = 0).Readonly(p => p.State == DeliveryState.Created || p.State == DeliveryState.Finished || p.State == DeliveryState.Cancel);
                View.Property(p => p.State).Readonly();
                View.Property(p => p.No).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.LineNo).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.OrderType).DefaultValue((int)OrderType.SaleOut).UseSelectEnumEditor(p =>
                {
                    p.ColumnXType = "planOrderTypeEditor";
                    p.AllowBlank = false;
                    p.ValuesList.Add((int)OrderType.SaleOut);
                    p.ValuesList.Add((int)OrderType.OutWorkFeed);
                    p.ValuesList.Add((int)OrderType.OutWorkFeedUse);
                    p.ValuesList.Add((int)OrderType.OutAllotReturn);
                    p.ValuesList.Add((int)OrderType.OtherOut);
                    p.ValuesList.Add((int)OrderType.SupplierReturn);
                    p.ValuesList.Add((int)OrderType.DirectAllocate);
                    p.ValuesList.Add((int)OrderType.TwoAllocate);
                    p.ValuesList.Add((int)OrderType.WhTransferOut);
                }).Cascade(p => p.Enterprise, null).Cascade(p => p.Customer, null).Cascade(p => p.Supplier, null).Cascade(p => p.AllotModel, null).Cascade(p => p.TargetWarehouse, null).Readonly(p => p.State != DeliveryState.Created);

                View.Property(p => p.ResourceId).Readonly();
                //调拨模式
                View.Property(p => p.AllotModel).Readonly(p => p.State != DeliveryState.Created || p.OrderType == OrderType.DirectAllocate || p.OrderType == OrderType.TwoAllocate).Cascade(p => p.TargetWarehouseId, null).HasLabel("发运调拨模式");
                View.Property(p => p.ItemId).UseDataSource((e, c, r) =>
                {
                    var plan = e as DeliveryPlan;
                    if (plan == null)
                    {
                        return new EntityList<Item>();
                    }
                    return RT.Service.Resolve<ItemController>().GetAllEnableItems(c, r);
                }).UsePagingLookUpEditor((p, t) =>
                {
                    p.Editable = false;
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(t.ItemCode), nameof(t.Item.Code));
                    keyValues.Add(nameof(t.ItemName), nameof(t.Item.Name));
                    keyValues.Add(nameof(t.ItemEnableExtendProp), nameof(t.Item.EnableExtendProperty));
                    keyValues.Add(nameof(t.ItemSpecificationModel), nameof(t.Item.SpecificationModel));
                    keyValues.Add(nameof(t.ItemUnitName), nameof(t.Item.UnitName));
                    p.DicLinkField = keyValues;
                }).Cascade(p => p.ItemExtProp, null).Cascade(p => p.ItemExtPropName, null)
                    .Cascade(p => p.LotCode, null).Cascade(p => p.ProductBatch, null).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.ItemName).Readonly();
                View.Property(p => p.ItemSpecificationModel).Readonly();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.DbField = "ItemExtProp";
                }).Readonly(p => p.State != DeliveryState.Created || !p.ItemEnableExtendProp);
                View.Property(p => p.ItemUnitName).Readonly();
                View.Property(p => p.RequireQty).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.NoCreateQty).Readonly();
                View.Property(p => p.DeliveryQty).Readonly();
                View.Property(p => p.CancelQty).Readonly();
                View.Property(p => p.DeliveryDate).UseDateEditor().Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.EnterpriseId).Readonly(p => p.State != DeliveryState.Created || (p.OrderType != OrderType.WorkFeed && p.OrderType != OrderType.OutWorkFeed
                && p.OrderType != OrderType.OutWorkFeedUse && p.OrderType != OrderType.OutAllotReturn && p.OrderType != OrderType.OtherOut));
                View.Property(p => p.ResourceId).Readonly();
                View.Property(p => p.CustomerId).UseDataSource((o, e, r) =>
                {
                    var plan = o as DeliveryPlan;
                    if (plan == null)
                    {
                        return new EntityList<Customer>();
                    }

                    return RT.Service.Resolve<CustomerController>().GetCustomer(CustomerType.CUSTOMER, r, e);
                }).Readonly(p => p.State != DeliveryState.Created || p.OrderType != OrderType.SaleOut);
                View.Property(p => p.SupplierId).UseDataSource((o, e, r) =>
                {
                    var plan = o as DeliveryPlan;
                    if (plan == null)
                    {
                        return new EntityList<Supplier>();
                    }

                    return RT.Service.Resolve<SupplierController>().GetSuppliers(e, r);
                }).Readonly(p => p.State != DeliveryState.Created || (p.OrderType != OrderType.SupplierReturn && p.OrderType != OrderType.OutWorkFeed && p.OrderType != OrderType.OutAllotReturn && p.OrderType != OrderType.OutWorkFeedUse));
                View.Property(p => p.TargetWarehouseId).UseWarehouseEditorWithOutLine(p =>
                {
                    p.DataSourceProperty = "true";
                    p.XType = "warehouseInvorgEditor";
                    p.ReloadDataOnPopping = true;
                    p.DisplayField = "Name";
                    var keyValues = new Dictionary<string, string>();

                    p.DicLinkField = keyValues;
                }).Readonly(p => p.State != DeliveryState.Created || (p.OrderType != OrderType.DirectAllocate && p.OrderType != OrderType.TwoAllocate && p.AllotModel == null));
                View.Property(p => p.WarehouseId).UseDataSource((o, e, r) =>
                {
                    var plan = o as DeliveryPlan;
                    if (plan == null)
                    {
                        return new EntityList<Warehouse>();
                    }
                    return RT.Service.Resolve<WarehouseController>().GetEnableWarehousesWithOutLine(e, r, LibraryType.Entity);
                }).Readonly(p => p.State == DeliveryState.Cancel || p.State == DeliveryState.Finished);
                View.Property(p => p.OrderNo).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.OrderLineNo).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.StorerCode).UseDeliveryPlanStorerCodeEditor(p =>
                {
                    p.DisplayField = Customer.CodeProperty.Name;
                    p.ValueField = Customer.CodeProperty.Name;
                    p.ReloadDataOnPopping = true;
                }).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.ProjectNo).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.TaskNo).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.LotCode).UseDeliveryPlanLotEditor(p =>
                {
                    p.DisplayField = Lot.CodeProperty.Name;
                    p.ValueField = Lot.CodeProperty.Name;
                    p.ReloadDataOnPopping = true;
                }).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.ProductBatch).UseDeliveryPlanLotEditor(p =>
                {
                    p.DisplayField = Lot.LotAtt04Property.Name;
                    p.ValueField = Lot.LotAtt04Property.Name;
                    p.ReloadDataOnPopping = true;
                }).Readonly(p => p.State != DeliveryState.Created);
                View.Property(p => p.SourceType).Readonly();

            }
        }

        /// <summary>
        /// 配置查看视图
        /// </summary>
        protected void StockPlanKittingView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.CreateQty).Readonly().Show();
                View.Property(p => p.KittingType).Readonly().Show();
                View.Property(p => p.ItemCode).Readonly().Show();
                View.Property(p => p.ItemName).Readonly().Show();
                View.Property(p => p.ItemExtPropName).Readonly().Show();
                View.Property(p => p.WarehouseName).Readonly().Show();
                View.Property(p => p.DeliveryDate).Readonly().Show();
                View.Property(p => p.StorerCode).Readonly().Show();
                View.Property(p => p.ProjectNo).Readonly().Show();
                View.Property(p => p.TaskNo).Readonly().Show();
                View.Property(p => p.ProductBatch).Readonly().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(StockPlanAssignViewModel), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var asn = args.Parent.CastTo<DeliveryPlan>();
                    if (asn == null)
                    {
                        return new EntityList<StockPlanAssignViewModel>();
                    }
                    return new EntityList<StockPlanAssignViewModel>();
                }).Show(ChildShowInWhere.All).HasLabel("预分配信息").OrderNo = 0;
            }
        }
    }
}