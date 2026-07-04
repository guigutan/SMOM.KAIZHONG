using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("供应商信息")]
    public class SupplierInfo : DataEntity
    {
        #region 设备出库单 OutDepot
        /// <summary>
        /// 设备出库单Id
        /// </summary>
        [Label("设备出库单")]
        public static readonly IRefIdProperty OutDepotIdProperty =
            P<SupplierInfo>.RegisterRefId(e => e.OutDepotId, ReferenceType.Parent);

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
            P<SupplierInfo>.RegisterRef(e => e.OutDepot, OutDepotIdProperty);

        /// <summary>
        /// 设备出库单
        /// </summary>
        public OutDepot OutDepot
        {
            get { return this.GetRefEntity(OutDepotProperty); }
            set { this.SetRefEntity(OutDepotProperty, value); }
        }
        #endregion

        #region 供应商编码 Supplier
        /// <summary>
        /// 供应商编码Id
        /// </summary>
        [Label("供应商编码")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<SupplierInfo>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商编码Id
        /// </summary>
        public double SupplierId
        {
            get { return (double)this.GetRefId(SupplierIdProperty); }
            set { this.SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<SupplierInfo>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        [Required]
        public static readonly Property<string> SupplierNameProperty = P<SupplierInfo>.Register(e => e.SupplierName);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { this.SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 联系人 Contact
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        [Required]
        public static readonly Property<string> ContactProperty = P<SupplierInfo>.Register(e => e.Contact);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact
        {
            get { return this.GetProperty(ContactProperty); }
            set { this.SetProperty(ContactProperty, value); }
        }
        #endregion

        #region 联系电话 ContactPhone
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        [Required]
        public static readonly Property<string> ContactPhoneProperty = P<SupplierInfo>.Register(e => e.ContactPhone);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone
        {
            get { return this.GetProperty(ContactPhoneProperty); }
            set { this.SetProperty(ContactPhoneProperty, value); }
        }
        #endregion

        #region 联系地址 ContactAddress
        /// <summary>
        /// 联系地址
        /// </summary>
        [Label("联系地址")]
        [Required]
        public static readonly Property<string> ContactAddressProperty = P<SupplierInfo>.Register(e => e.ContactAddress);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress
        {
            get { return this.GetProperty(ContactAddressProperty); }
            set { this.SetProperty(ContactAddressProperty, value); }
        }
        #endregion

        #region 发货方式 DeliveryWay
        /// <summary>
        /// 发货方式
        /// </summary>
        [Label("发货方式")]
        [Required]
        public static readonly Property<DeliveryWay> DeliveryWayProperty = P<SupplierInfo>.Register(e => e.DeliveryWay);

        /// <summary>
        /// 发货方式
        /// </summary>
        public DeliveryWay DeliveryWay
        {
            get { return this.GetProperty(DeliveryWayProperty); }
            set { this.SetProperty(DeliveryWayProperty, value); }
        }
        #endregion

        #region 单号 OderNo
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> OderNoProperty = P<SupplierInfo>.Register(e => e.OderNo);

        /// <summary>
        /// 单号
        /// </summary>
        public string OderNo
        {
            get { return this.GetProperty(OderNoProperty); }
            set { this.SetProperty(OderNoProperty, value); }
        }
        #endregion

        #region 预计返还日期 DepotRetDate
        /// <summary>
        /// 预计设备返还日期
        /// </summary>
        [Label("预计设备返还日期")]
        public static readonly Property<DateTime> DepotRetDateProperty = P<SupplierInfo>.Register(e => e.DepotRetDate);

        /// <summary>
        /// 预计设备返还日期
        /// </summary>
        public DateTime DepotRetDate
        {
            get { return this.GetProperty(DepotRetDateProperty); }
            set { this.SetProperty(DepotRetDateProperty, value); }
        }
        #endregion

        #region 出库状态 OutState
        /// <summary>
        /// 出库状态
        /// </summary>
        [Label("出库状态")]
        public static readonly Property<OutDepotState> OutStateProperty = P<SupplierInfo>.RegisterView(e => e.OutState, p => p.OutDepot.OutDepotState);

        /// <summary>
        /// 出库状态
        /// </summary>
        public OutDepotState OutState
        {
            get { return this.GetProperty(OutStateProperty); }
        }
        #endregion

        #region 是否是申请单推送 IsAppComeHere
        /// <summary>
        /// 是否是申请单推送
        /// </summary>
        [Label("是否是申请单推送")]
        public static readonly Property<YesNo> IsAppComeHereProperty = P<SupplierInfo>.RegisterView(e => e.IsAppComeHere, p => p.OutDepot.IsAppComeHere);

        /// <summary>
        /// 是否是申请单推送
        /// </summary>
        public YesNo IsAppComeHere
        {
            get { return this.GetProperty(IsAppComeHereProperty); }
        }
        #endregion

    }
    /// <summary>
    /// 供应商信息配置
    /// </summary>
    internal class SupplierInfoConfig : EntityConfig<SupplierInfo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SUPPLIER_INFO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
