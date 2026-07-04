using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Warehouses.Stations;
using SIE.Web.Common;
using SIE.Web.Inventory.Strategy.Commands;
using System;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 上架规则明细视图配置
    /// </summary>
    public class OnShelvesRuleDetailViewConfig : WebViewConfig<OnShelvesRuleDetail>
    {
        #region  扩展查看视图

        #region 扩展查看视图
        /// <summary>
        ///  扩展查看视图
        /// </summary>
        public const string OnShelvesRuleReadOnlyView = "OnShelvesRuleReadOnlyView";

        /// <summary>
        /// 扩展查看视图
        /// </summary>
        public void OnShelvesRuleReadOnly()
        {
            View.ClearCommands();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInDetail(columnSpan: 1).Readonly().Show();
                View.Property(p => p.Strategy).ShowInDetail(columnSpan: 2).Readonly().Show();
                View.Property(p => p.SceneType).ShowInDetail(columnSpan: 1).Readonly().Show();
                View.Property(p => p.FromLocation).HasLabel("来源库位").Readonly().Show();
                View.Property(p => p.ToArea).HasLabel("目标库区").Readonly().Show();
                View.Property(p => p.ToLocation).HasLabel("目标库位").Readonly().Show();
                View.Property(p => p.ToLogicArea).Readonly().Show();
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, SourceLimitationReadOnlyView).HasLabel("来源限制").Show(ChildShowInWhere.All).OrderNo = 0;
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, StayBatchLimitationReadOnlyView).HasLabel("待上架批次属性限制").Show(ChildShowInWhere.All).OrderNo = 1;
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, InventoryLimitationReadOnlyView).HasLabel("目标库位库存限制").Show(ChildShowInWhere.All).OrderNo = 2;
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, InventoryBathLimitationReadOnlyView).HasLabel("目标库位库存批次属性限制").Show(ChildShowInWhere.All).OrderNo = 3;
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, InventorySpaceLimitationReadOnlyView).HasLabel("目标库位空间限制").Show(ChildShowInWhere.All).OrderNo = 4;
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, InventoryOperationLimitationReadOnlyView).HasLabel("目标库位操作限制").Show(ChildShowInWhere.All).OrderNo = 5;
                View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
                {
                    return getData(c);
                }, InventoryStorageLimitationReadOnlyView).HasLabel("目标库位存储限制").Show(ChildShowInWhere.All).OrderNo = 6;
            }
        }
        #endregion

        #region 来源限制
        /// <summary>
        /// 来源限制
        /// </summary>
        public const string SourceLimitationReadOnlyView = "SourceLimitationReadOnlyView";

        /// <summary>
        /// 查看视图
        /// </summary>
        public void SourceLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType).Readonly().Show();
                View.Property(p => p.Transaction).Readonly().Show().HasLabel("单据小类");
                View.Property(p => p.Shipper).Readonly().Show().HasLabel("货主名称");
                View.Property(p => p.AbcType).Readonly().Show();
                View.Property(p => p.FromItemCategory).Readonly().Show();
            }
        }
        #endregion

        #region 待上架批次属性
        /// <summary>
        /// 待上架批次属性限制
        /// </summary>
        public const string StayBatchLimitationReadOnlyView = "StayBatchLimitationReadOnlyView";
        public void StayBatchLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.FromLotAtt01).HasLabel("限制1").Readonly().Show();
                View.Property(p => p.FromLotAtt01Value).HasLabel(string.Empty).Readonly().Show();
                View.Property(p => p.FromLotAtt02).HasLabel("限制2").Readonly().Show();
                View.Property(p => p.FromLotAtt02Value).HasLabel(string.Empty).Readonly().Show();
                View.Property(p => p.FromLotAtt03).HasLabel("限制3").Readonly().Show();
                View.Property(p => p.FromLotAtt03Value).HasLabel(string.Empty).Readonly().Show();
                View.Property(p => p.FromLotAtt04).HasLabel("限制4").Readonly().Show();
                View.Property(p => p.FromLotAtt04Value).HasLabel(string.Empty).Readonly().Show();
            }
        }
        #endregion

        #region 目标库位库存限制
        /// <summary>
        /// 目标库位库存限制
        /// </summary>
        public const string InventoryLimitationReadOnlyView = "InventoryLimitationReadOnlyView";
        public void InventoryLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.MaxItemNum).Readonly().Show();
                View.Property(p => p.MaxLotNum).Readonly().Show();
                View.Property(p => p.LocationState).Readonly().Show();
                View.Property(p => p.MaxLocNum).Readonly().Show();
                View.Property(p => p.ItemCategory).HasLabel("库存类别").Readonly().Show();
                View.Property(p => p.ExistsItem).HasLabel("具有的物料库存").Readonly().Show();
            }
        }
        #endregion

        #region 目标库位库存批次属性限制
        /// <summary>
        /// 目标库位库存批次属性限制
        /// </summary>
        public const string InventoryBathLimitationReadOnlyView = "InventoryBathLimitationReadOnlyView";
        public void InventoryBathLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.ToLotAtt01).HasLabel("限制1").Readonly().Show();
                View.Property(p => p.ToLotAtt01Value).HasLabel(string.Empty).Readonly().Show();
                View.Property(p => p.ToLotAtt02).HasLabel("限制2").Readonly().Show();
                View.Property(p => p.ToLotAtt02Value).HasLabel(string.Empty).Readonly().Show();
                View.Property(p => p.ToLotAtt03).HasLabel("限制3").Readonly().Show();
                View.Property(p => p.ToLotAtt03Value).HasLabel(string.Empty).Readonly().Show();
                View.Property(p => p.ToLotAtt04).HasLabel("限制4").Readonly().Show();
                View.Property(p => p.ToLotAtt04Value).HasLabel(string.Empty).Readonly().Show();
            }
        }
        #endregion

        #region 目标库位空间限制
        /// <summary>
        /// 目标库位空间限制
        /// </summary>
        public const string InventorySpaceLimitationReadOnlyView = "InventorySpaceLimitationReadOnlyView";
        public void InventorySpaceLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.SpaceLimit1).HasLabel("条件1").Readonly().Show();
                View.Property(p => p.SpaceLimit2).HasLabel("条件2").Readonly().Show();
                View.Property(p => p.SpaceLimit3).HasLabel("条件3").Readonly().Show();
                View.Property(p => p.SpaceLimit4).HasLabel("条件4").Readonly().Show();
            }
        }
        #endregion

        #region 目标库位操作限制
        /// <summary>
        /// 目标库位操作限制
        /// </summary>
        public const string InventoryOperationLimitationReadOnlyView = "InventoryOperationLimitationReadOnlyView";
        public void InventoryOperationLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.UpProcessType).Readonly().Show();
                View.Property(p => p.PickProcessType).Readonly().Show();
                View.Property(p => p.LocationType).Readonly().Show();
            }

        }
        #endregion

        #region 目标库位存储限制
        /// <summary>
        /// 目标库位存储限制
        /// </summary>
        public const string InventoryStorageLimitationReadOnlyView = "InventoryStorageLimitationReadOnlyView";
        public void InventoryStorageLimitationReadOnly()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.StorageLimit1).HasLabel("条件1").Readonly().Show();
                View.Property(p => p.StorageLimit2).HasLabel("条件2").Readonly().Show();
                View.Property(p => p.StorageLimit3).HasLabel("条件3").Readonly().Show();
                View.Property(p => p.StorageLimit4).HasLabel("条件4").Readonly().Show();
            }
        }
        #endregion

        #endregion

        #region  编辑视图

        #region 来源限制
        /// <summary>
        /// 来源限制
        /// </summary>
        public const string SourceLimitationDetailView = "SourceLimitationDetailView";
        public void SourceLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType).UseEnumEditor("RECEIPT").Cascade(p => p.Transaction, null)
                    .UseListSetting(e => { e.HelpInfo = "更改订单类型清空单据小类"; }).ShowInDetail();
                View.Property(p => p.Transaction).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).HasLabel("单据小类").UseDataSource((e, c, r) =>
                {
                    var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                    if (onShelvesRuleDetail == null || !onShelvesRuleDetail.OrderType.HasValue)
                        return new EntityList<Transaction>();
                    return RT.Service.Resolve<TransactionController>().GetTransactions(c, r, (OrderType)onShelvesRuleDetail.OrderType);
                }).Readonly(p => p.OrderType == null)
                .UseListSetting(e => { e.HelpInfo = "订单类型为空不可编辑"; }).ShowInDetail();
                View.Property(p => p.Shipper).UseDataSource((e, c, r) =>
                {
                    var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                    if (onShelvesRuleDetail == null)
                        return new EntityList<Customer>();
                    return RT.Service.Resolve<CustomerController>().GetCustomer(CustomerType.SHIPPER, r, c);
                }).HasLabel("货主").UseListSetting(e => { e.HelpInfo = "显示类型为货主的客户列表"; }).ShowInDetail();
                View.Property(p => p.AbcType).ShowInDetail();
                View.Property(p => p.FromItemCategory).UseDataSource((e, p, s) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemCategoryByType(CategoryType.Item, null, p,s);
                }).ShowInDetail();
                View.Property(p => p.TrunoverBoxModel).ShowInDetail();
            }
        }
        #endregion

        #region 待上架批次属性限制
        /// <summary>
        /// 待上架批次属性限制
        /// </summary>
        public const string StayBatchLimitationDetailView = "StayBatchLimitationDetailView";
        public void StayBatchLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.FromLotAtt01).HasLabel("限制1").ShowInDetail();
                View.Property(p => p.FromLotAtt01Value).HasLabel(string.Empty).ShowInDetail();
                View.Property(p => p.FromLotAtt02).HasLabel("限制2").ShowInDetail();
                View.Property(p => p.FromLotAtt02Value).HasLabel(string.Empty).ShowInDetail();
                View.Property(p => p.FromLotAtt03).HasLabel("限制3").ShowInDetail();
                View.Property(p => p.FromLotAtt03Value).HasLabel(string.Empty).ShowInDetail();
                View.Property(p => p.FromLotAtt04).HasLabel("限制4").ShowInDetail();
                View.Property(p => p.FromLotAtt04Value).HasLabel(string.Empty).ShowInDetail();
            }
        }
        #endregion

        #region 目标库位库存限制
        /// <summary>
        /// 目标库位库存限制
        /// </summary>
        public const string InventoryLimitationDetailView = "InventoryLimitationDetailView";
        public void InventoryLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.MaxItemNum).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.MaxLotNum).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.LocationState).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.MaxLocNum).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ItemCategory).UseDataSource((e, p, s) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemCategoryByType(CategoryType.Item, null, p);
                }).HasLabel("库存类别").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ExistsItem).UseDataSource((e, c, r) =>
                {
                    var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                    if (onShelvesRuleDetail == null)
                        return new EntityList<Item>();
                    return RT.Service.Resolve<ItemController>().GetItemsFormType(null, State.Enable, r, c);
                }).HasLabel("具有的物料库存").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.IsSameItemOnhand).UseCheckDropDownEditor().Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
            }
        }
        #endregion

        #region 目标库位库存批次属性限制
        /// <summary>
        /// 目标库位库存批次属性限制
        /// </summary>
        public const string InventoryBathLimitationDetailView = "InventoryBathLimitationDetailView";
        public void InventoryBathLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.ToLotAtt01).HasLabel("限制1").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt01Value).HasLabel(string.Empty).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt02).HasLabel("限制2").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt02Value).HasLabel(string.Empty).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt03).HasLabel("限制3").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt03Value).HasLabel(string.Empty).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt04).HasLabel("限制4").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.ToLotAtt04Value).HasLabel(string.Empty).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
            }
        }
        #endregion 目标库位库存批次属性限制

        #region 目标库位空间限制
        /// <summary>
        /// 目标库位空间限制
        /// </summary>
        public const string InventorySpaceLimitationDetailView = "InventorySpaceLimitationDetailView";
        public void InventorySpaceLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.SpaceLimit1).HasLabel("条件1").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.SpaceLimit2).HasLabel("条件2").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.SpaceLimit3).HasLabel("条件3").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.SpaceLimit4).HasLabel("条件4").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
            }
        }
        #endregion

        #region 目标库位操作限制
        /// <summary>
        /// 目标库位操作限制
        /// </summary>
        public const string InventoryOperationLimitationDetailView = "InventoryOperationLimitationDetailView";
        public void InventoryOperationLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 3);
            using (View.OrderProperties())
            {
                View.Property(p => p.UpProcessType).ShowInDetail(columnSpan: 1).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.PickProcessType).ShowInDetail(columnSpan: 1).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.LocationType).UseCatalogEditor(p => { p.CatalogType = StorageLocationInfo.LOCATIONFORM;p.CatalogReloadData = true; }).ShowInDetail(columnSpan: 1)
                    .UseListSetting(e => { e.HelpInfo = "库位形式快码类型(LOCATION_FORM)"; }).Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
            }

        }
        #endregion

        #region  目标库位存储限制
        /// <summary>
        /// 目标库位存储限制
        /// </summary>
        public const string InventoryStorageLimitationDetailView = "InventoryStorageLimitationDetailView";
        public void InventoryStorageLimitationDetail()
        {
            BeforeCreate();
            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.StorageLimit1).HasLabel("条件1").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.StorageLimit2).HasLabel("条件2").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.StorageLimit3).HasLabel("条件3").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
                View.Property(p => p.StorageLimit4).HasLabel("条件4").Readonly(p => p.SceneType == SceneType.ASRS).ShowInDetail();
            }
        }
        #endregion

        #endregion

        #region 公共
        /// <summary>
        /// 子表生成前操作
        /// </summary>
        void BeforeCreate()
        {
            View.AddBehavior("SIE.Web.Inventory.Strategy.Scripts.OnShelvesRuleDetailBehavior");
            View.ClearCommands();
        }
        /// <summary>
        /// 子表获取数据
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        OnShelvesRuleDetail getData(ChildDataArgs c)
        {
            var parent = c.Parent as OnShelvesRuleDetail;
            return parent;
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            #region 注册视图
            View.DeclareExtendViewGroup(OnShelvesRuleReadOnlyView);
            View.DeclareExtendViewGroup(SourceLimitationReadOnlyView);
            View.DeclareExtendViewGroup(StayBatchLimitationReadOnlyView);
            View.DeclareExtendViewGroup(InventoryLimitationReadOnlyView);
            View.DeclareExtendViewGroup(InventoryBathLimitationReadOnlyView);
            View.DeclareExtendViewGroup(InventorySpaceLimitationReadOnlyView);
            View.DeclareExtendViewGroup(InventoryOperationLimitationReadOnlyView);
            View.DeclareExtendViewGroup(InventoryStorageLimitationReadOnlyView);
            View.DeclareExtendViewGroup(SourceLimitationDetailView);
            View.DeclareExtendViewGroup(StayBatchLimitationDetailView);
            View.DeclareExtendViewGroup(InventoryLimitationDetailView);
            View.DeclareExtendViewGroup(InventoryBathLimitationDetailView);
            View.DeclareExtendViewGroup(InventorySpaceLimitationDetailView);
            View.DeclareExtendViewGroup(InventoryOperationLimitationDetailView);
            View.DeclareExtendViewGroup(InventoryStorageLimitationDetailView);
            #endregion

            switch (ViewGroup)
            {
                case OnShelvesRuleReadOnlyView:
                    OnShelvesRuleReadOnly();
                    break;
                case SourceLimitationReadOnlyView:
                    SourceLimitationReadOnly();
                    break;
                case StayBatchLimitationReadOnlyView:
                    StayBatchLimitationReadOnly();
                    break;
                case InventoryLimitationReadOnlyView:
                    InventoryLimitationReadOnly();
                    break;
                case InventoryBathLimitationReadOnlyView:
                    InventoryBathLimitationReadOnly();
                    break;
                case InventorySpaceLimitationReadOnlyView:
                    InventorySpaceLimitationReadOnly();
                    break;
                case InventoryOperationLimitationReadOnlyView:
                    InventoryOperationLimitationReadOnly();
                    break;
                case InventoryStorageLimitationReadOnlyView:
                    InventoryStorageLimitationReadOnly();
                    break;
                case SourceLimitationDetailView:
                    SourceLimitationDetail();
                    break;
                case StayBatchLimitationDetailView:
                    StayBatchLimitationDetail();
                    break;
                case InventoryLimitationDetailView:
                    InventoryLimitationDetail();
                    break;
                case InventoryBathLimitationDetailView:
                    InventoryBathLimitationDetail();
                    break;
                case InventorySpaceLimitationDetailView:
                    InventorySpaceLimitationDetail();
                    break;
                case InventoryOperationLimitationDetailView:
                    InventoryOperationLimitationDetail();
                    break;
                case InventoryStorageLimitationDetailView:
                    InventoryStorageLimitationDetail();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddOnShelvesRuleDetailCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditOnShelvesRuleDetailCommand).FullName);
            View.UseCommands(typeof(ViewOnShelvesRuleDetailCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.DeclareBand("基础信息".L10N()))
            {
                View.Property(p => p.LineNo).Readonly().FixColumn();
                View.Property(p => p.SceneType).ShowInDetail(columnSpan: 1);
                View.Property(p => p.Strategy).Readonly().FixColumn();
                View.Property(p => p.FromLogicAreaId).Readonly();
                View.Property(p => p.FromLocation).HasLabel("来源库位".L10N()).Readonly();
                View.Property(p => p.ToArea).HasLabel("目标库区".L10N()).Readonly();
                View.Property(p => p.ToLocation).HasLabel("目标库位".L10N()).Readonly();
                View.Property(p => p.ToLogicArea).Readonly();
                View.Property(p => p.ToLogicAreaId).Readonly();
                View.Property(p => p.FromStationId).Readonly();
                View.Property(p => p.ToStationId).Readonly();
                View.Property(p => p.ToStationGroupId).Readonly();
                View.Property(p => p.OrderType).Readonly();
                View.Property(p => p.Transaction).HasLabel("单据小类".L10N()).Readonly();
                View.Property(p => p.ShipperName).HasLabel("货主名称".L10N()).Readonly();
                View.Property(p => p.AbcType).Readonly();
                View.Property(p => p.FromItemCategory).Readonly();
                View.Property(p => p.MaxItemNum).Readonly();
                View.Property(p => p.MaxLotNum).Readonly();
                View.Property(p => p.LocationState).Readonly();
                View.Property(p => p.MaxLocNum).Readonly();
                View.Property(p => p.ItemCategory).HasLabel("库存类别".L10N()).Readonly();
                View.Property(p => p.TrunoverBoxModelId).Readonly();
                View.Property(p => p.ExistsItem).HasLabel("具有的物料库存".L10N()).Readonly();
                View.Property(p => p.UpProcessType).Readonly();
                View.Property(p => p.PickProcessType).Readonly();
                View.Property(p => p.LocationType).Readonly();
            }

            using (View.DeclareBand("待上架物料批次属性".L10N()))
            {
                View.Property(p => p.FromLotAtt01).HasLabel("属性1".L10N()).Readonly();
                View.Property(p => p.FromLotAtt02).HasLabel("属性2".L10N()).Readonly();
                View.Property(p => p.FromLotAtt03).HasLabel("属性3".L10N()).Readonly();
                View.Property(p => p.FromLotAtt04).HasLabel("属性4".L10N()).Readonly();
                View.Property(p => p.FromLotAtt01Value).HasLabel("属性1值".L10N()).Readonly();
                View.Property(p => p.FromLotAtt02Value).HasLabel("属性2值".L10N()).Readonly();
                View.Property(p => p.FromLotAtt03Value).HasLabel("属性3值".L10N()).Readonly();
                View.Property(p => p.FromLotAtt04Value).HasLabel("属性4值".L10N()).Readonly();
            }

            using (View.DeclareBand("目标库位库存批次属性".L10N()))
            {
                View.Property(p => p.ToLotAtt01).HasLabel("属性1".L10N()).Readonly();
                View.Property(p => p.ToLotAtt02).HasLabel("属性2".L10N()).Readonly();
                View.Property(p => p.ToLotAtt03).HasLabel("属性3".L10N()).Readonly();
                View.Property(p => p.ToLotAtt04).HasLabel("属性4".L10N()).Readonly();
                View.Property(p => p.ToLotAtt01Value).HasLabel("属性1值".L10N()).Readonly();
                View.Property(p => p.ToLotAtt02Value).HasLabel("属性2值".L10N()).Readonly();
                View.Property(p => p.ToLotAtt03Value).HasLabel("属性3值".L10N()).Readonly();
                View.Property(p => p.ToLotAtt04Value).HasLabel("属性4值".L10N()).Readonly();
            }

            using (View.DeclareBand("储存限制".L10N()))
            {
                View.Property(p => p.StorageLimit1).HasLabel("条件1".L10N()).Readonly();
                View.Property(p => p.StorageLimit2).HasLabel("条件2".L10N()).Readonly();
                View.Property(p => p.StorageLimit3).HasLabel("条件3".L10N()).Readonly();
                View.Property(p => p.StorageLimit4).HasLabel("条件4".L10N()).Readonly();
            }

            using (View.DeclareBand("空间限制".L10N()))
            {
                View.Property(p => p.SpaceLimit1).HasLabel("条件1".L10N()).Readonly();
                View.Property(p => p.SpaceLimit2).HasLabel("条件2".L10N()).Readonly();
                View.Property(p => p.SpaceLimit3).HasLabel("条件3".L10N()).Readonly();
                View.Property(p => p.SpaceLimit4).HasLabel("条件4".L10N()).Readonly();
            }

            using (View.DeclareBand("操作用户".L10N()))
            {
                View.Property(p => p.CreateByName).Readonly();
                View.Property(p => p.CreateDate).Readonly();
                View.Property(p => p.UpdateByName).Readonly();
                View.Property(p => p.UpdateDate).Readonly();
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.LineNo).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.SceneType).ShowInDetail(columnSpan: 1);
            View.Property(p => p.Strategy).UseEnumEditor(p => p.XType = "combo").ShowInDetail(columnSpan: 2);
            View.Property(p => p.FromLogicArea).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<LogicArea>();
                return RT.Service.Resolve<WarehouseController>().GetLogicAreas(onShelvesRuleDetail.WarehouseId, r, c);
            }).Readonly(p => p.Strategy != StrategyType.Strategy08 && p.Strategy != StrategyType.Strategy09 && p.Strategy != StrategyType.Strategy10)
            .UseListSetting(e => { e.HelpInfo = "策略为\"根据来源逻辑分区寻找目标逻辑分区的库位上架\"可编辑".L10N(); });
            View.UseDetail(columnCount: 4);
            View.Property(p => p.FromLocation).HasLabel("来源库位".L10N()).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<StorageLocation>();
                return RT.Service.Resolve<WarehouseController>().GetTemporaryLocations(onShelvesRuleDetail.WarehouseId, r, c);
            }).Readonly(p => !(p.Strategy == StrategyType.Strategy01 || p.Strategy == StrategyType.Strategy02))
            .UseListSetting(e => { e.HelpInfo = "策略为\"根据来源库位上架到目标库位\"或\"根据来源库位寻找目标库区的库位上架\"可编辑".L10N(); });

            View.Property(p => p.ToArea).HasLabel("目标库区".L10N()).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<StorageArea>();
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageAreas(onShelvesRuleDetail.WarehouseId, r, c);
            }).Readonly(p => !(p.Strategy == StrategyType.Strategy02 || p.Strategy == StrategyType.Strategy03))
            .UseListSetting(e => { e.HelpInfo = "显示当前仓库下的库位,策略为\"根据来源库位寻找目标库区的库位上架\"或\"直接寻找目标库区的库位上架\"可编辑".L10N(); });

            View.Property(p => p.ToLocation).HasLabel("目标库位".L10N()).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<StorageLocation>();
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(onShelvesRuleDetail.WarehouseId, r, c);
            }).Readonly(p => !(p.Strategy == StrategyType.Strategy01 || p.Strategy == StrategyType.Strategy04))
            .UseListSetting(e => { e.HelpInfo = "策略为\"根据来源库位上架到目标库位\"或\"直接上架到目标库位\"可编辑".L10N(); });


            View.Property(p => p.ToLogicArea).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<LogicArea>();
                return RT.Service.Resolve<WarehouseController>().GetLogicAreas(onShelvesRuleDetail.WarehouseId, r, c);
            }).Readonly(p => p.Strategy != StrategyType.Strategy07 && p.Strategy != StrategyType.Strategy08)
            .UseListSetting(e => { e.HelpInfo = "策略为\"直接寻找目标逻辑分区的库位上架\"可编辑".L10N(); });
            View.Property(p => p.FromStation).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<LogicArea>();
                return RT.Service.Resolve<StationController>().GetStrategyStations(onShelvesRuleDetail.WarehouseId, r, c, true);
            }).Readonly(p => p.SceneType != SceneType.ASRS && p.Strategy != StrategyType.Strategy03 &&
            p.Strategy != StrategyType.Strategy07 && p.Strategy != StrategyType.Strategy09 && p.Strategy != StrategyType.Strategy10);

            View.Property(p => p.ToStation).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<LogicArea>();
                return RT.Service.Resolve<StationController>().GetStrategyStations(onShelvesRuleDetail.WarehouseId, r, c, false);
            }).Readonly(p => p.Strategy != StrategyType.Strategy09)
            .UseListSetting(e => { e.HelpInfo = "策略为\"直接上架到目标站台\"可编辑".L10N(); });

            View.Property(p => p.ToStationGroup).UseDataSource((e, c, r) =>
            {
                var onShelvesRuleDetail = e as OnShelvesRuleDetail;
                if (onShelvesRuleDetail == null)
                    return new EntityList<LogicArea>();
                return RT.Service.Resolve<StationController>().GetStationGroups(onShelvesRuleDetail.WarehouseId, StationGroupType.In, r, c);
            }).Readonly(p => p.Strategy != StrategyType.Strategy10)
            .UseListSetting(e => { e.HelpInfo = "策略为\"直接上架到目标站台组\"可编辑".L10N(); });

            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, SourceLimitationDetailView).HasLabel("来源限制".L10N()).Show(ChildShowInWhere.All).OrderNo = 0;
            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, StayBatchLimitationDetailView).HasLabel("待上架批次属性限制".L10N()).Show(ChildShowInWhere.All).OrderNo = 1;
            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, InventoryLimitationDetailView).HasLabel("目标库位库存限制".L10N()).Show(ChildShowInWhere.All).OrderNo = 2;
            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, InventoryBathLimitationDetailView).HasLabel("目标库位库存批次属性限制".L10N()).Show(ChildShowInWhere.All).OrderNo = 3;
            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, InventorySpaceLimitationDetailView).HasLabel("目标库位空间限制".L10N()).Show(ChildShowInWhere.All).OrderNo = 4;
            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, InventoryOperationLimitationDetailView).HasLabel("目标库位操作限制").Show(ChildShowInWhere.All).OrderNo = 5;
            View.AttachDetailChildrenProperty(typeof(OnShelvesRuleDetail), (c) =>
            {
                return getData(c);
            }, InventoryStorageLimitationDetailView).HasLabel("目标库位存储限制".L10N()).Show(ChildShowInWhere.All).OrderNo = 6;
        }

    }
}
