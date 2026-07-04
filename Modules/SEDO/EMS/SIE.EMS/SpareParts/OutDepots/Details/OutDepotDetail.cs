using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.EquipModels;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 出库申请单明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("出库申请单明细")]
    public class OutDepotDetail : DataEntity
    {
        #region 设备出库单 OutDepot
        /// <summary>
        /// 设备出库单Id
        /// </summary>
        [Label("设备出库单")]
        public static readonly IRefIdProperty OutDepotIdProperty =
            P<OutDepotDetail>.RegisterRefId(e => e.OutDepotId, ReferenceType.Parent);

        /// <summary>
        /// 设备出库单Id
        /// </summary>
        public double OutDepotId
        {
            get { return (double)this.GetRefId(OutDepotIdProperty); }
            set { this.SetRefId(OutDepotIdProperty, value); }
        }

        /// <summary>
        /// 设备出库单
        /// </summary>
        public static readonly RefEntityProperty<OutDepot> OutDepotProperty =
            P<OutDepotDetail>.RegisterRef(e => e.OutDepot, OutDepotIdProperty);

        /// <summary>
        /// 设备出库单
        /// </summary>
        public OutDepot OutDepot
        {
            get { return this.GetRefEntity(OutDepotProperty); }
            set { this.SetRefEntity(OutDepotProperty, value); }
        }
        #endregion

        #region 备件编码 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<OutDepotDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<OutDepotDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件名称 SpareName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SpareNameProperty = P<OutDepotDetail>.Register(e => e.SpareName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SpareName
        {
            get { return this.GetProperty(SpareNameProperty); }
            set { this.SetProperty(SpareNameProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<OutDepotDetail>.Register(e => e.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { this.SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 备件部位 SparePartPart
        /// <summary>
        /// 备件部位
        /// </summary>
        [Label("备件部位")]
        public static readonly Property<string> SparePartPartProperty = P<OutDepotDetail>.Register(e => e.SparePartPart);

        /// <summary>
        /// 备件部位
        /// </summary>
        public string SparePartPart
        {
            get { return this.GetProperty(SparePartPartProperty); }
            set { this.SetProperty(SparePartPartProperty, value); }
        }
        #endregion       

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<OutDepotDetail>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<OutDepotDetail>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 申请数量 RequireCount
        /// <summary>
        /// 申请数量
        /// </summary>
        [Label("申请数量")]
        public static readonly Property<int> RequireCountProperty = P<OutDepotDetail>.Register(e => e.RequireCount);

        /// <summary>
        /// 申请数量
        /// </summary>
        public int RequireCount
        {
            get { return this.GetProperty(RequireCountProperty); }
            set { this.SetProperty(RequireCountProperty, value); }
        }
        #endregion

        #region 拣货数 PickedCount
        /// <summary>
        /// 拣货数
        /// </summary>
        [Label("拣货数")]
        public static readonly Property<int> PickedCountProperty = P<OutDepotDetail>.Register(e => e.PickedCount);

        /// <summary>
        /// 拣货数
        /// </summary>
        public int PickedCount
        {
            get { return this.GetProperty(PickedCountProperty); }
            set { this.SetProperty(PickedCountProperty, value); }
        }
        #endregion

        #region 出库数量 OutDepotCount
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        [Required]
        public static readonly Property<int> OutDepotCountProperty = P<OutDepotDetail>.Register(e => e.OutDepotCount);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutDepotCount
        {
            get { return this.GetProperty(OutDepotCountProperty); }
            set { this.SetProperty(OutDepotCountProperty, value); }
        }
        #endregion

        #region 接收数量 ReceiveQty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<int> ReceiveQtyProperty = P<OutDepotDetail>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public int ReceiveQty
        {
            get { return this.GetProperty(ReceiveQtyProperty); }
            set { this.SetProperty(ReceiveQtyProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<OutDepotDetail>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double)this.GetRefId(UnitIdProperty); }
            set { this.SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<OutDepotDetail>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 库存数 DepotPartCount
        /// <summary>
        /// 库存数
        /// </summary>
        [Label("库存数")]
        public static readonly Property<int> DepotPartCountProperty = P<OutDepotDetail>.Register(e => e.DepotPartCount);

        /// <summary>
        /// 库存数
        /// </summary>
        public int DepotPartCount
        {
            get { return this.GetProperty(DepotPartCountProperty); }
            set { this.SetProperty(DepotPartCountProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<OutDepotDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 规格型号 SpecModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecModelProperty = P<OutDepotDetail>.RegisterView(e => e.SpecModel, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecModel
        {
            get { return this.GetProperty(SpecModelProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<OutDepotDetail>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<OutDepotDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 设备型号 SpartEquipModelName
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> SpartEquipModelNameProperty = P<OutDepotDetail>.RegisterView(e => e.SpartEquipModelName, p => p.SparePart.SpartEquipModel.Name);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string SpartEquipModelName
        {
            get { return this.GetProperty(SpartEquipModelNameProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<OutDepotDetail>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 出库状态 OutState
        /// <summary>
        /// 出库状态
        /// </summary>
        [Label("出库状态")]
        public static readonly Property<OutDepotState> OutStateProperty = P<OutDepotDetail>.RegisterView(e => e.OutState, p => p.OutDepot.OutDepotState);

        /// <summary>
        /// 出库状态
        /// </summary>
        public OutDepotState   OutState
        {
            get { return this.GetProperty(OutStateProperty); }
        }
        #endregion

        #region 是否是申请单推送 IsAppComeHere
        /// <summary>
        /// 是否是申请单推送
        /// </summary>
        [Label("是否是申请单推送")]
        public static readonly Property<YesNo> IsAppComeHereProperty = P<OutDepotDetail>.RegisterView(e => e.IsAppComeHere, p => p.OutDepot.IsAppComeHere);

        /// <summary>
        /// 是否是申请单推送
        /// </summary>
        public YesNo IsAppComeHere
        {
            get { return this.GetProperty(IsAppComeHereProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 备件编码 SparePartCodeView
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeViewProperty = P<OutDepotDetail>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCodeView
        {
            get { return this.GetProperty(SparePartCodeViewProperty); }
            set { this.SetProperty(SparePartCodeViewProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartNameView
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameViewProperty = P<OutDepotDetail>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartNameView
        {
            get { return this.GetProperty(SparePartNameViewProperty); }
            set { this.SetProperty(SparePartNameViewProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethodView
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodViewProperty = P<OutDepotDetail>.RegisterView(e => e.ControlMethodView, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethodView
        {
            get { return this.GetProperty(ControlMethodViewProperty); }
            set { this.SetProperty(ControlMethodViewProperty, value); }

        }
        #endregion

        #region 推荐库位 AdviceStorageLocation
        /// <summary>
        /// 推荐库位
        /// </summary>
        [Label("推荐库位")]
        public static readonly Property<string> AdviceStorageLocationProperty = P<OutDepotDetail>.Register(e => e.AdviceStorageLocation);

        /// <summary>
        /// 推荐库位
        /// </summary>
        public string AdviceStorageLocation
        {
            get { return this.GetProperty(AdviceStorageLocationProperty); }
            set { this.SetProperty(AdviceStorageLocationProperty, value); }
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 申请单明细配置
    /// </summary>
    internal class OutDepotDetailConfig : EntityConfig<OutDepotDetail>
    {

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_OUT_DEPOT_DETAIL").MapAllProperties();
            Meta.Property(OutDepotDetail.AdviceStorageLocationProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
