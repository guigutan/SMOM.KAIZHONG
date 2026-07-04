using SIE.Core.Boxs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台组
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StationGroupCriteria))]
    [Label("站台组")]
    [DisplayMember(nameof(Code))]
    public partial class StationGroup : DataEntity, IStateEntity
    {
        #region 站台组代码 Code
        /// <summary>
        /// 站台组代码,格式：StationGroup:编号_库区_仓库
        /// </summary>       
        [Label("编码")]
        [Required, NotDuplicate]
        public static readonly Property<string> CodeProperty = P<StationGroup>.Register(e => e.Code);

        /// <summary>
        /// 站台组代码,格式：StationGroup:编号_库区_仓库
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [MaxLength(80)]
        [Required, NotDuplicate]
        public static readonly Property<string> NameProperty = P<StationGroup>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 站台组类型 StationGroupType
        /// <summary>
        /// 站台组类型
        /// </summary>
        [Label("站台组类型")]
        [Required]
        public static readonly Property<StationGroupType> StationGroupTypeProperty = P<StationGroup>.Register(e => e.StationGroupType);

        /// <summary>
        /// 名称
        /// </summary>
        public StationGroupType StationGroupType
        {
            get { return this.GetProperty(StationGroupTypeProperty); }
            set { this.SetProperty(StationGroupTypeProperty, value); }
        }
        #endregion

        #region 托盘型号 TurnoverBoxModel
        /// <summary>
        /// 托盘型号Id
        /// </summary>
        [Label("托盘型号")]
        public static readonly IRefIdProperty TurnoverBoxModelIdProperty = P<StationGroup>.RegisterRefId(e => e.TurnoverBoxModelId, ReferenceType.Normal);

        /// <summary>
        /// 托盘型号Id
        /// </summary>
        public double TurnoverBoxModelId
        {
            get { return (double)GetRefId(TurnoverBoxModelIdProperty); }
            set { SetRefId(TurnoverBoxModelIdProperty, value); }
        }

        /// <summary>
        /// 托盘型号
        /// </summary>
        public static readonly RefEntityProperty<TurnoverBoxModel> TurnoverBoxModelProperty = P<StationGroup>.RegisterRef(e => e.TurnoverBoxModel, TurnoverBoxModelIdProperty);

        /// <summary>
        /// 托盘型号
        /// </summary>
        public TurnoverBoxModel TurnoverBoxModel
        {
            get { return GetRefEntity(TurnoverBoxModelProperty); }
            set { SetRefEntity(TurnoverBoxModelProperty, value); }
        }
        #endregion

        #region 托盘型号编码 TurnoverBoxModelCode
        /// <summary>
        /// 托盘型号编码
        /// </summary>
        [Label("托盘型号编码")]
        public static readonly Property<string> TurnoverBoxModelCodeProperty = P<StationGroup>.RegisterView(e => e.TurnoverBoxModelCode, p => p.TurnoverBoxModel.Code);

        /// <summary>
        /// 托盘型号编码
        /// </summary>
        public string TurnoverBoxModelCode
        {
            get { return this.GetProperty(TurnoverBoxModelCodeProperty); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<StationGroup>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StationGroup>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StationGroup>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StationGroup>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
            set { SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StationGroup>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 站台组所在方案中位置 Location
        /// <summary>
        /// 站台组所在方案中位置
        /// </summary>
        [MaxLength(64)]
        [Label("所在方案中位置")]
        public static readonly Property<string> LocationProperty = P<StationGroup>.Register(e => e.Location);

        /// <summary>
        /// 站台组所在方案中位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 明细列表 StationGroupLineList
        /// <summary>
        /// 站台组明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<StationGroupLine>> StationGroupLineListProperty = P<StationGroup>.RegisterList(e => e.StationGroupLineList);
        /// <summary>
        /// 站台组明细列表
        /// </summary>
        public EntityList<StationGroupLine> StationGroupLineList
        {
            get { return this.GetLazyList(StationGroupLineListProperty); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<StationGroup>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 站台组 实体配置
    /// </summary>
    internal class StationGroupConfig : EntityConfig<StationGroup>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    StationGroup.StationGroupTypeProperty,
                    StationGroup.TurnoverBoxModelIdProperty,
                    StationGroup.WarehouseIdProperty,
                    StationGroup.StorageAreaIdProperty,
                },
                MessageBuilder = o =>
                {
                    return "已经存在重复的站台组类型、托盘型号、仓库、库区的数据".L10N();
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WCS_STATION_GROUP").MapAllProperties();          
            Meta.Property(StationGroup.CodeProperty).ColumnMeta.HasLength(240);
            Meta.Property(StationGroup.LocationProperty).ColumnMeta.HasLength(128);
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}