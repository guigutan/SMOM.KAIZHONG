using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 自动报工记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("自动报工记录")]
    public partial class AutoReportRecord : DataEntity
    {
        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<AutoReportRecord>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion  

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<AutoReportRecord>.Register(e => e.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 资源ID WipResourceId
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源ID")]
        public static readonly Property<double> WipResourceIdProperty = P<AutoReportRecord>.Register(e => e.WipResourceId);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double WipResourceId
        {
            get { return this.GetProperty(WipResourceIdProperty); }
            set { this.SetProperty(WipResourceIdProperty, value); }
        }
        #endregion

        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<AutoReportRecord>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 结束工序 IsEndProcess
        /// <summary>
        /// 结束工序 标记下线报工记录
        /// </summary>
        [Label("结束工序")]
        public static readonly Property<bool> IsEndProcessProperty = P<AutoReportRecord>.Register(e => e.IsEndProcess);

        /// <summary>
        /// 结束工序
        /// </summary>
        public bool IsEndProcess
        {
            get { return this.GetProperty(IsEndProcessProperty); }
            set { this.SetProperty(IsEndProcessProperty, value); }
        }
        #endregion

        #region 工位ID StationId
        /// <summary>
        /// 工位ID
        /// </summary>
        [Label("工位ID")]
        public static readonly Property<double> StationIdProperty = P<AutoReportRecord>.Register(e => e.StationId);

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId
        {
            get { return this.GetProperty(StationIdProperty); }
            set { this.SetProperty(StationIdProperty, value); }
        }
        #endregion

        #region 员工Id EmployeeId
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工Id")]
        public static readonly Property<double> EmployeeIdProperty = P<AutoReportRecord>.Register(e => e.EmployeeId);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return this.GetProperty(EmployeeIdProperty); }
            set { this.SetProperty(EmployeeIdProperty, value); }
        }
        #endregion

        #region 生产批次号 WipBatchNo
        /// <summary>
        /// 生产批次号
        /// </summary>
        [Label("生产批次号")]
        public static readonly Property<string> WipBatchNoProperty = P<AutoReportRecord>.Register(e => e.WipBatchNo);

        /// <summary>
        /// 生产批次号
        /// </summary>
        public string WipBatchNo
        {
            get { return this.GetProperty(WipBatchNoProperty); }
            set { this.SetProperty(WipBatchNoProperty, value); }
        }
        #endregion

        #region 总合格数 TotalOkQty
        /// <summary>
        /// 总合格数 已报工+待报工
        /// </summary>
        [Label("总合格数")]
        public static readonly Property<decimal> TotalOkQtyProperty = P<AutoReportRecord>.Register(e => e.TotalOkQty);

        /// <summary>
        /// 总合格数
        /// </summary>
        public decimal TotalOkQty
        {
            get { return this.GetProperty(TotalOkQtyProperty); }
            set { this.SetProperty(TotalOkQtyProperty, value); }
        }
        #endregion

        #region 总不合格数 TotalNgQty
        /// <summary>
        /// 总不合格数 已报工+待报工
        /// </summary>
        [Label("总不合格数")]
        public static readonly Property<decimal> TotalNgQtyProperty = P<AutoReportRecord>.Register(e => e.TotalNgQty);

        /// <summary>
        /// 总不合格数
        /// </summary>
        public decimal TotalNgQty
        {
            get { return this.GetProperty(TotalNgQtyProperty); }
            set { this.SetProperty(TotalNgQtyProperty, value); }
        }
        #endregion

        #region 待报工数量 ToReportQty
        /// <summary>
        /// 待报工数量，未报工
        /// </summary>
        [Label("待报工数量")]
        public static readonly Property<decimal> ToReportQtyProperty = P<AutoReportRecord>.Register(e => e.ToReportQty);

        /// <summary>
        /// 待报工数量
        /// </summary>
        public decimal ToReportQty
        {
            get { return this.GetProperty(ToReportQtyProperty); }
            set { this.SetProperty(ToReportQtyProperty, value); }
        }
        #endregion

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量 ，未报工，报工后减少
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<decimal> OkQtyProperty = P<AutoReportRecord>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量 ，未报工，报工后减少
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<AutoReportRecord>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 条码明细 Details
        /// <summary>
        /// 条码明细
        /// </summary>
        [Label("条码明细")]
        public static readonly ListProperty<EntityList<AutoReportRecordDetail>> DetailsProperty = P<AutoReportRecord>.RegisterList(e => e.Details);

        /// <summary>
        /// 条码明细
        /// </summary>
        public EntityList<AutoReportRecordDetail> Details
        {
            get { return this.GetLazyList(DetailsProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 自动报工记录 实体配置
    /// </summary>
    internal class AutoReportRecordConfig : EntityConfig<AutoReportRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_AUTO_REPORT_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}