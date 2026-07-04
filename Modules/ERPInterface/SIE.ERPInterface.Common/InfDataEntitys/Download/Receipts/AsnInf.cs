using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 接收单中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("接收单中间表")]
    public partial class AsnInf : DownloadBaseEntity
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<AsnInf>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 制单人编码 BillerCode
        /// <summary>
        /// 制单人编码
        /// </summary>
        [Label("制单人编码")]
        public static readonly Property<string> BillerCodeProperty = P<AsnInf>.Register(e => e.BillerCode);

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
        public static readonly Property<DateTime> BillDateProperty = P<AsnInf>.Register(e => e.BillDate);

        /// <summary>
        /// 制单日期
        /// </summary>
        public DateTime BillDate
        {
            get { return GetProperty(BillDateProperty); }
            set { SetProperty(BillDateProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<AsnInf>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 生产部门编码 EnterpriseCode
        /// <summary>
        /// 生产部门编码
        /// </summary>
        [Label("生产部门编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<AsnInf>.Register(e => e.EnterpriseCode);

        /// <summary>
        /// 生产部门编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return GetProperty(EnterpriseCodeProperty); }
            set { SetProperty(EnterpriseCodeProperty, value); }
        }
        #endregion

        #region 收货仓库编码 WarehouseCode
        /// <summary>
        /// 收货仓库编码
        /// </summary>
        [Label("收货仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<AsnInf>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 收货仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return GetProperty(WarehouseCodeProperty); }
            set { SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 取消标记 CancelFlag
        /// <summary>
        /// 取消标记
        /// </summary>
        [Label("取消标记")]
        public static readonly Property<string> CancelFlagProperty = P<AsnInf>.Register(e => e.CancelFlag);

        /// <summary>
        /// 取消标记
        /// </summary>
        public string CancelFlag
        {
            get { return GetProperty(CancelFlagProperty); }
            set { SetProperty(CancelFlagProperty, value); }
        }
        #endregion

        #region 取消日期 CancelDate
        /// <summary>
        /// 取消日期
        /// </summary>
        [Label("取消日期")]
        public static readonly Property<string> CancelDateProperty = P<AsnInf>.Register(e => e.CancelDate);

        /// <summary>
        /// 取消日期
        /// </summary>
        public string CancelDate
        {
            get { return GetProperty(CancelDateProperty); }
            set { SetProperty(CancelDateProperty, value); }
        }
        #endregion

        //#region 单据状态 AsnState
        ///// <summary>
        ///// 单据状态
        ///// </summary>
        //[Label("单据状态")]
        //public static readonly Property<AsnState> AsnStateProperty = P<AsnInf>.Register(e => e.AsnState);

        ///// <summary>
        ///// 单据状态
        ///// </summary>
        //public AsnState AsnState
        //{
        //    get { return GetProperty(AsnStateProperty); }
        //    set { SetProperty(AsnStateProperty, value); }
        //}
        //#endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<int> OrderTypeProperty = P<AsnInf>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public int OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(1000)]
        public static readonly Property<string> RemarkProperty = P<AsnInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<AsnInf>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<AsnInf>.Register(e => e.SupplierCode);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
            set { this.SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<AsnInf>.Register(e => e.Contacts);

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
        public static readonly Property<string> ContactNumberProperty = P<AsnInf>.Register(e => e.ContactNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return this.GetProperty(ContactNumberProperty); }
            set { this.SetProperty(ContactNumberProperty, value); }
        }
        #endregion

        #region 交接人 Connecter
        /// <summary>
        /// 交接人
        /// </summary>
        [Label("交接人")]
        public static readonly Property<string> ConnecterProperty = P<AsnInf>.Register(e => e.Connecter);

        /// <summary>
        /// 交接人
        /// </summary>
        public string Connecter
        {
            get { return this.GetProperty(ConnecterProperty); }
            set { this.SetProperty(ConnecterProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 接收单中间表 实体配置
    /// </summary>
    internal class AsnInfConfig : EntityConfig<AsnInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_ASN").MapAllProperties();
            Meta.Property(AsnInf.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}