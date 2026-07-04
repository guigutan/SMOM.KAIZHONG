using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 采集统计预处理失败数据
    /// </summary>
    [RootEntity, Serializable]
    [Label("采集统计预处理失败数据，原始数据")]
    public partial class WipCollectedFail : DataEntity
    {
        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<WipCollectedFail>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 数据来源库存组织ID SourceInvOrgId
        /// <summary>
        /// 数据来源库存组织ID
        /// </summary>
        [Label("数据来源库存组织ID")]
        public static readonly Property<int?> SourceInvOrgIdProperty = P<WipCollectedFail>.Register(e => e.SourceInvOrgId);

        /// <summary>
        /// 数据来源库存组织
        /// </summary>
        public int? SourceInvOrgId
        {
            get { return this.GetProperty(SourceInvOrgIdProperty); }
            set { this.SetProperty(SourceInvOrgIdProperty, value); }
        }
        #endregion

        #region 采集时间，数据库时间 CollectDate
        /// <summary>
        /// 采集时间，数据库时间
        /// </summary>
        [Label("采集时间，数据库时间")]
        public static readonly Property<DateTime> CollectDateProperty = P<WipCollectedFail>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集时间，数据库时间
        /// </summary>
        public DateTime CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<WipCollectedFail>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipCollectedFail>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<WipCollectedFail>.Register(e => e.CustomerName);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { this.SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 机型ID ModelId
        /// <summary>
        /// 机型ID
        /// </summary>
        [Label("机型ID")]
        public static readonly Property<double?> ModelIdProperty = P<WipCollectedFail>.Register(e => e.ModelId);

        /// <summary>
        /// 机型ID
        /// </summary>
        public double? ModelId
        {
            get { return this.GetProperty(ModelIdProperty); }
            set { this.SetProperty(ModelIdProperty, value); }
        }
        #endregion

        #region 机型名称 ModelName
        /// <summary>
        /// 机型名称
        /// </summary>
        [Label("机型名称")]
        public static readonly Property<string> ModelNameProperty = P<WipCollectedFail>.Register(e => e.ModelName);

        /// <summary>
        /// 机型名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<WipCollectedFail>.Register(e => e.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WipCollectedFail>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 员工ID EmployeeId
        /// <summary>
        /// 员工ID
        /// </summary>
        [Label("员工ID")]
        public static readonly Property<double> EmployeeIdProperty = P<WipCollectedFail>.Register(e => e.EmployeeId);

        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId
        {
            get { return this.GetProperty(EmployeeIdProperty); }
            set { this.SetProperty(EmployeeIdProperty, value); }
        }
        #endregion

        #region 资源ID ResourceId
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源ID")]
        public static readonly Property<double> ResourceIdProperty = P<WipCollectedFail>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<WipCollectedFail>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工位ID StationId
        /// <summary>
        /// 工位ID
        /// </summary>
        [Label("工位ID")]
        public static readonly Property<double> StationIdProperty = P<WipCollectedFail>.Register(e => e.StationId);

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId
        {
            get { return this.GetProperty(StationIdProperty); }
            set { this.SetProperty(StationIdProperty, value); }
        }
        #endregion

        #region 设备ID EquipmentId
        /// <summary>
        /// 设备ID
        /// </summary>
        [Label("设备ID")]
        public static readonly Property<double> EquipmentIdProperty = P<WipCollectedFail>.Register(e => e.EquipmentId);

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipmentId
        {
            get { return this.GetProperty(EquipmentIdProperty); }
            set { this.SetProperty(EquipmentIdProperty, value); }
        }
        #endregion

        #region 产品数量 Qty
        /// <summary>
        /// 产品数量
        /// </summary>
        [Label("产品数量")]
        public static readonly Property<decimal> QtyProperty = P<WipCollectedFail>.Register(e => e.Qty);

        /// <summary>
        /// 产品数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 批次产品数量 BatchQty
        /// <summary>
        /// 批次产品数量
        /// </summary>
        [Label("批次产品数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<WipCollectedFail>.Register(e => e.BatchQty);

        /// <summary>
        /// 批次产品数量
        /// </summary>
        public decimal BatchQty
        {
            get { return this.GetProperty(BatchQtyProperty); }
            set { this.SetProperty(BatchQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<WipCollectedFail>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 是否报废 IsScrap
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrapProperty = P<WipCollectedFail>.Register(e => e.IsScrap);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrap
        {
            get { return this.GetProperty(IsScrapProperty); }
            set { this.SetProperty(IsScrapProperty, value); }
        }
        #endregion

        #region 是否开始工序 IsStart
        /// <summary>
        /// 是否开始工序
        /// </summary>
        [Label("是否开始工序")]
        public static readonly Property<bool> IsStartProperty = P<WipCollectedFail>.Register(e => e.IsStart);

        /// <summary>
        /// 是否开始工序
        /// </summary>
        public bool IsStart
        {
            get { return this.GetProperty(IsStartProperty); }
            set { this.SetProperty(IsStartProperty, value); }
        }
        #endregion

        #region 是否结束工序 IsEnd
        /// <summary>
        /// 是否结束工序
        /// </summary>
        [Label("是否结束工序")]
        public static readonly Property<bool> IsEndProperty = P<WipCollectedFail>.Register(e => e.IsEnd);

        /// <summary>
        /// 是否结束工序
        /// </summary>
        public bool IsEnd
        {
            get { return this.GetProperty(IsEndProperty); }
            set { this.SetProperty(IsEndProperty, value); }
        }
        #endregion

        #region 是否不良 IsNg
        /// <summary>
        /// 是否不良
        /// </summary>
        [Label("是否不良")]
        public static readonly Property<bool> IsNgProperty = P<WipCollectedFail>.Register(e => e.IsNg);

        /// <summary>
        /// 是否不良
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly Property<ResultType> ResultProperty = P<WipCollectedFail>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get { return this.GetProperty(ResultProperty); }
            set { this.SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType> ProcessTypeProperty = P<WipCollectedFail>.Register(e => e.ProcessType);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
            set { this.SetProperty(ProcessTypeProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷
        /// </summary>
        [Label("缺陷")]
        public static readonly Property<string> DefectProperty = P<WipCollectedFail>.Register(e => e.Defect);

        /// <summary>
        /// 缺陷
        /// </summary>
        public string Defect
        {
            get { return this.GetProperty(DefectProperty); }
            set { this.SetProperty(DefectProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 采集数据 实体配置
    /// </summary>
    internal class WipCollectedFailConfig : EntityConfig<WipCollectedFail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_COLLECTED_FAIL").MapAllProperties();
            Meta.Property(WipCollectedFail.DefectProperty).ColumnMeta.HasLength("MAX");
            Meta.DisablePhantoms();
            Meta.DisableInvOrg();
            Meta.DisableDataSync();
        }
    }
}