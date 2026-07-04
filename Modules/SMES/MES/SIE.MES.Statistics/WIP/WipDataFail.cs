using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 采集预统计失败数据表
    /// </summary>
    [RootEntity, Serializable]
    [Label("采集预统计失败数据表，已汇总数据")]
    public partial class WipDataFail : DataEntity
    {
        #region 工序序号 ProcessIndex
        /// <summary>
        /// 工序序号
        /// </summary>
        [Label("工序序号")]
        public static readonly Property<int> ProcessIndexProperty = P<WipDataFail>.Register(e => e.ProcessIndex);

        /// <summary>
        /// 工序序号
        /// </summary>
        public int ProcessIndex
        {
            get { return this.GetProperty(ProcessIndexProperty); }
            set { this.SetProperty(ProcessIndexProperty, value); }
        }
        #endregion


        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<WipDataFail>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Required]
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<WipDataFail>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return GetProperty(WorkOrderIdProperty); }
            set { SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipDataFail>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 客户Id CustomerId
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户Id")]
        public static readonly Property<double> CustomerIdProperty = P<WipDataFail>.Register(e => e.CustomerId);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double CustomerId
        {
            get { return GetProperty(CustomerIdProperty); }
            set { SetProperty(CustomerIdProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<WipDataFail>.Register(e => e.CustomerName);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkShopIdProperty = P<WipDataFail>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return GetProperty(WorkShopIdProperty); }
            set { SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<WipDataFail>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double> ResourceIdProperty = P<WipDataFail>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<WipDataFail>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<WipDataFail>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return GetProperty(ProcessIdProperty); }
            set { SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<WipDataFail>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工位Id StationId
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位Id")]
        public static readonly Property<double> StationIdProperty = P<WipDataFail>.Register(e => e.StationId);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return GetProperty(StationIdProperty); }
            set { SetProperty(StationIdProperty, value); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<WipDataFail>.Register(e => e.StationName);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return GetProperty(StationNameProperty); }
            set { SetProperty(StationNameProperty, value); }
        }
        #endregion

        #region 设备Id EquipmentId
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备Id")]
        public static readonly Property<double> EquipmentIdProperty = P<WipDataFail>.Register(e => e.EquipmentId);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipmentId
        {
            get { return GetProperty(EquipmentIdProperty); }
            set { SetProperty(EquipmentIdProperty, value); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<WipDataFail>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return GetProperty(EquipmentNameProperty); }
            set { SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 操作人Id OperatorId
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人Id")]
        public static readonly Property<double> OperatorIdProperty = P<WipDataFail>.Register(e => e.OperatorId);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperatorId
        {
            get { return GetProperty(OperatorIdProperty); }
            set { SetProperty(OperatorIdProperty, value); }
        }
        #endregion

        #region 操作人名称 OperatorName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人名称")]
        public static readonly Property<string> OperatorNameProperty = P<WipDataFail>.Register(e => e.OperatorName);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName
        {
            get { return GetProperty(OperatorNameProperty); }
            set { SetProperty(OperatorNameProperty, value); }
        }
        #endregion

        #region 班次Id ShiftId
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次Id")]
        public static readonly Property<double> ShiftIdProperty = P<WipDataFail>.Register(e => e.ShiftId);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return GetProperty(ShiftIdProperty); }
            set { SetProperty(ShiftIdProperty, value); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次名称")]
        public static readonly Property<string> ShiftNameProperty = P<WipDataFail>.Register(e => e.ShiftName);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return GetProperty(ShiftNameProperty); }
            set { SetProperty(ShiftNameProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<WipDataFail>.Register(e => e.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return GetProperty(ProductIdProperty); }
            set { SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WipDataFail>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 产品机型ID ModelId
        /// <summary>
        /// 产品机型ID
        /// </summary>
        [Label("产品机型ID")]
        public static readonly Property<double?> ModelIdProperty = P<WipDataFail>.Register(e => e.ModelId);

        /// <summary>
        /// 产品机型ID
        /// </summary>
        public double? ModelId
        {
            get { return GetProperty(ModelIdProperty); }
            set { SetProperty(ModelIdProperty, value); }
        }
        #endregion

        #region 产品机型名称 ModelName
        /// <summary>
        /// 产品机型名称
        /// </summary>
        [Label("产品机型名称")]
        public static readonly Property<string> ModelNameProperty = P<WipDataFail>.Register(e => e.ModelName);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ModelName
        {
            get { return GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 采集时间 CollectDate
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime> CollectDateProperty = P<WipDataFail>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectDate
        {
            get { return GetProperty(CollectDateProperty); }
            set { SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 班次时间 ShiftDate
        /// <summary>
        /// 班次时间
        /// </summary>
        [Label("班次时间")]
        public static readonly Property<DateTime> ShiftDateProperty = P<WipDataFail>.Register(e => e.ShiftDate);

        /// <summary>
        /// 班次时间
        /// </summary>
        public DateTime ShiftDate
        {
            get { return GetProperty(ShiftDateProperty); }
            set { SetProperty(ShiftDateProperty, value); }
        }
        #endregion

        #region 小时 Hour
        /// <summary>
        /// 小时
        /// </summary>
        [Label("小时")]
        public static readonly Property<int> HourProperty = P<WipDataFail>.Register(e => e.Hour);

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour
        {
            get { return GetProperty(HourProperty); }
            set { SetProperty(HourProperty, value); }
        }
        #endregion

        #region 工单成功数 QtyPass
        /// <summary>
        /// 工单成功数
        /// </summary>
        [Label("工单成功数")]
        public static readonly Property<decimal> QtyPassProperty = P<WipDataFail>.Register(e => e.QtyPass);

        /// <summary>
        /// 工单成功数
        /// </summary>
        public decimal QtyPass
        {
            get { return GetProperty(QtyPassProperty); }
            set { SetProperty(QtyPassProperty, value); }
        }
        #endregion

        #region 工单失败数 QtyFailed
        /// <summary>
        /// 工单失败数
        /// </summary>
        [Label("工单失败数")]
        public static readonly Property<decimal> QtyFailedProperty = P<WipDataFail>.Register(e => e.QtyFailed);

        /// <summary>
        /// 工单失败数
        /// </summary>
        public decimal QtyFailed
        {
            get { return GetProperty(QtyFailedProperty); }
            set { SetProperty(QtyFailedProperty, value); }
        }
        #endregion

        #region 工单上线数 QtyOnline
        /// <summary>
        /// 工单上线数
        /// </summary>
        [Label("工单上线数")]
        public static readonly Property<decimal> QtyOnlineProperty = P<WipDataFail>.Register(e => e.QtyOnline);

        /// <summary>
        /// 工单上线数
        /// </summary>
        public decimal QtyOnline
        {
            get { return GetProperty(QtyOnlineProperty); }
            set { SetProperty(QtyOnlineProperty, value); }
        }
        #endregion

        #region 是否报废 IsScrap
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrapProperty = P<WipDataFail>.Register(e => e.IsScrap);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrap
        {
            get { return GetProperty(IsScrapProperty); }
            set { SetProperty(IsScrapProperty, value); }
        }
        #endregion

        #region 数据来源库存组织 SourceInvOrgId
        /// <summary>
        /// 数据来源库存组织
        /// </summary>
        [Label("数据来源库存组织")]
        public static readonly Property<int?> SourceInvOrgIdProperty = P<WipDataFail>.Register(e => e.SourceInvOrgId);

        /// <summary>
        /// 数据来源库存组织
        /// </summary>
        public int? SourceInvOrgId
        {
            get { return GetProperty(SourceInvOrgIdProperty); }
            set { SetProperty(SourceInvOrgIdProperty, value); }
        }
        #endregion

        #region 工位信息-第一次过站成功数 S_QtyPass
        /// <summary>
        /// 工位信息-第一次过站成功数
        /// </summary>
        [Label("工位信息-第一次过站成功数")]
        public static readonly Property<decimal> S_QtyPassProperty = P<WipDataFail>.Register(e => e.S_QtyPass);

        /// <summary>
        /// 工位信息-第一次过站成功数
        /// </summary>
        public decimal S_QtyPass
        {
            get { return GetProperty(S_QtyPassProperty); }
            set { SetProperty(S_QtyPassProperty, value); }
        }
        #endregion

        #region 工位信息-失败数（第一次过站） S_QtyFailed
        /// <summary>
        /// 工位信息-失败数（第一次过站）
        /// </summary>
        [Label("工位信息-失败数（第一次过站）")]
        public static readonly Property<decimal> S_QtyFailedProperty = P<WipDataFail>.Register(e => e.S_QtyFailed);

        /// <summary>
        /// 工位信息-失败数（第一次过站）
        /// </summary>
        public decimal S_QtyFailed
        {
            get { return GetProperty(S_QtyFailedProperty); }
            set { SetProperty(S_QtyFailedProperty, value); }
        }
        #endregion

        #region 工位信息-重复过站成功数 S_QtyTimes
        /// <summary>
        /// 工位信息-重复过站成功数
        /// </summary>
        [Label("工位信息-重复过站成功数")]
        public static readonly Property<decimal> S_QtyTimesProperty = P<WipDataFail>.Register(e => e.S_QtyTimes);

        /// <summary>
        /// 工位信息-重复过站成功数
        /// </summary>
        public decimal S_QtyTimes
        {
            get { return GetProperty(S_QtyTimesProperty); }
            set { SetProperty(S_QtyTimesProperty, value); }
        }
        #endregion

        #region 工位信息-重复过站失败数 S_QtyFailedTimes
        /// <summary>
        /// 工位信息-重复过站失败数
        /// </summary>
        [Label("工位信息-重复过站失败数")]
        public static readonly Property<decimal> S_QtyFailedTimesProperty = P<WipDataFail>.Register(e => e.S_QtyFailedTimes);

        /// <summary>
        /// 工位信息-重复过站失败数
        /// </summary>
        public decimal S_QtyFailedTimes
        {
            get { return GetProperty(S_QtyFailedTimesProperty); }
            set { SetProperty(S_QtyFailedTimesProperty, value); }
        }
        #endregion

        #region 工位信息-过站数 S_QtyMove
        /// <summary>
        /// 工位信息-过站数
        /// </summary>
        [Label("工位信息-过站数")]
        public static readonly Property<decimal> S_QtyMoveProperty = P<WipDataFail>.Register(e => e.S_QtyMove);

        /// <summary>
        /// 工位信息-过站数
        /// </summary>
        public decimal S_QtyMove
        {
            get { return GetProperty(S_QtyMoveProperty); }
            set { SetProperty(S_QtyMoveProperty, value); }
        }
        #endregion

        #region 工序信息-成功数 P_QtyPass
        /// <summary>
        /// 工序信息-成功数
        /// </summary>
        [Label("工序信息-成功数")]
        public static readonly Property<decimal> P_QtyPassProperty = P<WipDataFail>.Register(e => e.P_QtyPass);

        /// <summary>
        /// 工序信息-成功数
        /// </summary>
        public decimal P_QtyPass
        {
            get { return GetProperty(P_QtyPassProperty); }
            set { SetProperty(P_QtyPassProperty, value); }
        }
        #endregion

        #region 工序信息-失败数 P_QtyFailed
        /// <summary>
        /// 工序信息-失败数
        /// </summary>
        [Label("工序信息-失败数")]
        public static readonly Property<decimal> P_QtyFailedProperty = P<WipDataFail>.Register(e => e.P_QtyFailed);

        /// <summary>
        /// 工序信息-失败数
        /// </summary>
        public decimal P_QtyFailed
        {
            get { return GetProperty(P_QtyFailedProperty); }
            set { SetProperty(P_QtyFailedProperty, value); }
        }
        #endregion

        #region 工序信息-过站数 P_QtyMove
        /// <summary>
        /// 工序信息-过站数
        /// </summary>
        [Label("工序信息-过站数")]
        public static readonly Property<decimal> P_QtyMoveProperty = P<WipDataFail>.Register(e => e.P_QtyMove);

        /// <summary>
        /// 工序信息-过站数
        /// </summary>
        public decimal P_QtyMove
        {
            get { return GetProperty(P_QtyMoveProperty); }
            set { SetProperty(P_QtyMoveProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 采集数据表 实体配置
    /// </summary>
    internal class WipDataFailConfig : EntityConfig<WipDataFail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DATA_FAIL").MapAllProperties();
            Meta.DisablePhantoms();
            Meta.DisableInvOrg();
            Meta.DisableDataSync();
        }
    }
}