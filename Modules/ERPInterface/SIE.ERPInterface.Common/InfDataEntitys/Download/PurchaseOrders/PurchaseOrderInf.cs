using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 采购订单中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("采购订单中间表")]
    public partial class PurchaseOrderInf : DownloadBaseEntity
    {
        #region 采购订单号 No
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> NoProperty = P<PurchaseOrderInf>.Register(e => e.No);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<PurchaseOrderInf>.Register(e => e.SupplierCode);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return GetProperty(SupplierCodeProperty); }
            set { SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 供应商ERP主键 SupplierErpKey
        /// <summary>
        /// 供应商ERP主键
        /// </summary>
        [Label("供应商ERP主键")]
        public static readonly Property<string> SupplierErpKeyProperty = P<PurchaseOrderInf>.Register(e => e.SupplierErpKey);

        /// <summary>
        /// 供应商ERP主键
        /// </summary>
        public string SupplierErpKey
        {
            get { return this.GetProperty(SupplierErpKeyProperty); }
            set { this.SetProperty(SupplierErpKeyProperty, value); }
        }
        #endregion

        #region 供应商地址主键 SupplierAdrssErpKey
        /// <summary>
        /// 供应商地址主键
        /// </summary>
        [Label("供应商地址主键")]
        public static readonly Property<string> SupplierAdrssErpKeyProperty = P<PurchaseOrderInf>.Register(e => e.SupplierAdrssErpKey);

        /// <summary>
        /// 供应商地址主键
        /// </summary>
        public string SupplierAdrssErpKey
        {
            get { return GetProperty(SupplierAdrssErpKeyProperty); }
            set { SetProperty(SupplierAdrssErpKeyProperty, value); }
        }
        #endregion

        #region 制单人编码 BillerCode
        /// <summary>
        /// 制单人编码
        /// </summary>
        [Label("制单人编码")]
        public static readonly Property<string> BillerCodeProperty = P<PurchaseOrderInf>.Register(e => e.BillerCode);

        /// <summary>
        /// 制单人编码
        /// </summary>
        public string BillerCode
        {
            get { return GetProperty(BillerCodeProperty); }
            set { SetProperty(BillerCodeProperty, value); }
        }
        #endregion

        #region 制单日期 BillDate
        /// <summary>
        /// 制单日期
        /// </summary>
        [Label("制单日期")]
        public static readonly Property<DateTime> BillDateProperty = P<PurchaseOrderInf>.Register(e => e.BillDate);

        /// <summary>
        /// 制单日期
        /// </summary>
        public DateTime BillDate
        {
            get { return GetProperty(BillDateProperty); }
            set { SetProperty(BillDateProperty, value); }
        }
        #endregion

        #region 审核日期 AuditDate
        /// <summary>
        /// 审核日期
        /// </summary>
        [Label("审核日期")]
        public static readonly Property<DateTime?> AuditDateProperty = P<PurchaseOrderInf>.Register(e => e.AuditDate);

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? AuditDate
        {
            get { return GetProperty(AuditDateProperty); }
            set { SetProperty(AuditDateProperty, value); }
        }
        #endregion

        #region 审核标记 AuditFlag
        /// <summary>
        /// 审核标记
        /// </summary>
        [Label("审核标记")]
        public static readonly Property<string> AuditFlagProperty = P<PurchaseOrderInf>.Register(e => e.AuditFlag);

        /// <summary>
        /// 审核标记
        /// </summary>
        public string AuditFlag
        {
            get { return GetProperty(AuditFlagProperty); }
            set { SetProperty(AuditFlagProperty, value); }
        }
        #endregion

        #region 审核人编码 AuditorCode
        /// <summary>
        /// 审核人编码
        /// </summary>
        [Label("审核人编码")]
        public static readonly Property<string> AuditorCodeProperty = P<PurchaseOrderInf>.Register(e => e.AuditorCode);

        /// <summary>
        /// 审核人编码
        /// </summary>
        public string AuditorCode
        {
            get { return GetProperty(AuditorCodeProperty); }
            set { SetProperty(AuditorCodeProperty, value); }
        }
        #endregion

        #region 收货仓库编码 ReceivingWhCode
        /// <summary>
        /// 收货仓库编码
        /// </summary>
        [Label("收货仓库编码")]
        public static readonly Property<string> ReceivingWhCodeProperty = P<PurchaseOrderInf>.Register(e => e.ReceivingWhCode);

        /// <summary>
        /// 收货仓库编码
        /// </summary>
        public string ReceivingWhCode
        {
            get { return GetProperty(ReceivingWhCodeProperty); }
            set { SetProperty(ReceivingWhCodeProperty, value); }
        }
        #endregion

        //#region 订单状态 State
        ///// <summary>
        ///// 订单状态
        ///// </summary>
        //[Label("订单状态")]
        //public static readonly Property<PO.PurchaseOrders.State> PoStateProperty = P<PurchaseOrderInf>.Register(e => e.PoState);

        ///// <summary>
        ///// 订单状态
        ///// </summary>
        //public PO.PurchaseOrders.State PoState
        //{
        //    get { return GetProperty(PoStateProperty); }
        //    set { SetProperty(PoStateProperty, value); }
        //}
        //#endregion

        #region 关闭日期 ClosedDate
        /// <summary>
        /// 关闭日期
        /// </summary>
        [Label("关闭日期")]
        public static readonly Property<DateTime?> ClosedDateProperty = P<PurchaseOrderInf>.Register(e => e.ClosedDate);

        /// <summary>
        /// 关闭日期
        /// </summary>
        public DateTime? ClosedDate
        {
            get { return GetProperty(ClosedDateProperty); }
            set { SetProperty(ClosedDateProperty, value); }
        }
        #endregion

        #region 关闭标记 ClosedFlag
        /// <summary>
        /// 关闭标记
        /// </summary>
        [Label("关闭标记")]
        public static readonly Property<string> ClosedFlagProperty = P<PurchaseOrderInf>.Register(e => e.ClosedFlag);

        /// <summary>
        /// 关闭标记
        /// </summary>
        public string ClosedFlag
        {
            get { return GetProperty(ClosedFlagProperty); }
            set { SetProperty(ClosedFlagProperty, value); }
        }
        #endregion

        #region 取消日期 CancelDate
        /// <summary>
        /// 取消日期
        /// </summary>
        [Label("取消日期")]
        public static readonly Property<DateTime?> CancelDateProperty = P<PurchaseOrderInf>.Register(e => e.CancelDate);

        /// <summary>
        /// 取消日期
        /// </summary>
        public DateTime? CancelDate
        {
            get { return GetProperty(CancelDateProperty); }
            set { SetProperty(CancelDateProperty, value); }
        }
        #endregion

        #region 取消标记 CancelFlag
        /// <summary>
        /// 取消标记
        /// </summary>
        [Label("取消标记")]
        public static readonly Property<string> CancelFlagProperty = P<PurchaseOrderInf>.Register(e => e.CancelFlag);

        /// <summary>
        /// 取消标记
        /// </summary>
        public string CancelFlag
        {
            get { return GetProperty(CancelFlagProperty); }
            set { SetProperty(CancelFlagProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<PurchaseOrderInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 更新人编码 UpdateByCode
        /// <summary>
        /// 更新人编码
        /// </summary>
        [Label("更新人编码")]
        public static readonly Property<string> UpdateByCodeProperty = P<PurchaseOrderInf>.Register(e => e.UpdateByCode);

        /// <summary>
        /// 更新人编码
        /// </summary>
        public string UpdateByCode
        {
            get { return GetProperty(UpdateByCodeProperty); }
            set { SetProperty(UpdateByCodeProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<PurchaseOrderInf>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return this.GetProperty(ContactsProperty); }
            set { this.SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 联系电话 ContactNumber
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactNumberProperty = P<PurchaseOrderInf>.Register(e => e.ContactNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return this.GetProperty(ContactNumberProperty); }
            set { this.SetProperty(ContactNumberProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<int> OrderTypeProperty = P<PurchaseOrderInf>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 采购订单中间表 实体配置
    /// </summary>
    internal class PurchaseOrderInfConfig : EntityConfig<PurchaseOrderInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PO").MapAllProperties();
            Meta.Property(PurchaseOrderInf.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}