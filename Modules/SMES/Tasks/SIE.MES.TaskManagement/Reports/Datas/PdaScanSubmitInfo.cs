using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 扫描提交信息
    /// </summary>
    [Serializable]
    public class PdaScanSubmitInfo
    {

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }


        /// <summary>
        /// 派工单Id
        /// </summary>
        public double DispatchTaskId { get; set; }


        /// <summary>
        /// 扫码类型 1、报工模式  2转入模式
        /// </summary>
        public int ScanType { get; set; }


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
        /// 报工员Id
        /// </summary>
        public double ReportEmployeeId { get; set; }

        /// <summary>
        /// 是否跳过前工序报工数量验证
        /// </summary>
        public bool IsSkipValidatePreQty { get; set; }

        /// <summary>
        /// 扫描内容列表
        /// </summary>
        public List<ScanDetailInfo> DetailInfos=new List<ScanDetailInfo>();

        /// <summary>
        /// 是否完工(只有手动、扫码弹窗选择的时候否的时候才输入false)
        /// </summary>
        public bool IsTaskFinish { get; set; }
        /// <summary>
        /// 是否自动上料(适用于成品工单包装报工)
        /// </summary>
        public bool IsAutoFeeding { get; set; }

    }

    /// <summary>
    /// 扫描明细
    /// </summary>
    [Serializable]
    public class ScanDetailInfo
    {

        /// <summary>
        /// SN
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量(标签数量)
        /// </summary>
        public decimal Qty { get; set; }


        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty { get; set; } 

        /// <summary>
        /// 良品数
        /// </summary>
        public decimal GoodQty { get; set; }
    }
}
