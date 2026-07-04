using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// ASN单明细中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("ASN单明细中间表")]
    public partial class AsnDetailInf : DownloadBaseEntity
    {
        #region ASN单号 AsnNo
        /// <summary>
        /// ASN单号
        /// </summary>
        [Label("ASN单号")]
        public static readonly Property<string> AsnNoProperty = P<AsnDetailInf>.Register(e => e.AsnNo);

        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo
        {
            get { return GetProperty(AsnNoProperty); }
            set { SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 预期数量 ExpectQty
        /// <summary>
        /// 预期数量
        /// </summary>
        [Label("预期数量")]
        public static readonly Property<decimal> ExpectQtyProperty = P<AsnDetailInf>.Register(e => e.ExpectQty);

        /// <summary>
        /// 预期数量
        /// </summary>
        public decimal ExpectQty
        {
            get { return GetProperty(ExpectQtyProperty); }
            set { SetProperty(ExpectQtyProperty, value); }
        }
        #endregion

        #region 相关单号 OrderNo
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> OrderNoProperty = P<AsnDetailInf>.Register(e => e.OrderNo);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo
        {
            get { return GetProperty(OrderNoProperty); }
            set { SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<AsnDetailInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        //#region 状态 State
        ///// <summary>
        ///// 状态
        ///// </summary>
        //[Label("状态")]
        //public static readonly Property<AsnState> AsnStateProperty = P<AsnDetailInf>.Register(e => e.AsnState);

        ///// <summary>
        ///// 状态
        ///// </summary>
        //public AsnState AsnState
        //{
        //    get { return GetProperty(AsnStateProperty); }
        //    set { SetProperty(AsnStateProperty, value); }
        //}
        //#endregion

        #region 收货库位 ReceiveStorageLocation
        /// <summary>
        /// 收货库位
        /// </summary>
        [Label("收货库位")]
        public static readonly Property<string> ReceiveStorageLocationProperty = P<AsnDetailInf>.Register(e => e.ReceiveStorageLocation);

        /// <summary>
        /// 收货库位
        /// </summary>
        public string ReceiveStorageLocation
        {
            get { return GetProperty(ReceiveStorageLocationProperty); }
            set { SetProperty(ReceiveStorageLocationProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<AsnDetailInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 制单日期 BillDate
        /// <summary>
        /// 制单日期
        /// </summary>
        [Label("制单日期")]
        public static readonly Property<DateTime> BillDateProperty = P<AsnDetailInf>.Register(e => e.BillDate);

        /// <summary>
        /// 制单日期
        /// </summary>
        public DateTime BillDate
        {
            get { return GetProperty(BillDateProperty); }
            set { SetProperty(BillDateProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<AsnDetailInf>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 需求日期 RequestDate
        /// <summary>
        /// 需求日期
        /// </summary>
        [Label("需求日期")]
        public static readonly Property<DateTime?> RequestDateProperty = P<AsnDetailInf>.Register(e => e.RequestDate);

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate
        {
            get { return GetProperty(RequestDateProperty); }
            set { SetProperty(RequestDateProperty, value); }
        }
        #endregion

        #region 物料单位 ItemUnit
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("物料单位")]
        public static readonly Property<string> ItemUnitProperty = P<AsnDetailInf>.Register(e => e.ItemUnit);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit
        {
            get { return GetProperty(ItemUnitProperty); }
            set { SetProperty(ItemUnitProperty, value); }
        }
        #endregion

        #region 采购订单号 PoNo
        /// <summary>
        /// 注释
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PoNoProperty = P<AsnDetailInf>.Register(e => e.PoNo);

        /// <summary>
        /// 注释
        /// </summary>
        public string PoNo
        {
            get { return this.GetProperty(PoNoProperty); }
            set { this.SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购订单行号 PoLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单行号")]
        public static readonly Property<string> PoLineNoProperty = P<AsnDetailInf>.Register(e => e.PoLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PoLineNo
        {
            get { return this.GetProperty(PoLineNoProperty); }
            set { this.SetProperty(PoLineNoProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Required]
        [Label("物料批次号")]
        [MaxLength(80)]
        public static readonly Property<string> LotProperty = P<AsnDetailInf>.Register(e => e.Lot);

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot
        {
            get { return GetProperty(LotProperty); }
            set { SetProperty(LotProperty, value); }
        }
        #endregion

        #region 批次属性01 LotAtt01
        /// <summary>
        /// 批次属性01
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> LotAtt01Property = P<AsnDetailInf>.Register(e => e.LotAtt01);

        /// <summary>
        /// 批次属性01
        /// </summary>
        public DateTime? LotAtt01
        {
            get { return GetProperty(LotAtt01Property); }
            set { SetProperty(LotAtt01Property, value); }
        }
        #endregion

        #region 批次属性02 LotAtt02
        /// <summary>
        /// 批次属性02
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateTime?> LotAtt02Property = P<AsnDetailInf>.Register(e => e.LotAtt02);

        /// <summary>
        /// 批次属性02
        /// </summary>
        public DateTime? LotAtt02
        {
            get { return GetProperty(LotAtt02Property); }
            set { SetProperty(LotAtt02Property, value); }
        }
        #endregion

        #region 批次属性04 LotAtt04
        /// <summary>
        /// 批次属性04
        /// </summary>
        [Label("生产批次")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt04Property = P<AsnDetailInf>.Register(e => e.LotAtt04);

        /// <summary>
        /// 批次属性04
        /// </summary>
        public string LotAtt04
        {
            get { return GetProperty(LotAtt04Property); }
            set { SetProperty(LotAtt04Property, value); }
        }
        #endregion

        #region 批次属性05 LotAtt05
        /// <summary>
        /// 批次属性05
        /// </summary>
        [Label("批次属性05")]
        public static readonly Property<decimal?> LotAtt05Property = P<AsnDetailInf>.Register(e => e.LotAtt05);

        /// <summary>
        /// 批次属性05
        /// </summary>
        public decimal? LotAtt05
        {
            get { return GetProperty(LotAtt05Property); }
            set { SetProperty(LotAtt05Property, value); }
        }
        #endregion

        #region 批次属性06 LotAtt06
        /// <summary>
        /// 批次属性06
        /// </summary>
        [Label("批次属性06")]
        public static readonly Property<decimal?> LotAtt06Property = P<AsnDetailInf>.Register(e => e.LotAtt06);

        /// <summary>
        /// 批次属性06
        /// </summary>
        public decimal? LotAtt06
        {
            get { return GetProperty(LotAtt06Property); }
            set { SetProperty(LotAtt06Property, value); }
        }
        #endregion

        #region 批次属性07 LotAtt07
        /// <summary>
        /// 批次属性07
        /// </summary>
        [Label("批次属性07")]
        public static readonly Property<bool?> LotAtt07Property = P<AsnDetailInf>.Register(e => e.LotAtt07);

        /// <summary>
        /// 批次属性07
        /// </summary>
        public bool? LotAtt07
        {
            get { return GetProperty(LotAtt07Property); }
            set { SetProperty(LotAtt07Property, value); }
        }
        #endregion

        #region 批次属性08 LotAtt08
        /// <summary>
        /// 批次属性08
        /// </summary>
        [Label("批次属性08")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt08Property = P<AsnDetailInf>.Register(e => e.LotAtt08);

        /// <summary>
        /// 批次属性08
        /// </summary>
        public string LotAtt08
        {
            get { return GetProperty(LotAtt08Property); }
            set { SetProperty(LotAtt08Property, value); }
        }
        #endregion

        #region 批次属性09 LotAtt09
        /// <summary>
        /// 批次属性09
        /// </summary>
        [Label("批次属性09")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt09Property = P<AsnDetailInf>.Register(e => e.LotAtt09);

        /// <summary>
        /// 批次属性09
        /// </summary>
        public string LotAtt09
        {
            get { return GetProperty(LotAtt09Property); }
            set { SetProperty(LotAtt09Property, value); }
        }
        #endregion

        #region 批次属性10 LotAtt10
        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt10Property = P<AsnDetailInf>.Register(e => e.LotAtt10);

        /// <summary>
        /// 批次属性10
        /// </summary>
        public string LotAtt10
        {
            get { return GetProperty(LotAtt10Property); }
            set { SetProperty(LotAtt10Property, value); }
        }
        #endregion

        #region 批次属性11 LotAtt11
        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        public static readonly Property<DateTime?> LotAtt11Property = P<AsnDetailInf>.Register(e => e.LotAtt11);

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt11
        {
            get { return GetProperty(LotAtt11Property); }
            set { SetProperty(LotAtt11Property, value); }
        }
        #endregion

        #region 批次属性12 LotAtt12
        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        public static readonly Property<DateTime?> LotAtt12Property = P<AsnDetailInf>.Register(e => e.LotAtt12);

        /// <summary>
        /// 批次属性12
        /// </summary>
        public DateTime? LotAtt12
        {
            get { return GetProperty(LotAtt12Property); }
            set { SetProperty(LotAtt12Property, value); }
        }
        #endregion
    }

    /// <summary>
    /// ASN单明细中间表 实体配置
    /// </summary>
    internal class AsnDetailInfConfig : EntityConfig<AsnDetailInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_ASN_DTL").MapAllProperties();
            Meta.Property(AsnDetailInf.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}