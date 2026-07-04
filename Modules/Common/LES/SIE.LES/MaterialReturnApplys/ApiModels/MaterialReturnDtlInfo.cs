using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys.ApiModels
{
    /// <summary>
    /// 退料申请明细
    /// </summary>
    [Serializable]
    public class MaterialReturnDtlInfo
    {
        /// <summary>
        /// 退料申请Id
        /// </summary>
        public double MaterialReturnApplyId { get; set; }

        /// <summary>
        /// 退料申请单号
        /// </summary>
        public string ReturnApplyNo { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 工单需求
        /// </summary>
        public double? WoDemandReportId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public double? ItemLabelId { get; set; }

        /// <summary>
        /// 良品状态 0-良品 1-不良品
        /// </summary>
        public int ReDetailQuality { get; set; }
    }
}
