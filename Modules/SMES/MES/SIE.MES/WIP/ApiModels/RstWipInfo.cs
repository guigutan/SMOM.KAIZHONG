using System;

namespace SIE.MES.WIP.ApiModels
{
    /// <summary>
    /// 采集返回信息
    /// </summary>
    [Serializable]
    public class RstWipInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductModel { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }
    }

    /// <summary>
    /// 采集查询信息
    /// </summary>
    [Serializable]
    public class WipQueryInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public int ProcessType { get; set; }
    }
}
