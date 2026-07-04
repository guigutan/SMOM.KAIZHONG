using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试备件使用
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安装调试备件使用")]
    public partial class SetupSparePart : DataEntity
    {
        #region 备件使用 EquipmentSetup
        /// <summary>
        /// 备件使用Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<SetupSparePart>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 备件使用Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 备件使用
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<SetupSparePart>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 备件使用
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 使用数量 UseQty
        /// <summary>
        /// 使用数量
        /// </summary>
        [Label("使用数量")]
        [MinValue(1)]
        [Required]
        public static readonly Property<int> UseQtyProperty = P<SetupSparePart>.Register(e => e.UseQty);

        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseQty
        {
            get { return GetProperty(UseQtyProperty); }
            set { SetProperty(UseQtyProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<SetupSparePart>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 批次下拉 LotInfo
        /// <summary>
        /// 批次下拉Id
        /// </summary>
        [Label("批次号")]
        public static readonly IRefIdProperty LotInfoIdProperty =
            P<SetupSparePart>.RegisterRefId(e => e.LotInfoId, ReferenceType.Normal);

        /// <summary>
        /// 批次下拉Id
        /// </summary>
        public string LotInfoId
        {
            get { return (string)this.GetRefId(LotInfoIdProperty); }
            set { this.SetRefId(LotInfoIdProperty, value); }
        }

        /// <summary>
        /// 批次下拉
        /// </summary>
        public static readonly RefEntityProperty<SetupLotSnInfo> LotInfoProperty =
            P<SetupSparePart>.RegisterRef(e => e.LotInfo, LotInfoIdProperty);

        /// <summary>
        /// 批次下拉
        /// </summary>
        public SetupLotSnInfo LotInfo
        {
            get { return this.GetRefEntity(LotInfoProperty); }
            set { this.SetRefEntity(LotInfoProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<SetupSparePart>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 序列号下拉 SnInfo
        /// <summary>
        /// 序列号下拉Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty SnInfoIdProperty =
            P<SetupSparePart>.RegisterRefId(e => e.SnInfoId, ReferenceType.Normal);

        /// <summary>
        /// 序列号下拉Id
        /// </summary>
        public string SnInfoId
        {
            get { return (string)this.GetRefId(SnInfoIdProperty); }
            set { this.SetRefId(SnInfoIdProperty, value); }
        }

        /// <summary>
        /// 序列号下拉
        /// </summary>
        public static readonly RefEntityProperty<SetupLotSnInfo> SnInfoProperty =
            P<SetupSparePart>.RegisterRef(e => e.SnInfo, SnInfoIdProperty);

        /// <summary>
        /// 序列号下拉
        /// </summary>
        public SetupLotSnInfo SnInfo
        {
            get { return this.GetRefEntity(SnInfoProperty); }
            set { this.SetRefEntity(SnInfoProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<SetupSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)GetRefNullableId(SparePartIdProperty); }
            set { SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<SetupSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SetupSparePart>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<SetupSparePart>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<SetupSparePart>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 出库单 PartOutDepotDetail
        /// <summary>
        /// 出库单Id
        /// </summary>
        [Label("出库单")]
        public static readonly IRefIdProperty PartOutDepotDetailIdProperty = P<SetupSparePart>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

        /// <summary>
        /// 出库单Id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return (double?)GetRefNullableId(PartOutDepotDetailIdProperty); }
            set { SetRefNullableId(PartOutDepotDetailIdProperty, value); }
        }

        /// <summary>
        /// 出库单
        /// </summary>
        public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty = P<SetupSparePart>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

        /// <summary>
        /// 出库单
        /// </summary>
        public PartOutDepotDetail PartOutDepotDetail
        {
            get { return GetRefEntity(PartOutDepotDetailProperty); }
            set { SetRefEntity(PartOutDepotDetailProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<SetupSparePart>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<SetupSparePart>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 是否出库单信息 IsOutDepotInfo
        /// <summary>
        /// 是否出库单信息
        /// </summary>
        [Label("是否出库单信息")]
        public static readonly Property<bool> IsOutDepotInfoProperty = P<SetupSparePart>.Register(e => e.IsOutDepotInfo);

        /// <summary>
        /// 是否出库单信息
        /// </summary>
        public bool IsOutDepotInfo
        {
            get { return this.GetProperty(IsOutDepotInfoProperty); }
            set { this.SetProperty(IsOutDepotInfoProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<SetupSparePart>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SetupSparePart>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<SetupSparePart>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<SetupSparePart>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<SetupSparePart>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 出库单明细行号 PartOutDepotDetailLineNo
        /// <summary>
        /// 出库单明细行号
        /// </summary>
        [Label("出库单明细行号")]
        public static readonly Property<int> PartOutDepotDetailLineNoProperty = P<SetupSparePart>.RegisterView(e => e.PartOutDepotDetailLineNo, p => p.PartOutDepotDetail.LineNo);

        /// <summary>
        /// 出库单明细行号
        /// </summary>
        public int PartOutDepotDetailLineNo
        {
            get { return this.GetProperty(PartOutDepotDetailLineNoProperty); }
            set { SetProperty(PartOutDepotDetailLineNoProperty, value); }
        }
        #endregion

        #region 出库单号 OutDepotNo
        /// <summary>
        /// 出库单号
        /// </summary>
        [Label("出库单号")]
        public static readonly Property<string> OutDepotNoProperty = P<SetupSparePart>.RegisterView(e => e.OutDepotNo, p => p.PartOutDepotDetail.OutDepot.No);

        /// <summary>
        /// 出库单号
        /// </summary>
        public string OutDepotNo
        {
            get { return this.GetProperty(OutDepotNoProperty); }
            set { SetProperty(OutDepotNoProperty, value); }
        }
        #endregion

        #region 发料数量 OutDepotCount
        /// <summary>
        /// 发料数量
        /// </summary>
        [Label("发料数量")]
        public static readonly Property<decimal> OutDepotCountProperty = P<SetupSparePart>.RegisterView(e => e.OutDepotCount, p => p.PartOutDepotDetail.OutDepotCount);

        /// <summary>
        /// 发料数量
        /// </summary>
        public decimal OutDepotCount
        {
            get { return this.GetProperty(OutDepotCountProperty); }
            set { SetProperty(OutDepotCountProperty, value); }
        }
        #endregion

        #region 出库明细使用数量 UseCount
        /// <summary>
        /// 出库明细使用数量
        /// </summary>
        [Label("出库明细使用数量")]
        public static readonly Property<decimal> UseCountProperty = P<SetupSparePart>.RegisterView(e => e.UseCount, p => p.PartOutDepotDetail.UseCount);

        /// <summary>
        /// 出库明细使用数量
        /// </summary>
        public decimal UseCount
        {
            get { return this.GetProperty(UseCountProperty); }
            set { SetProperty(UseCountProperty, value); }
        }
        #endregion

        #region 出库明细退回数量 ReturnQty
        /// <summary>
        /// 出库明细退回数量
        /// </summary>
        [Label("出库明细退回数量")]
        public static readonly Property<decimal> ReturnQtyProperty = P<SetupSparePart>.RegisterView(e => e.ReturnQty, p => p.PartOutDepotDetail.ReturnQty);

        /// <summary>
        /// 出库明细退回数量
        /// </summary>
        public decimal ReturnQty
        {
            get { return this.GetProperty(ReturnQtyProperty); }
            set { SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 剩余数量(不映射数据库) SurplusQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> SurplusQtyProperty = P<SetupSparePart>.Register(e => e.SurplusQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal SurplusQty
        {
            get { return this.GetProperty(SurplusQtyProperty); }
            set { this.SetProperty(SurplusQtyProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试备件使用 实体配置
    /// </summary>
    internal class SetupSparePartConfig : EntityConfig<SetupSparePart>
    {
        ///// <summary>
        ///// 实体验证规则的配置
        ///// </summary>
        ///// <param name="rules"></param>
        //protected override void AddValidations(IValidationDeclarer rules)
        //{
        //    rules.AddRule(SetupSparePart.UseQtyProperty, new NumberRangeRule() { Min = 1 });

        //    base.AddValidations(rules);
        //}

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_SP").MapAllProperties();
            Meta.Property(SetupSparePart.SurplusQtyProperty).DontMapColumn();
            Meta.Property(SetupSparePart.LotInfoIdProperty).DontMapColumn();
            Meta.Property(SetupSparePart.SnInfoIdProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}