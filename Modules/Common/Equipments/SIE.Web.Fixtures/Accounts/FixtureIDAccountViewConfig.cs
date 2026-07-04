using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Warehouses;
using SIE.Web.Fixtures.Accounts.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台账（ID管理）-界面
    /// </summary>
    internal class FixtureIDAccountViewConfig : WebViewConfig<FixtureIDAccount>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));
        }

        private const bool isReadOnly = true;
        /// <summary>
        /// 配置列表界面
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Accounts.Scripts.FixtureIDAccountBehavior");
            View.FormEdit();
            View.UseCommands("SIE.Web.Fixtures.Accounts.Commands.AddIDAccountCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Rfid);
                View.Property(p => p.TotalQty);
                View.Property(p => p.AccountState);
                View.Property(p => p.QualityState);
                View.Property(p => p.OriginalSN);
                View.Property(p => p.AssetCode);
                View.Property(p => p.Proprietorship);
                View.Property(p => p.FixedAssetsAccountCode);
                View.Property(p => p.FixedAssetsAccountName);
                View.Property(p => p.SupplierCode);
                View.Property(p => p.SupplierName);
                View.Property(p => p.CustomerCode);
                View.Property(p => p.CustomerName);
                View.Property(p => p.ProductionDate);
                View.Property(p => p.InStorageDate);
                View.Property(p => p.Manufacturer);
                View.Property(p => p.IndustryProperties);
                View.Property(p => p.FixtureEncodeId).HasLabel("工治具编码");
                View.Property(p => p.ModelCode);
                View.Property(p => p.ModelName);
                View.Property(p => p.FixtureTypeCode);
                View.Property(p => p.FixedStorage);
                View.Property(p => p.LoadingManage);
                View.Property(p => p.BindProduct);
                View.Property(p => p.BindEquip);
                View.Property(p => p.UnitName);
                View.Property(p => p.SlotType);
                View.Property(p => p.MaxUseNum);
                View.Property(p => p.TotalUseNum);
                View.Property(p => p.MaxUseHour);
                View.Property(p => p.TotalUseHour);
                View.Property(p => p.MaintainNum);
                View.Property(p => p.MaintainedNum);
                View.Property(p => p.MaintainHour);
                View.Property(p => p.MaintainedHour);
                View.Property(p => p.WarnNum);
                View.Property(p => p.WarnHour);
                View.Property(p => p.OnlineHour);
                View.Property(p => p.MaintainEnforce);
                View.ChildrenProperty(p => p.StockList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ToolList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.UseResumeList).Show(ChildShowInWhere.Hide);
            }

            View.AttachChildrenProperty(typeof(FixtureAccountStock), o =>
            {
                var account = o.Parent as FixtureIDAccount;
                var stockList = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountStocksByAccountIds(new List<double>() { account.Id });
                return stockList;
            }, FixtureAccountStockViewConfig.AccountListView).HasLabel("库存详情");
            View.AttachChildrenProperty(typeof(FixtureAccountTool), o =>
            {
                var account = o.Parent as FixtureIDAccount;
                var toolList = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountTools(account.Id);
                return toolList;
            }).HasLabel("feeder详情");
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
                var account = o.Parent as FixtureIDAccount;
                var useResumes = RT.Service.Resolve<CoreFixtureController>().GetPagingSortUseResumes(account.Id, args.PagingInfo, args.SortInfo.ToList());
                return useResumes;
            }).HasLabel("使用履历").Readonly();
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Accounts.Scripts.AddFixtureIDAccountBehavior");
            View.UseCommand(typeof(EditSaveIDAccountCommand).FullName);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("选择编码/类型", 4, false))
                {
                    View.Property(p => p.FixtureEncodeId).UseDataSource((e, page, code) =>
                    {
                        return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(ManageMode.Number, code, page);
                    }).UsePagingLookUpEditor((m, r) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(r.ModelCode), nameof(r.FixtureEncode.ModelCode));
                        keyValues.Add(nameof(r.ModelName), nameof(r.FixtureEncode.ModelName));
                        keyValues.Add(nameof(r.SlotType), nameof(r.FixtureEncode.SlotType));
                        keyValues.Add(nameof(r.FixtureTypeCode), nameof(r.FixtureEncode.FixtureType));
                        keyValues.Add(nameof(r.UnitName), nameof(r.FixtureEncode.UnitName));
                        keyValues.Add(nameof(r.LoadingManage), nameof(r.FixtureEncode.LoadingManage));
                        keyValues.Add(nameof(r.ManageMode), nameof(r.FixtureEncode.ManageMode));
                        keyValues.Add(nameof(r.BindProduct), nameof(r.FixtureEncode.BindProduct));
                        keyValues.Add(nameof(r.BindEquip), nameof(r.FixtureEncode.BindEquip));

                        keyValues.Add(nameof(r.MaxUseNum), nameof(r.FixtureEncode.FixtureModel.MaxUseNum));
                        keyValues.Add(nameof(r.MaxUseHour), nameof(r.FixtureEncode.FixtureModel.MaxUseHour));
                        keyValues.Add(nameof(r.MaintainNum), nameof(r.FixtureEncode.FixtureModel.MaintainNum));
                        keyValues.Add(nameof(r.MaintainHour), nameof(r.FixtureEncode.FixtureModel.MaintainHour));
                        keyValues.Add(nameof(r.OnlineHour), nameof(r.FixtureEncode.FixtureModel.OnlineHour));
                        keyValues.Add(nameof(r.MaintainEnforce), nameof(r.FixtureEncode.FixtureModel.MaintainEnforce));
                        keyValues.Add(nameof(r.WarnNum), nameof(r.FixtureEncode.FixtureModel.WarnNum));
                        keyValues.Add(nameof(r.WarnHour), nameof(r.FixtureEncode.FixtureModel.WarnHour));

                        keyValues.Add(nameof(r.FixedStorage), nameof(r.FixtureEncode.FixedStorage));
                        keyValues.Add(nameof(r.IndustryProperties), nameof(r.FixtureEncode.FixtureModel.IndustryProperties));
                        m.DicLinkField = keyValues;
                    }).HasLabel("工治具编码").Show(ShowInWhere.All);
                    View.Property(p => p.ModelCode).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.ModelName).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.SlotType).Readonly(isReadOnly).Show(ShowInWhere.All);

                    View.Property(p => p.FixtureTypeCode).HasLabel("工治具类型").Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.UnitName).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.LoadingManage).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.ManageMode).Readonly(isReadOnly).Show(ShowInWhere.All);

                    View.Property(p => p.BindProduct).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.BindEquip).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.MaxUseNum).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.MaxUseHour).Readonly(isReadOnly).Show(ShowInWhere.All);

                    View.Property(p => p.MaintainNum).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.MaintainHour).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.OnlineHour).Readonly(isReadOnly).HasLabel("上线定期保养标准时数").Show(ShowInWhere.All);
                    View.Property(p => p.MaintainEnforce).Readonly(isReadOnly).Show(ShowInWhere.All);

                    View.Property(p => p.WarnNum).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.WarnHour).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.FixedStorage).Cascade(p => p.LocationId, null).Cascade(p => p.LocationName, null).Cascade(p => p.WarehouseId, null).Cascade(p => p.WarehouseName, null).Readonly(isReadOnly).Show(ShowInWhere.All);
                    View.Property(p => p.IndustryProperties).Readonly(isReadOnly).Show(ShowInWhere.All);
                }
                using (View.DeclareGroup("维护详细信息", 4, false))
                {
                    View.Property(p => p.Code).Show(ShowInWhere.All);
                    View.Property(p => p.TotalQty).DefaultValue(1).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.OriginalSN).Show(ShowInWhere.All);
                    View.Property(p => p.Rfid).Show(ShowInWhere.All);

                    View.Property(p => p.AssetCode).Show(ShowInWhere.All);
                    View.Property(p => p.Proprietorship).Show(ShowInWhere.All);
                    View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                        m.DicLinkField = keyValues;
                    }).HasLabel("供应商编码").Show(ShowInWhere.All);
                    View.Property(p => p.SupplierName).Readonly().Show(ShowInWhere.All);

                    View.Property(p => p.CustomerId).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
                        m.DicLinkField = keyValues;
                    }).HasLabel("客户编码").Show(ShowInWhere.All);
                    View.Property(p => p.CustomerName).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.ProductionDate).UseDateEditor().Show(ShowInWhere.All);
                    View.Property(p => p.Manufacturer).Show(ShowInWhere.All);
                }
                //维护详细信息 库存位置
                PageTwo();
            }
        }

        /// <summary>
        /// 维护详细信息 库存位置
        /// </summary>
        protected void PageTwo()
        {
            using (View.DeclareGroup("库存位置", 4, false))
            {
                View.Property(p => p.WarehouseId).UseDataSource((e, p, s) =>
                {
                    var model = e as FixtureIDAccount;
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
                }).HasLabel("仓库编码*").Cascade(p => p.LocationId, null).Cascade(p => p.LocationName, null).Show(ShowInWhere.All);
                View.Property(p => p.WarehouseName).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.LocationId).UseDataSource((e, p, s) =>
                {
                    var model = e as FixtureIDAccount;
                    if (model.FixtureEncode == null || model.WarehouseId == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<CoreFixtureController>().GetStorageLocationsByEncodeId(model.FixtureEncodeId, model.WarehouseId.Value, s, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.LocationName), nameof(e.Location.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("库位编码").Show(ShowInWhere.All);
                View.Property(p => p.LocationName).Readonly(isReadOnly).Show(ShowInWhere.All);
            }

        }

    }
}
