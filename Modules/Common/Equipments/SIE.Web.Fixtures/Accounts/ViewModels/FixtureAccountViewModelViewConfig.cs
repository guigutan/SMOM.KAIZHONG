using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Fixtures.Accounts.ViewModels;
using SIE.Warehouses;
using SIE.Web.Fixtures.Accounts.Commands;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Accounts.ViewModels
{
    /// <summary>
    /// 工治具台帐ViewModel
    /// </summary>
    public class FixtureAccountViewModelViewConfig : WebViewConfig<FixtureAccountViewModel>
    {
        /// <summary>
        /// 添加ID类工治具台帐界面
        /// </summary>
        public const string AddIDAccount = "AddIDAccount";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddIDAccount);
            View.AssignAuthorize(typeof(FixtureIDAccount), typeof(FixtureCodeAccount));
            if (ViewGroup == AddIDAccount)
                AddIDAccountView();
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(FixtureCodeAccount));
            View.ClearCommands();
            View.UseCommands(typeof(SaveCodeAccCommand).FullName);
            View.AddBehavior("SIE.Web.Fixtures.Accounts.Scripts.AddCodeAccBehavior");
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
                    keyValues.Add(nameof(r.FixtureType), nameof(r.FixtureEncode.FixtureType));
                    keyValues.Add(nameof(r.UnitName), nameof(r.FixtureEncode.UnitName));
                    keyValues.Add(nameof(r.LoadingManage), nameof(r.FixtureEncode.LoadingManage));
                    keyValues.Add(nameof(r.ManageMode), nameof(r.FixtureEncode.ManageMode));
                    keyValues.Add(nameof(r.BindProduct), nameof(r.FixtureEncode.BindProduct));
                    keyValues.Add(nameof(r.BindEquip), nameof(r.FixtureEncode.BindEquip));
                    keyValues.Add(nameof(r.FixedStorage), nameof(r.FixtureEncode.FixedStorage));
                    m.DicLinkField = keyValues;
                }).HasLabel("工治具编码").UseListSetting(e => { e.HelpInfo = "显示工治具编码"; });
                View.Property(p => p.ModelCode).Readonly();
                View.Property(p => p.ModelName).Readonly();
                View.Property(p => p.SlotType).Readonly();
                View.Property(p => p.FixtureType).Readonly();
                View.Property(p => p.UnitName).Readonly();
                View.Property(p => p.LoadingManage).Readonly();
                View.Property(p => p.ManageMode).Readonly();
                View.Property(p => p.BindProduct).Readonly();
                View.Property(p => p.BindEquip).Readonly();
                View.Property(p => p.FixedStorage).Readonly();
            }

            using (View.DeclareGroup("维护详细信息", 4))
            {
                View.Property(p => p.Code).Readonly();
                View.Property(p => p.Qty);
                View.Property(p => p.AccountState).Readonly();
                View.Property(p => p.OriginalSN);
                View.Property(p => p.AssetCode);
                View.Property(p => p.Proprietorship);
                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.SupplierName), nameof(r.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").UseListSetting(e => { e.HelpInfo = "显示供应商编码"; });
                View.Property(p => p.SupplierName).Readonly();
                View.Property(p => p.CustomerId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.CustomerName), nameof(r.Customer.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("客户编码").UseListSetting(e => { e.HelpInfo = "显示客户编码"; });
                View.Property(p => p.CustomerName).Readonly();
                View.Property(p => p.ProductDate);
                View.Property(p => p.Manufacturer);
                View.Property(p => p.UnitPrice);
                
            }
        }

        /// <summary>
        /// 添加ID类工治具台账界面
        /// </summary>
        protected void AddIDAccountView()
        {
            View.AssignAuthorize(typeof(FixtureIDAccount));
            View.AddBehavior("SIE.Web.Fixtures.Accounts.AddIDAccountBehavior");
            View.UseCommands(typeof(SaveIDAccountCommand).FullName);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("选择编码/类型", 4, false))
                {
                    View.Property(p => p.FixtureEncodeId).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.FixtureModel.Code));
                        keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
                        keyValues.Add(nameof(e.SlotType), nameof(e.FixtureEncode.FixtureModel.SlotType));
                        keyValues.Add(nameof(e.FixtureType), nameof(e.FixtureEncode.FixtureModel.FixtureType));
                        keyValues.Add(nameof(e.UnitName), nameof(e.FixtureEncode.UnitName));
                        keyValues.Add(nameof(e.LoadingManage), nameof(e.FixtureEncode.FixtureModel.LoadingManage));
                        keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureEncode.FixtureModel.ManageMode));
                        keyValues.Add(nameof(e.BindProduct), nameof(e.FixtureEncode.FixtureModel.BindProduct));
                        keyValues.Add(nameof(e.BindEquip), nameof(e.FixtureEncode.FixtureModel.BindEquip));
                        keyValues.Add(nameof(e.MaxUseNum), nameof(e.FixtureEncode.FixtureModel.MaxUseNum));
                        keyValues.Add(nameof(e.MaxUseHour), nameof(e.FixtureEncode.FixtureModel.MaxUseHour));
                        keyValues.Add(nameof(e.MaintainNum), nameof(e.FixtureEncode.FixtureModel.MaintainNum));
                        keyValues.Add(nameof(e.MaintainHour), nameof(e.FixtureEncode.FixtureModel.MaintainHour));
                        keyValues.Add(nameof(e.OnlineHour), nameof(e.FixtureEncode.FixtureModel.OnlineHour));
                        keyValues.Add(nameof(e.MaintainEnforce), nameof(e.FixtureEncode.FixtureModel.MaintainEnforce));
                        keyValues.Add(nameof(e.WarnNum), nameof(e.FixtureEncode.FixtureModel.WarnNum));
                        keyValues.Add(nameof(e.WarnHour), nameof(e.FixtureEncode.FixtureModel.WarnHour));
                        keyValues.Add(nameof(e.FixedStorage), nameof(e.FixtureEncode.FixtureModel.FixedStorage));
                        m.DicLinkField = keyValues;
                    }).UseDataSource((e, p, s) =>
                    {
                        return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(ManageMode.Number, s, p);
                    }).HasLabel("工治具编码").Show(ShowInWhere.All);
                    View.Property(p => p.ModelCode).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.ModelName).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.SlotType).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.FixtureType).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.UnitName).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.LoadingManage).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.ManageMode).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.BindProduct).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.BindEquip).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.MaxUseNum).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.MaxUseHour).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.MaintainNum).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.MaintainHour).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.OnlineHour).Readonly().HasLabel("上线定期保养标准时数").Show(ShowInWhere.All);
                    View.Property(p => p.MaintainEnforce).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.WarnNum).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.WarnHour).Readonly().Show(ShowInWhere.All);
                    View.Property(p => p.FixedStorage).Readonly().Show(ShowInWhere.All);
                }

                using (View.DeclareGroup("维护详细信息", 4, false))
                {
                    ConfigViewDetailInfo();
                }

                using (View.DeclareGroup("库存位置", 4, false))
                {
                    ConfigViewStorageLocation();
                }
            }
        }

        private void ConfigViewStorageLocation()
        {
            View.Property(p => p.FixtureWarehouseId).UseDataSource((e, p, s) =>
            {
                var model = e as FixtureAccountViewModel;
                if (model.FixtureEncode == null)
                    return new EntityList<Warehouse>();
                return RT.Service.Resolve<CoreFixtureController>().GetWarehousesByEncodeId(model.FixtureEncodeId, s, p);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.FixtureWarehouseName), nameof(e.Warehouse.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("仓库编码*").Readonly(p => p.FixedStorage == YesNo.No)
            .UseListSetting(e => { e.HelpInfo = "对应治具编码下的仓库,【固定储位】为是时，需要维护"; }).Show(ShowInWhere.All);
            View.Property(p => p.FixtureWarehouseName).HasLabel("仓库名称*").Readonly().Show(ShowInWhere.All);
            View.Property(p => p.FixtureStorageLocationId).UseDataSource((e, p, s) =>
            {
                var model = e as FixtureAccountViewModel;
                if (model.FixtureEncode == null || model.FixtureWarehouseId == null)
                    return new EntityList<StorageLocation>();
                return RT.Service.Resolve<CoreFixtureController>().GetStorageLocationsByEncodeId(model.FixtureEncodeId, model.FixtureWarehouseId.Value, s, p);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.FixtureStorageLocationName), nameof(e.StorageLocation.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("库位编码").Readonly(p => p.FixedStorage == YesNo.No)
            .UseListSetting(e => { e.HelpInfo = "对应工治具编码下的库位，【固定储位】为是时，需要维护"; }).Show(ShowInWhere.All);
            View.Property(p => p.FixtureStorageLocationName).Readonly().Show(ShowInWhere.All);
        }

        private void ConfigViewDetailInfo()
        {
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Qty).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.AccountState).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.OriginalSN).Show(ShowInWhere.All);
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
            View.Property(p => p.ProductDate).Show(ShowInWhere.All);
            View.Property(p => p.Manufacturer).Show(ShowInWhere.All);
            View.Property(p => p.UnitPrice).Show(ShowInWhere.All);
            View.Property(p => p.Rfid).Show(ShowInWhere.All);
        }
    }
}
