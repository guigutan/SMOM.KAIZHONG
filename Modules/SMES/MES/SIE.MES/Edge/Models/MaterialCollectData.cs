using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 物料采集数据实体
    /// </summary>
    [Serializable]
    public class MaterialCollectData
    {

        /// <summary>
        /// 工单Id
        /// </summary>
        public string WorkOrderId { get; set; }

        /// <summary>
        /// 生产条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemId { get; set; }


        /// <summary>
        /// 过站工序
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string EmployeeId { get; set; }


        /// <summary>
        /// 数据类型
        /// </summary>
        public MaterialDataType DataType { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public SingleLabels.LoadItemSourceType? SourceType { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CreationDate { get;set; }

    }
}
