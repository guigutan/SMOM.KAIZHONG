using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修备件更换
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备维修备件更换")]
    public partial class EquipRepairSparePartChg : DataEntity
    {
        #region 设备维修单 EquipRepairBill
        /// <summary>
        /// 设备维修单Id
        /// </summary>
        [Label("设备维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty =
            P<EquipRepairSparePartChg>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

        /// <summary>
        /// 设备维修单Id
        /// </summary>
        public double EquipRepairBillId
        {
            get { return (double)this.GetRefId(EquipRepairBillIdProperty); }
            set { this.SetRefId(EquipRepairBillIdProperty, value); }
        }

        /// <summary>
        /// 设备维修单
        /// </summary>
        public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty =
            P<EquipRepairSparePartChg>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill EquipRepairBill
        {
            get { return this.GetRefEntity(EquipRepairBillProperty); }
            set { this.SetRefEntity(EquipRepairBillProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<EquipRepairSparePartChg>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<EquipRepairSparePartChg>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ChangeSparePartState> StateProperty = P<EquipRepairSparePartChg>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ChangeSparePartState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 备件出库单明细 PartOutDepotDetail
        /// <summary>
        /// 备件出库单明细Id
        /// </summary>
        [Label("备件出库单")]
        public static readonly IRefIdProperty PartOutDepotDetailIdProperty =
            P<EquipRepairSparePartChg>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件出库单明细Id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return (double?)this.GetRefNullableId(PartOutDepotDetailIdProperty); }
            set { this.SetRefNullableId(PartOutDepotDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件出库单明细
        /// </summary>
        public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty =
            P<EquipRepairSparePartChg>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

        /// <summary>
        /// 备件出库单明细
        /// </summary>
        public PartOutDepotDetail PartOutDepotDetail
        {
            get { return this.GetRefEntity(PartOutDepotDetailProperty); }
            set { this.SetRefEntity(PartOutDepotDetailProperty, value); }
        }
        #endregion

        #region 更换数量 ChangeQty
        /// <summary>
        /// 更换数量
        /// </summary>
        [Label("更换数量")]
        public static readonly Property<int> ChangeQtyProperty = P<EquipRepairSparePartChg>.Register(e => e.ChangeQty);

        /// <summary>
        /// 更换数量
        /// </summary>
        public int ChangeQty
        {
            get { return this.GetProperty(ChangeQtyProperty); }
            set { this.SetProperty(ChangeQtyProperty, value); }
        }
        #endregion

        #region 旧序列号 OldSequence
        /// <summary>
        /// 旧序列号
        /// </summary>
        [Label("旧序列号")]
        public static readonly Property<string> OldSequenceProperty = P<EquipRepairSparePartChg>.Register(e => e.OldSequence);

        /// <summary>
        /// 旧序列号
        /// </summary>
        public string OldSequence
        {
            get { return this.GetProperty(OldSequenceProperty); }
            set { this.SetProperty(OldSequenceProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipRepairSparePartChg>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 不映射数据库字段

        #region 剩余数 RemainingQty
        /// <summary>
        /// 剩余数
        /// </summary>
        [Label("剩余数")]
        public static readonly Property<int> RemainingQtyProperty = P<EquipRepairSparePartChg>.Register(e => e.RemainingQty);

        /// <summary>
        /// 剩余数
        /// </summary>
        public int RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
            set { this.SetProperty(RemainingQtyProperty, value); }
        }
        #endregion

       
        #endregion

        #region 视图属性

        #region 备件编码 SparePartCodeView
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeViewProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCodeView
        {
            get { return this.GetProperty(SparePartCodeViewProperty); }
        }
        #endregion

        #region 备件名称 SparePartNameView
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameViewProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartNameView
        {
            get { return this.GetProperty(SparePartNameViewProperty); }
        }
        #endregion

        #region 规格型号 SpecificationView
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationViewProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

        /// <summary>
        /// 是否以旧换新
        /// </summary>
        public string SpecificationView
        {
            get { return this.GetProperty(SpecificationViewProperty); }
        }
        #endregion

        #region 单位 UnitView
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitViewProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.UnitView, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitView
        {
            get { return this.GetProperty(UnitViewProperty); }
        }
        #endregion

        #region 出库单号 OutDepotNoView
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("出库单号")]
        public static readonly Property<string> OutDepotNoViewProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.OutDepotNoView, p => p.PartOutDepotDetail.OutDepot.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string OutDepotNoView
        {
            get { return this.GetProperty(OutDepotNoViewProperty); }
        }
        #endregion

        #region 出库单行号 LineNo
        /// <summary>
        /// 出库单行号
        /// </summary>
        [Label("出库单行号")]
        public static readonly Property<int> LineNoProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.LineNo, p => p.PartOutDepotDetail.LineNo);

        /// <summary>
        /// 出库单行号
        /// </summary>
        public int LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
        }
        #endregion


        #region 序列号 SeriaNo
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SeriaNoProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.SeriaNo, p => p.PartOutDepotDetail.SeriaNo);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNo
        {
            get { return this.GetProperty(SeriaNoProperty); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.BatchNo, p => p.PartOutDepotDetail.BatchNoRef.BatchNumber);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<EquipRepairSparePartChg>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #endregion

        #region 出库单号-行号 OutDepotLineNo
        /// <summary>
        /// 出库单号-行号
        /// </summary>
        [Label("出库单号-行号")]
        public static readonly Property<string> OutDepotLineNoProperty = P<EquipRepairSparePartChg>.RegisterReadOnly(
            e => e.OutDepotLineNo, e => e.GetOutDepotLineNo(), OutDepotNoViewProperty, LineNoProperty);

        /// <summary>
        /// 出库单号-行号
        /// </summary>
        public string OutDepotLineNo
        {
            get { return this.GetProperty(OutDepotLineNoProperty); }
        }
        private string GetOutDepotLineNo()
        {
            return string.Format("{0}-{1}", OutDepotNoView, LineNo);
        }
        #endregion

    }

    /// <summary>
    /// 设备维修备件更换 实体配置
    /// </summary>
    internal class EquipRepairSparePartChgConfig : EntityConfig<EquipRepairSparePartChg>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(EquipRepairSparePartChg.RemarkProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairSparePartChg.ChangeQtyProperty, new PositiveNumberRule());
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR_SP_CHG").MapAllProperties();
            Meta.Property(EquipRepairSparePartChg.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairSparePartChg.RemainingQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}