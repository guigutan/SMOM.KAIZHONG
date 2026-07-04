using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Fixtures.Accounts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
	/// 工治具台账（编码管理）视图配置
	/// </summary>
	internal class FixtureCodeAccountViewConfig : WebViewConfig<FixtureCodeAccount>
    {
        private const bool isReadOnly = true;

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));
            View.AssignAuthorize(typeof(FixtureCodeAccount));
            View.ClearCommands().UseCommands("SIE.Web.Fixtures.Accounts.Commands.AddCodeAccCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.EncodeCode).Readonly(isReadOnly);
                View.Property(p => p.TotalQty).Readonly(isReadOnly);
                View.Property(p => p.PassQty).Readonly(isReadOnly);
                View.Property(p => p.NgQty).Readonly(isReadOnly);
                View.Property(p => p.InStockQty).Readonly(isReadOnly);
                View.Property(p => p.OnlineQty).Readonly(isReadOnly);
                View.Property(p => p.WaitShelfQty).Readonly(isReadOnly).HasLabel("待入库");
                View.Property(p => p.WaitMaintain).Readonly(isReadOnly);
                View.Property(p => p.WaitReceive).Readonly(isReadOnly);
                View.Property(p => p.WaitRepair).Readonly(isReadOnly);
                View.Property(p => p.ToAccepted).Readonly(isReadOnly);
                View.Property(p => p.ScrapQty).Readonly(isReadOnly);
                View.Property(p => p.OriginalSN).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.Proprietorship).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.AssetCode).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SupplierCode).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SupplierName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CustomerCode).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CustomerName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.Manufacturer).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.ModelCode).Readonly(isReadOnly);
                View.Property(p => p.ModelName).Readonly(isReadOnly);
                View.Property(p => p.FixtureTypeCode).Readonly(isReadOnly);
                View.Property(p => p.FixedStorage).Readonly(isReadOnly);
                View.Property(p => p.BindProduct).Readonly(isReadOnly);
                View.Property(p => p.BindEquip).Readonly(isReadOnly);
                View.Property(p => p.UnitName).Readonly(isReadOnly);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.StockList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.UseResumeList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ToolList).Show(ChildShowInWhere.Hide);
            }

            View.AttachChildrenProperty(typeof(FixtureAccountStock), o =>
            {
                var account = o.Parent as FixtureCodeAccount;
                var stockList = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountStocksByAccountIds(new List<double>() { account.Id });
                return stockList;
            }, FixtureAccountStockViewConfig.AccountListView).HasLabel("库存详情");
            View.AttachChildrenProperty(typeof(FixtureAccountUseResume), o =>
            {
                var args = o as ChildPagingDataArgs;
                if (!args.SortInfo.Any())
                {
                    var orderInfo = new OrderInfo();
                    orderInfo.Property = FixtureAccountUseResume.OperationTimeProperty.Name;
                    orderInfo.SortIndex = 0;
                    orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Descending;
                    args.SortInfo.Add(orderInfo);
                }
                var account = o.Parent as FixtureCodeAccount;
                var useResumes = RT.Service.Resolve<CoreFixtureController>().GetPagingSortUseResumes(account.Id, args.PagingInfo, args.SortInfo.ToList());
                return useResumes;
            }, FixtureAccountUseResumeViewConfig.CodeUseResumeView).HasLabel("使用履历").Readonly(isReadOnly);
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));
            View.AssignAuthorize(typeof(FixtureCodeAccount));
            View.AddBehavior("SIE.Web.Fixtures.Accounts.Scripts.AddFixtureCodeAccountBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(EditSaveCodeAccCommand).FullName);
            using (View.DeclareGroup("选择编码/类型", 4))
            {
                View.Property(p => p.FixtureEncodeId).UseDataSource((e, page, code) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(ManageMode.Code, code, page);
                }).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.ModelCode), nameof(r.FixtureEncode.ModelCode));
                    keyValues.Add(nameof(r.ModelName), nameof(r.FixtureEncode.ModelName));
                    keyValues.Add(nameof(r.SlotType), nameof(r.FixtureEncode.SlotType));
                    keyValues.Add(nameof(r.FixtureTypeCode), nameof(r.FixtureEncode.FixtureType));
                    keyValues.Add(nameof(r.UnitName), nameof(r.FixtureEncode.FixtureModel.Unit.Name));
                    keyValues.Add(nameof(r.LoadingManage), nameof(r.FixtureEncode.LoadingManage));
                    keyValues.Add(nameof(r.ManageMode), nameof(r.FixtureEncode.ManageMode));
                    keyValues.Add(nameof(r.BindProduct), nameof(r.FixtureEncode.BindProduct));
                    keyValues.Add(nameof(r.BindEquip), nameof(r.FixtureEncode.BindEquip));
                    keyValues.Add(nameof(r.FixedStorage), nameof(r.FixtureEncode.FixedStorage));
                    //编码管理 工治具ID与工治具编码一致
                    keyValues.Add(nameof(r.Code), nameof(r.FixtureEncode.Code));
                    m.DicLinkField = keyValues;
                }).HasLabel("工治具编码").UseListSetting(e => { e.HelpInfo = "显示工治具编码"; });
                View.Property(p => p.ModelCode).Readonly(isReadOnly);
                View.Property(p => p.ModelName).Readonly(isReadOnly);
                View.Property(p => p.SlotType).Readonly(isReadOnly);
                View.Property(p => p.FixtureTypeCode).Readonly(isReadOnly).Show(ShowInWhere.All);
                View.Property(p => p.UnitName).Readonly(isReadOnly);
                View.Property(p => p.LoadingManage).Readonly(isReadOnly);
                View.Property(p => p.ManageMode).Readonly(isReadOnly);
                View.Property(p => p.BindProduct).Readonly(isReadOnly);
                View.Property(p => p.BindEquip).Readonly(isReadOnly);
                View.Property(p => p.FixedStorage).Readonly(isReadOnly);
            }
            using (View.DeclareGroup("维护详细信息", 4))
            {
                //View.Property(p => p.Code).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.SupplierName), nameof(r.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").UseListSetting(e => { e.HelpInfo = "显示供应商编码"; });
                View.Property(p => p.SupplierName).Readonly();
                View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.CustomerName), nameof(r.Customer.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("客户编码").UseListSetting(e => { e.HelpInfo = "显示客户编码"; });
                View.Property(p => p.CustomerName).Readonly();
                View.Property(p => p.Proprietorship);
                View.Property(p => p.TotalQty).HasLabel("数量*");
            }
            using (View.DeclareGroup("库存位置", 4, false))
            {
                View.Property(p => p.WarehouseId).UseDataSource((e, p, s) =>
                {
                    var model = e as FixtureCodeAccount;
                    if (model.FixtureEncode == null)
                    {
                        return new EntityList<Warehouse>();
                    }
                    return RT.Service.Resolve<CoreFixtureController>().GetWarehousesByEncodeId(model.FixtureEncodeId, s, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WarehouseName), nameof(e.Warehouse.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("仓库编码*").Show(ShowInWhere.All);
                View.Property(p => p.WarehouseName).Readonly().Show(ShowInWhere.All);
            }
        }
    }
}
