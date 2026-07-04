using SIE.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.LES.MaterialMoves.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Warehouses;
using SIE.MetaModel;

namespace SIE.LES.MaterialMoves
{
    /// <summary>
    /// 工单挪料记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialMoveRecordCriteria))]
    [Label("工单挪料记录")]
    public class MaterialMoveRecord : DataEntity
    {
        /// <summary>
        /// 工单挪料记录原因快码
        /// </summary>
        public const string MaterialMoveReasonStr = "MATERIAL_MOVE_REASON";

        #region 挪料类型 MoveType
        /// <summary>
        /// 挪料类型
        /// </summary>
        [Label("挪料类型")]
        public static readonly Property<MoveType> MoveTypeProperty = P<MaterialMoveRecord>.Register(e => e.MoveType);

        /// <summary>
        /// 挪料类型
        /// </summary>
        public MoveType MoveType
        {
            get { return this.GetProperty(MoveTypeProperty); }
            set { this.SetProperty(MoveTypeProperty, value); }
        }
        #endregion

        #region 挪料工单 SourceWo
        /// <summary>
        /// 挪料工单Id
        /// </summary>
        [Label("挪料工单")]
        public static readonly IRefIdProperty SourceWoIdProperty =
            P<MaterialMoveRecord>.RegisterRefId(e => e.SourceWoId, ReferenceType.Normal);

        /// <summary>
        /// 挪料工单Id
        /// </summary>
        public double? SourceWoId
        {
            get { return (double?)this.GetRefNullableId(SourceWoIdProperty); }
            set { this.SetRefNullableId(SourceWoIdProperty, value); }
        }

        /// <summary>
        /// 挪料工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> SourceWoProperty =
            P<MaterialMoveRecord>.RegisterRef(e => e.SourceWo, SourceWoIdProperty);

        /// <summary>
        /// 挪料工单
        /// </summary>
        public WorkOrder SourceWo
        {
            get { return this.GetRefEntity(SourceWoProperty); }
            set { this.SetRefEntity(SourceWoProperty, value); }
        }
        #endregion

        #region 目标工单 TargetWo
        /// <summary>
        /// 目标工单Id
        /// </summary>
        [Label("目标工单")]
        public static readonly IRefIdProperty TargetWoIdProperty =
            P<MaterialMoveRecord>.RegisterRefId(e => e.TargetWoId, ReferenceType.Normal);

        /// <summary>
        /// 目标工单Id
        /// </summary>
        public double? TargetWoId
        {
            get { return (double?)this.GetRefNullableId(TargetWoIdProperty); }
            set { this.SetRefNullableId(TargetWoIdProperty, value); }
        }

        /// <summary>
        /// 目标工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> TargetWoProperty =
            P<MaterialMoveRecord>.RegisterRef(e => e.TargetWo, TargetWoIdProperty);

        /// <summary>
        /// 目标工单
        /// </summary>
        public WorkOrder TargetWo
        {
            get { return this.GetRefEntity(TargetWoProperty); }
            set { this.SetRefEntity(TargetWoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<MaterialMoveRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<MaterialMoveRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<MaterialMoveRecord>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialMoveRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料扩展属性Id ItemExtProp
        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialMoveRecord>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialMoveRecord>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 挪料原因 Reason
        /// <summary>
        /// 挪料原因
        /// </summary>
        [Label("挪料原因")]
        public static readonly Property<string> ReasonProperty = P<MaterialMoveRecord>.Register(e => e.Reason);

        /// <summary>
        /// 挪料原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 挪料数量 MoveQty
        /// <summary>
        /// 挪料数量
        /// </summary>
        [Label("挪料数量")]
        public static readonly Property<decimal> MoveQtyProperty = P<MaterialMoveRecord>.Register(e => e.MoveQty);

        /// <summary>
        /// 挪料数量
        /// </summary>
        public decimal MoveQty
        {
            get { return this.GetProperty(MoveQtyProperty); }
            set { this.SetProperty(MoveQtyProperty, value); }
        }
        #endregion

        #region 挪料仓库 Warehouse
        /// <summary>
        /// 挪料仓库Id
        /// </summary>
        [Label("挪料仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialMoveRecord>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 挪料仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 挪料仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<MaterialMoveRecord>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 挪料仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 来源类型 MoveSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<MoveSourceType> MoveSourceTypeProperty = P<MaterialMoveRecord>.Register(e => e.MoveSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public MoveSourceType MoveSourceType
        {
            get { return this.GetProperty(MoveSourceTypeProperty); }
            set { this.SetProperty(MoveSourceTypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class MaterialMoveRecordConfig : EntityConfig<MaterialMoveRecord>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_MOVE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
