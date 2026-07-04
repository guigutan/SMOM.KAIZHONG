using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试备件申请
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安装调试备件申请")]
    public partial class SetupSparePartApply : DataEntity
    {
        #region 备件申请 EquipmentSetup
        /// <summary>
        /// 备件申请Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<SetupSparePartApply>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 备件申请Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 备件申请
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<SetupSparePartApply>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 备件申请
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 申请数量 ApplyQty
        /// <summary>
        /// 申请数量
        /// </summary>
        [Label("申请数量")]
        [MinValue(0)]
        public static readonly Property<decimal> ApplyQtyProperty = P<SetupSparePartApply>.Register(e => e.ApplyQty);

        /// <summary>
        /// 申请数量
        /// </summary>
        public decimal ApplyQty
        {
            get { return GetProperty(ApplyQtyProperty); }
            set { SetProperty(ApplyQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SetupSparePartApply>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 需求时间 RequireDateTime
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime?> RequireDateTimeProperty = P<SetupSparePartApply>.Register(e => e.RequireDateTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? RequireDateTime
        {
            get { return GetProperty(RequireDateTimeProperty); }
            set { SetProperty(RequireDateTimeProperty, value); }
        }
        #endregion

        #region 出库仓库 Warehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<SetupSparePartApply>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 出库仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 出库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<SetupSparePartApply>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<SetupSparePartApply>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<SetupSparePartApply>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SetupSparePartApply>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SetupSparePartApply>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<SetupSparePartApply>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 备件类型 PartType
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> PartTypeProperty = P<SetupSparePartApply>.RegisterView(e => e.PartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 备件类型
        /// </summary>
        public SparePartType PartType
        {
            get { return this.GetProperty(PartTypeProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<SetupSparePartApply>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<SetupSparePartApply>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 发料数量(不映射数据库) IssueQty
        /// <summary>
        /// 发料数量
        /// </summary>
        [Label("发料数量")]
        public static readonly Property<decimal?> IssueQtyProperty = P<SetupSparePartApply>.Register(e => e.IssueQty);

        /// <summary>
        /// 发料数量
        /// </summary>
        public decimal? IssueQty
        {
            get { return this.GetProperty(IssueQtyProperty); }
            set { this.SetProperty(IssueQtyProperty, value); }
        }
        #endregion

        #region 消耗数量(不映射数据库) ConsumeQty
        /// <summary>
        /// 消耗数量
        /// </summary>
        [Label("消耗数量")]
        public static readonly Property<decimal?> ConsumeQtyProperty = P<SetupSparePartApply>.Register(e => e.ConsumeQty);

        /// <summary>
        /// 消耗数量
        /// </summary>
        public decimal? ConsumeQty
        {
            get { return this.GetProperty(ConsumeQtyProperty); }
            set { this.SetProperty(ConsumeQtyProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SetupSparePartApply>.RegisterView(e => e.ApprovalStatus, p => p.EquipmentSetup.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试备件申请 实体配置
    /// </summary>
    internal class SetupSparePartApplyConfig : EntityConfig<SetupSparePartApply>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_SP_APL").MapAllProperties();
            Meta.Property(SetupSparePartApply.IssueQtyProperty).DontMapColumn();
            Meta.Property(SetupSparePartApply.ConsumeQtyProperty).DontMapColumn();
            Meta.Property(SetupSparePartApply.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}