using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 扫描信息
    /// </summary>
    [Serializable]
    public class PdaScanInfo
    {

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }


        /// <summary>
        /// 派工单Id
        /// </summary>
        public double? DispatchTaskId { get; set; }


        /// <summary>
        /// SN
        /// </summary>
        public string Sn { get; set; }


        /// <summary>
        /// 扫码类型 1、报工模式  2转入模式
        /// </summary>
        public int ScanType { get; set; }

        /// <summary>
        /// 是否扫描第一个SN
        /// </summary>
        public bool IsFirstSn { get; set; }


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
        /// 功能类型  1.工序扫码
        /// </summary>
        public int? Type { get; set; }
    }
}
