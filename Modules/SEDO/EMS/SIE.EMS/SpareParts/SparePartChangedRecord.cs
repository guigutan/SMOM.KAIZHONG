using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 设备台账 备件更换记录
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("更换记录")]
    public class SparePartChangedRecord : DataEntity
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<SparePartChangedRecord>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<SparePartChangedRecord>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion


        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<SparePartChangedRecord>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<SparePartChangedRecord>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion


        #region 批次号 BatchNumber
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNumberProperty = P<SparePartChangedRecord>.Register(e => e.BatchNumber);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber
        {
            get { return this.GetProperty(BatchNumberProperty); }
            set { this.SetProperty(BatchNumberProperty, value); }
        }
        #endregion

        #region 序列号 SerialNumber
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SerialNumberProperty = P<SparePartChangedRecord>.Register(e => e.SerialNumber);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber
        {
            get { return this.GetProperty(SerialNumberProperty); }
            set { this.SetProperty(SerialNumberProperty, value); }
        }
        #endregion

        #region 旧序列号 OldSerialNumber
        /// <summary>
        /// 旧序列号
        /// </summary>
        [Label("旧序列号")]
        public static readonly Property<string> OldSerialNumberProperty = P<SparePartChangedRecord>.Register(e => e.OldSerialNumber);

        /// <summary>
        /// 旧序列号
        /// </summary>
        public string OldSerialNumber
        {
            get { return this.GetProperty(OldSerialNumberProperty); }
            set { this.SetProperty(OldSerialNumberProperty, value); }
        }
        #endregion

        #region 来源 Source
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<FromType> SourceProperty = P<SparePartChangedRecord>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public FromType Source
        {
            get { return this.GetProperty(SourceProperty); }
            set { this.SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<SparePartChangedRecord>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 来源单ID SourceId
        /// <summary>
        /// 来源单ID
        /// </summary>
        [Label("来源单ID")]
        public static readonly Property<double?> SourceIdProperty = P<SparePartChangedRecord>.Register(e => e.SourceId);

        /// <summary>
        /// 来源单ID
        /// </summary>
        public double? SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SparePartChangedRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 来源单据已完成 IsSourceCompleted
        /// <summary>
        /// 来源单据已完成
        /// </summary>
        [Label("来源单据已完成")]
        public static readonly Property<bool> IsSourceCompletedProperty = P<SparePartChangedRecord>.Register(e => e.IsSourceCompleted);

        /// <summary>
        /// 来源单据已完成
        /// </summary>
        public bool IsSourceCompleted
        {
            get { return this.GetProperty(IsSourceCompletedProperty); }
            set { this.SetProperty(IsSourceCompletedProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 备件编码 SparePartCodeView
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeViewProperty = P<SparePartChangedRecord>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameViewProperty = P<SparePartChangedRecord>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartNameView
        {
            get { return this.GetProperty(SparePartNameViewProperty); }
        }
        #endregion

        #region 规则型号 SpecificationView
        /// <summary>
        /// 规则型号
        /// </summary>
        [Label("规则型号")]
        public static readonly Property<string> SpecificationViewProperty = P<SparePartChangedRecord>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

        /// <summary>
        /// 规则型号
        /// </summary>
        public string SpecificationView
        {
            get { return this.GetProperty(SpecificationViewProperty); }
        }
        #endregion

        #region 类型 SparePartTypeView
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<string> SparePartTypeViewProperty = P<SparePartChangedRecord>.RegisterView(e => e.SparePartTypeView, p => p.SparePart.ItemCategory.Code);

        /// <summary>
        /// 类型
        /// </summary>
        public string SparePartTypeView
        {
            get { return this.GetProperty(SparePartTypeViewProperty); }
        }
        #endregion

        #region 单位 SparePartUnitView
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> SparePartUnitViewProperty = P<SparePartChangedRecord>.RegisterView(e => e.SparePartUnitView, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string SparePartUnitView
        {
            get { return this.GetProperty(SparePartUnitViewProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 备件更换记录 实体配置
    /// </summary>
    internal class EquipAccountSpPartChangedRecConfig : EntityConfig<SparePartChangedRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_PART_CHG_REC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
