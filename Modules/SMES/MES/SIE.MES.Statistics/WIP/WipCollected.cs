using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 采集数据
    /// </summary>
    public class WipCollected
    {
        /// <summary>
        /// 工序顺序
        /// </summary>
        public int ProcessIndex { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType Type { get; set; }

        /// <summary>
        /// 库存组织ID
        /// </summary>
        public int? InvOrgId { get; set; }

        /// <summary>
        /// 采集时间，数据库时间
        /// </summary>
        public DateTime CollectDate { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 机型ID
        /// </summary>
        public double? ModelId { get; set; }

        /// <summary>
        /// 机型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipmentId { get; set; }

        /// <summary>
        /// 产品数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 批次产品数量
        /// </summary>
        public decimal BatchQty { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrap { get; set; }

        /// <summary>
        /// 是否结束工序
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 是否开始工序
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 产品是否不良，维修后也是不良
        /// </summary>
        public bool IsNg { get; set; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType ProcessType { get; set; }

        /// <summary>
        /// 缺陷
        /// </summary>
        public List<DefectData> DefectList { get; set; } = new List<DefectData>();
    }
}