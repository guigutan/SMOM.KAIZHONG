using SIE.Core.Barcodes;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 采集数据
    /// </summary>
    public class WipData
    {
        /// <summary>
        /// 采集数据
        /// </summary>
        public WipData()
        {
            WipStation = new WipStation();
            WipProcess = new WipProcess();
        }

        /// <summary>
        /// 工序索引排序
        /// </summary>
        public int ProcessIndex { get; set; }

        /// <summary>
        /// 工位信息
        /// </summary>
        public WipStation WipStation { get; set; }

        /// <summary>
        /// 工序信息
        /// </summary>
        public WipProcess WipProcess { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType Type { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double CustomerId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 采集名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipmentId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public double OperatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品机型ID
        /// </summary>
        public double? ModelId { get; set; }

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 采集时间，采集过站传过来的数据库时间
        /// </summary>
        public DateTime CollectDate { get; set; }

        /// <summary>
        /// 班次时间
        /// </summary>
        public DateTime ShiftDate { get; set; }

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 工单成功数
        /// </summary>
        public decimal QtyPass { get; set; }

        /// <summary>
        /// 工单失败数
        /// </summary>
        public decimal QtyFailed { get; set; }

        /// <summary>
        /// 工单上线数
        /// </summary>
        public decimal QtyOnline { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrap { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId { get; set; }
    }

    /// <summary>
    /// 工位信息
    /// </summary>
    public class WipStation
    {
        /// <summary>
        /// 成功数（第一次过站）
        /// </summary>
        public decimal QtyPass { get; set; }

        /// <summary>
        /// 失败数（第一次过站）
        /// </summary>
        public decimal QtyFailed { get; set; }

        /// <summary>
        /// 重复过站成功数
        /// </summary>
        public decimal QtyTimes { get; set; }

        /// <summary>
        /// 重复过站失败数
        /// </summary>
        public decimal QtyFailedTimes { get; set; }

        /// <summary>
        /// 过站数（不计重复过站）
        /// </summary>
        public decimal QtyMove { get; set; }
    }

    /// <summary>
    /// 工序信息
    /// </summary>
    public class WipProcess
    {
        /// <summary>
        /// 成功数
        /// </summary>
        public decimal QtyPass { get; set; }

        /// <summary>
        /// 失败数
        /// </summary>
        public decimal QtyFailed { get; set; }

        /// <summary>
        /// 过站数
        /// </summary>
        public decimal QtyMove { get; set; }
    }
}