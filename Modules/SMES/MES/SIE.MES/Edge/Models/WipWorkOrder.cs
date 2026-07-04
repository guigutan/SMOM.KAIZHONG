using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 在制工单类
    /// </summary>
    [Serializable]
    public class WipWorkOrder
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public string WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品机型
        /// </summary>
        public string ProductModel { get; set; }

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public string ProductModelId { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string LineId { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public int IsPause { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 完工数
        /// </summary>
        public decimal FinishedQty { get; set; }

        /// <summary>
        /// 上线数
        /// </summary>
        public decimal OnlineQty { get; set; }

        /// <summary>
        /// 采集运行时工序
        /// </summary>
        public List<EdgeRouteProcess> ProcesseList { get; } = new List<EdgeRouteProcess>();

        /// <summary>
        /// 工序工位信息
        /// "1235": {"Sequence": 0, "Stations": {"1": "ST001", "2": "ST002"}, "Employees": {"1":"emp_name01","2":"emp_name02"}}
        /// </summary>
        public List<EdgeProcessStation> ProcessStationList { get; } = new List<EdgeProcessStation>();

        /// <summary>
        /// 条码信息
        /// </summary>
        public List<EdgeBarcode> BarcodeList { get; } = new List<EdgeBarcode>();

        /// <summary>
        /// 包装规则
        /// </summary>
        public List<EdgePackRule> PackRuleList { get; } = new List<EdgePackRule>();

        /// <summary>
        /// 包装号信息
        /// </summary>
        public List<EdgePackingBarcode> PackBarcodeList { get; } = new List<EdgePackingBarcode>();
    }
}
