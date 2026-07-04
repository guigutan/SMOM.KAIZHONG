using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 采购订单明细中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("采购订单明细中间表")]
    public partial class PurchaseOrderDetailInf : DownloadBaseEntity
    {
        #region 采购订单号 PoNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PoNoProperty = P<PurchaseOrderDetailInf>.Register(e => e.PoNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PoNo
        {
            get { return GetProperty(PoNoProperty); }
            set { SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购订单ERP主键 PoErpKey
        /// <summary>
        /// 采购订单ERP主键
        /// </summary>
        [Label("采购订单ERP主键")]
        public static readonly Property<string> PoErpKeyProperty = P<PurchaseOrderDetailInf>.Register(e => e.PoErpKey);

        /// <summary>
        /// 采购订单ERP主键
        /// </summary>
        public string PoErpKey
        {
            get { return this.GetProperty(PoErpKeyProperty); }
            set { this.SetProperty(PoErpKeyProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<PurchaseOrderDetailInf>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 采购数量 Quantity
        /// <summary>
        /// 采购数量
        /// </summary>
        [Label("采购数量")]
        public static readonly Property<decimal> QuantityProperty = P<PurchaseOrderDetailInf>.Register(e => e.Quantity);

        /// <summary>
        /// 采购数量
        /// </summary>
        public decimal Quantity
        {
            get { return GetProperty(QuantityProperty); }
            set { SetProperty(QuantityProperty, value); }
        }
        #endregion

        #region 采购单价 UnitPrice
        /// <summary>
        /// 采购单价
        /// </summary>
        [Label("采购单价")]
        public static readonly Property<decimal> UnitPriceProperty = P<PurchaseOrderDetailInf>.Register(e => e.UnitPrice);

        /// <summary>
        /// 采购单价
        /// </summary>
        public decimal UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime> DeliveryDateProperty = P<PurchaseOrderDetailInf>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PurchaseOrderDetailInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料ERP主键 ItemErpKey
        /// <summary>
        /// 物料ERP主键
        /// </summary>
        [Label("物料ERP主键")]
        public static readonly Property<string> ItemErpKeyProperty = P<PurchaseOrderDetailInf>.Register(e => e.ItemErpKey);

        /// <summary>
        /// 物料ERP主键
        /// </summary>
        public string ItemErpKey
        {
            get { return this.GetProperty(ItemErpKeyProperty); }
            set { this.SetProperty(ItemErpKeyProperty, value); }
        }
        #endregion

        #region 采购单位 PurchaseUnit
        /// <summary>
        /// 采购单位
        /// </summary>
        [Label("采购单位")]
        public static readonly Property<string> PurchaseUnitProperty = P<PurchaseOrderDetailInf>.Register(e => e.PurchaseUnit);

        /// <summary>
        /// 采购单位
        /// </summary>
        public string PurchaseUnit
        {
            get { return GetProperty(PurchaseUnitProperty); }
            set { SetProperty(PurchaseUnitProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<PurchaseOrderDetailInf>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<PurchaseOrderDetailInf>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 关闭日期 ClosedDate
        /// <summary>
        /// 关闭日期
        /// </summary>
        [Label("关闭日期")]
        public static readonly Property<DateTime?> ClosedDateProperty = P<PurchaseOrderDetailInf>.Register(e => e.ClosedDate);

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
        public static readonly Property<string> ClosedFlagProperty = P<PurchaseOrderDetailInf>.Register(e => e.ClosedFlag);

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
        public static readonly Property<DateTime?> CancelDateProperty = P<PurchaseOrderDetailInf>.Register(e => e.CancelDate);

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
        public static readonly Property<string> CancelFlagProperty = P<PurchaseOrderDetailInf>.Register(e => e.CancelFlag);

        /// <summary>
        /// 取消标记
        /// </summary>
        public string CancelFlag
        {
            get { return GetProperty(CancelFlagProperty); }
            set { SetProperty(CancelFlagProperty, value); }
        }
        #endregion

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因
        /// </summary>
        [MaxLength(1000)]
        [Label("取消原因")]
        public static readonly Property<string> CancelReasonProperty = P<PurchaseOrderDetailInf>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason
        {
            get { return GetProperty(CancelReasonProperty); }
            set { SetProperty(CancelReasonProperty, value); }
        }
        #endregion

        #region 更新人编码 UpdateByCode
        /// <summary>
        /// 更新人编码
        /// </summary>
        [Label("更新人编码")]
        public static readonly Property<string> UpdateByCodeProperty = P<PurchaseOrderDetailInf>.Register(e => e.UpdateByCode);

        /// <summary>
        /// 更新人编码
        /// </summary>
        public string UpdateByCode
        {
            get { return GetProperty(UpdateByCodeProperty); }
            set { SetProperty(UpdateByCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 采购订单明细中间表 实体配置
    /// </summary>
    internal class PurchaseOrderDetailInfConfig : EntityConfig<PurchaseOrderDetailInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PO_DTL").MapAllProperties();
            Meta.Property(PurchaseOrderDetailInf.CancelReasonProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}